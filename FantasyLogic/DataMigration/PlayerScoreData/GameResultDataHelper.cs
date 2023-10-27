using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using FantasyLogic.Calculations;
using FantasyLogic.SharedLogic;
using IntegrationWith365.Entities.GameModels;
using IntegrationWith365.Entities.GamesModels;
using System.ComponentModel;
using static Contracts.EnumData.DBModelsEnum;
using static Contracts.EnumData.HanfireEnum;
using static Entities.EnumData.LogicEnumData;

namespace FantasyLogic.DataMigration.PlayerScoreData
{
    public class GameResultDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;
        private readonly GameResultLogic _gameResultLogic;
        private readonly PlayerStateCalc _playerStateCalc;
        private readonly HangFireCustomJob _hangFireCustomJob;

        private readonly string RecurringJobMatchId = "UpdateMatch-";
        private readonly string JobPlayerId = "UpdateMatchPlayer-";

        public GameResultDataHelper(
            UnitOfWork unitOfWork,
            _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
            _gameResultLogic = new GameResultLogic(unitOfWork);
            _playerStateCalc = new PlayerStateCalc(unitOfWork);
            _hangFireCustomJob = new HangFireCustomJob(unitOfWork);
        }

        public void RunUpdateGameResult(
            _365CompetitionsEnum _365CompetitionsEnum,
            TeamGameWeakParameters parameters,
            bool runBonus,
            bool inDebug,
            bool runAll,
            bool stopAll,
            bool statisticsOnly)
        {
            parameters.Fk_Season = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);

            List<TeamGameWeakForCalc> teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(parameters).Select(a => new TeamGameWeakForCalc
            {
                Id = a.Id,
                _365_MatchId = a._365_MatchId,
                StartTime = a.StartTime,
                Fk_GameWeek = a.Fk_GameWeak,
                Fk_Season = a.GameWeak.Fk_Season,
                IsEnded = a.IsEnded,
                HalfTimeEnded = a.HalfTimeEnded,
            }).ToList();

            List<ScoreTypeForCalc> scoreTypes = _unitOfWork.PlayerScore
                                                       .GetScoreTypes(new ScoreTypeParameters
                                                       {
                                                           HavePoints = true,
                                                       })
                                                       .Select(a => new ScoreTypeForCalc
                                                       {
                                                           Id = a.Id,
                                                           _365_EventTypeId = a._365_EventTypeId,
                                                           _365_TypeId = a._365_TypeId,
                                                       })
                                                       .ToList();

            foreach (TeamGameWeakForCalc teamGameWeak in teamGameWeaks)
            {
                if (inDebug)
                {
                    UpdateGameResult(_365CompetitionsEnum, teamGameWeak, scoreTypes, runBonus, inDebug, runAll, stopAll, statisticsOnly).Wait();
                }
                if (runAll)
                {
                    _ = BackgroundJob.Enqueue(HanfireQueuesEnum.MatchPoints.ToString(), () => UpdateGameResult(_365CompetitionsEnum, teamGameWeak, scoreTypes, runBonus, inDebug, runAll, stopAll, statisticsOnly));
                }
                else
                {
                    if (statisticsOnly == false)
                    {
                        if (runAll || teamGameWeak.EndTime > DateTime.UtcNow.ToEgypt())
                        {
                            RecurringJob.AddOrUpdate(RecurringJobMatchId + teamGameWeak._365_MatchId.ToString(), () => UpdateGameResult(_365CompetitionsEnum, teamGameWeak, scoreTypes, runBonus, inDebug, runAll, stopAll, statisticsOnly), CronExpression.EveryMinutes(5));
                        }
                    }
                }
            }
        }

        public async Task UpdateGameResult(
            _365CompetitionsEnum _365CompetitionsEnum,
            TeamGameWeakForCalc teamGameWeak,
            List<ScoreTypeForCalc> scoreTypes,
            bool runBonus,
            bool inDebug,
            bool runAll,
            bool stopAll,
            bool statisticsOnly)
        {
            GameReturn gameReturn = await _365Services.GetGame(_365CompetitionsEnum, new _365GameParameters
            {
                GameId = teamGameWeak._365_MatchId.ParseToInt()
            });

            bool matchEnded = !inDebug && !runAll && !runBonus && teamGameWeak.IsEnded /*teamGameWeak.EndTime < DateTime.UtcNow.ToEgypt()*/;

            runBonus = runBonus || gameReturn.Game.IsEnded;

            if (statisticsOnly == false)
            {
                if (matchEnded || stopAll)
                {
                    RecurringJob.RemoveIfExists(RecurringJobMatchId + teamGameWeak._365_MatchId.ToString());
                }

                if (runBonus || matchEnded || runAll)
                {
                    if (inDebug)
                    {
                        _playerStateCalc.UpdateTop15(teamGameWeak.Fk_GameWeek, teamGameWeak.Fk_Season);

                    }
                    else
                    {
                        _ = BackgroundJob.Enqueue(HanfireQueuesEnum.MatchPoints.ToString(), () => _playerStateCalc.UpdateTop15(teamGameWeak.Fk_GameWeek, teamGameWeak.Fk_Season));
                    }
                }
            }

            if (gameReturn != null &&
                gameReturn.Game != null &&
                gameReturn.Game.AwayCompetitor != null &&
                gameReturn.Game.HomeCompetitor != null)
            {
                TeamGameWeak match = await _unitOfWork.Season.FindTeamGameWeakbyId(teamGameWeak.Id, trackChanges: true);

                if (statisticsOnly == false)
                {
                    match.LastUpdateId = gameReturn.LastUpdateId;
                    match.IsEnded = gameReturn.Game.IsEnded;
                    match.HalfTimeEnded = gameReturn.Game.HalfTimeEnded;
                    match.AwayScore = (int)gameReturn.Game.AwayCompetitor.Score;
                    match.HomeScore = (int)gameReturn.Game.HomeCompetitor.Score;

                    if (!match.IsCanNotEdit)
                    {
                        match.StartTime = gameReturn.Game.StartTime;
                    }

                    await _unitOfWork.Save();
                }

                if (runBonus || matchEnded || runAll || statisticsOnly)
                {
                    List<StatisticScoreModelForCalc> statisticScores = _unitOfWork.MatchStatistic.GetStatisticScores(new StatisticScoreParameters())
                                                     .Select(a => new StatisticScoreModelForCalc
                                                     {
                                                         Id = a.Id,
                                                         Name = a.Name,
                                                         _365_Id = a._365_Id
                                                     }).ToList();
                    if (inDebug)
                    {
                        await UpdateStatistic(gameReturn.Game.AwayCompetitor.Statistics, statisticScores, teamGameWeak.Id, match.Fk_Away);
                        await UpdateStatistic(gameReturn.Game.HomeCompetitor.Statistics, statisticScores, teamGameWeak.Id, match.Fk_Home);
                    }
                    else
                    {
                        _ = BackgroundJob.Enqueue(HanfireQueuesEnum.MatchPoints.ToString(), () => UpdateStatistic(gameReturn.Game.AwayCompetitor.Statistics, statisticScores, teamGameWeak.Id, match.Fk_Away));
                        _ = BackgroundJob.Enqueue(HanfireQueuesEnum.MatchPoints.ToString(), () => UpdateStatistic(gameReturn.Game.HomeCompetitor.Statistics, statisticScores, teamGameWeak.Id, match.Fk_Home));
                    }
                }
                if (statisticsOnly == false)
                {
                    if (gameReturn.Game.Members != null &&
                        gameReturn.Game.Members.Any())
                    {
                        List<GameMember> allMembers = gameReturn.Game.Members;

                        IQueryable<Player> playersQuary = _unitOfWork.Team.GetPlayers(new PlayerParameters
                        {
                            _365_PlayerIds = allMembers.Select(a => a.AthleteId.ToString()).ToList(),
                        });

                        if (playersQuary.Any())
                        {
                            List<PlayerForCalc> players = playersQuary.Select(a => new PlayerForCalc
                            {
                                Id = a.Id,
                                _365_PlayerId = a._365_PlayerId,
                                Fk_PlayerPosition = a.Fk_PlayerPosition,
                                Fk_Team = a.Fk_Team
                            }).ToList();

                            List<Member> allMembersResults = new();

                            allMembersResults.AddRange(gameReturn.Game.HomeCompetitor.Lineups.Members);
                            allMembersResults.AddRange(gameReturn.Game.AwayCompetitor.Lineups.Members);

                            allMembersResults.ForEach(a =>
                            {
                                a.AthleteId = allMembers.Where(b => b.Id == a.Id)
                                                        .Select(a => a.AthleteId)
                                                        .FirstOrDefault();
                            });

                            allMembersResults = allMembersResults.Where(a => a.Stats != null).ToList();

                            List<double> rankings = runBonus ? allMembersResults.Where(a => a.Ranking > 0)
                                                                                .OrderByDescending(a => a.Ranking)
                                                                                .Skip(0)
                                                                                .Take(3)
                                                                                .Select(a => a.Ranking)
                                                                                .ToList() : null;

                            var membersRanking = runBonus &&
                                                 rankings != null &&
                                                 rankings.Any() ? allMembersResults
                                                                  .Where(a => rankings.Contains(a.Ranking))
                                                                  .Select(a => new
                                                                  {
                                                                      a.Id,
                                                                      index = rankings.IndexOf(a.Ranking) + 1,
                                                                      a.Ranking
                                                                  })
                                                                  .ToList() : null;

                            foreach (GameMember member in allMembers)
                            {
                                PlayerForCalc player = players.Where(a => a._365_PlayerId == member.AthleteId.ToString())
                                                          .Select(a => new PlayerForCalc
                                                          {
                                                              Id = a.Id,
                                                              Fk_PlayerPosition = a.Fk_PlayerPosition,
                                                              Fk_Team = a.Fk_Team
                                                          })
                                                          .SingleOrDefault();
                                if (player != null)
                                {
                                    Member memberResult = allMembersResults.Where(a => a.AthleteId == member.AthleteId)
                                                                           .LastOrDefault();

                                    int rankingIndex = 0;

                                    if (membersRanking != null &&
                                        membersRanking.Any() &&
                                        membersRanking.Any(a => a.Id == member.Id))
                                    {
                                        rankingIndex = membersRanking.Where(a => a.Id == member.Id)
                                                                     .Select(a => a.index)
                                                                     .FirstOrDefault();
                                    }

                                    if (memberResult != null)
                                    {
                                        if (inDebug)
                                        {
                                            UpdatePlayerResult(_365CompetitionsEnum, gameReturn, member, player, rankingIndex, teamGameWeak, memberResult, scoreTypes, match, inDebug).Wait();
                                        }
                                        else
                                        {
                                            string JobPlayerId = this.JobPlayerId + teamGameWeak._365_MatchId.ToString() + $"-{member.Id}";

                                            string hangfireJobId = BackgroundJob.Enqueue(HanfireQueuesEnum.MatchPoints.ToString(), () => UpdatePlayerResult(_365CompetitionsEnum, gameReturn, member, player, rankingIndex, teamGameWeak, memberResult, scoreTypes, match, inDebug));

                                            _hangFireCustomJob.ReplaceJob(hangfireJobId, JobPlayerId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (gameReturn.Game.Members == null ||
                         !gameReturn.Game.Members.Any() ||
                         runBonus ||
                         matchEnded ||
                         runAll)
                    {
                        List<int> players = _unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
                        {
                            Fk_GameWeak = match.Fk_GameWeak,
                            Fk_TeamGameWeak = match.Id
                        }).Select(a => a.PlayerGameWeak.Fk_Player).ToList();

                        if (inDebug)
                        {
                            _playerStateCalc.RunPlayersStateCalculations(_365CompetitionsEnum, match.Fk_GameWeak, match._365_MatchId, players, null, false, inDebug);
                        }
                        else
                        {
                            _ = BackgroundJob.Enqueue(HanfireQueuesEnum.MatchPoints.ToString(), () => _playerStateCalc.RunPlayersStateCalculations(_365CompetitionsEnum, match.Fk_GameWeak, match._365_MatchId, players, null, false, inDebug));
                        }
                    }
                }

            }
        }

        public async Task UpdatePlayerResult(
            _365CompetitionsEnum _365CompetitionsEnum,
            GameReturn gameReturn,
            GameMember member,
            PlayerForCalc player,
            int rankingIndex,
            TeamGameWeakForCalc teamGameWeak,
            Member memberResult,
            List<ScoreTypeForCalc> scoreTypes,
            TeamGameWeak match,
            bool inDebug)
        {
            int substitutionId = 1000;
            int goalId = 1;

            List<EventType> eventResult = new();
            int assistCount = 0;

            if (gameReturn.Game != null && gameReturn.Game.Events != null)
            {
                List<EventType> substitutions = new();
                List<Event> otherGoals = gameReturn.Game
                                    .Events
                                    .Where(a => a.CompetitorId != member.CompetitorId &&
                                                (a.EventType.Id == goalId))
                                    .OrderBy(a => a.GameTime)
                                    .ToList();

                eventResult = gameReturn.Game
                                    .Events
                                    .Where(a => a.PlayerId == member.Id && a.GameTime > 0)
                                    .Select(a => new EventType
                                    {
                                        Id = a.EventType.Id,
                                        Name = a.EventType.Name,
                                        SubTypeId = a.EventType.SubTypeId,
                                        SubTypeName = a.EventType.SubTypeName,
                                        GameTime = a.GameTime,
                                        Value = 1,
                                        ExtraPlayer = (a.ExtraPlayers != null && a.ExtraPlayers.Any()) ? a.ExtraPlayers.First() : 0,
                                        IsOut = (a.EventType.Id == substitutionId && a.ExtraPlayers != null && a.ExtraPlayers.Any()) ? true : null,
                                        IsAssist = ((a.EventType.Id == goalId) && a.ExtraPlayers != null && a.ExtraPlayers.Any()) ? true : null
                                    }).ToList();

                assistCount = gameReturn.Game.Events
                                        .Where(a => a.EventType.Id == goalId && a.ExtraPlayers != null && a.ExtraPlayers.Any() && a.ExtraPlayers.First() == member.Id && a.GameTime > 0)
                                        .Count();

                if (gameReturn.Game.Events.Any(a => a.EventType.Id == substitutionId && a.ExtraPlayers != null && a.ExtraPlayers.Any() && a.ExtraPlayers.First() == member.Id && a.GameTime > 0))
                {
                    EventType eventResultItem = gameReturn.Game
                                                     .Events
                                                     .Where(a => a.EventType.Id == substitutionId && a.ExtraPlayers != null && a.ExtraPlayers.Any() && a.ExtraPlayers.First() == member.Id && a.GameTime > 0)
                                                     .Select(a => new EventType
                                                     {
                                                         Id = a.EventType.Id,
                                                         Name = a.EventType.Name,
                                                         SubTypeId = a.EventType.SubTypeId,
                                                         SubTypeName = a.EventType.SubTypeName,
                                                         GameTime = a.GameTime,
                                                         Value = 1,
                                                         IsOut = true
                                                     }).First();

                    eventResult.Add(eventResultItem);
                }

                if (gameReturn.Game.Events.Any(a => a.EventType.Id == substitutionId &&
                                                    a.GameTime > 0 &&
                                                    ((a.PlayerId == member.Id) ||
                                                     (a.ExtraPlayers != null && a.ExtraPlayers.Any() && a.ExtraPlayers.First() == member.Id))))
                {
                    substitutions.AddRange(gameReturn.Game.Events.Where(a => a.EventType.Id == substitutionId &&
                                                    a.GameTime > 0 &&
                                                    ((a.PlayerId == member.Id) ||
                                                     (a.ExtraPlayers != null && a.ExtraPlayers.Any() && a.ExtraPlayers.First() == member.Id)))
                                                      .Select(a => new EventType
                                                      {
                                                          Id = a.EventType.Id,
                                                          Name = a.EventType.Name,
                                                          SubTypeId = a.EventType.SubTypeId,
                                                          SubTypeName = a.EventType.SubTypeName,
                                                          GameTime = a.GameTime,
                                                          Value = 1,
                                                          ExtraPlayer = (a.ExtraPlayers != null && a.ExtraPlayers.Any()) ? a.ExtraPlayers.First() : 0,
                                                          IsOut = a.EventType.Id == substitutionId && a.ExtraPlayers != null && a.ExtraPlayers.Any() && a.ExtraPlayers.First() == member.Id,
                                                      }).ToList());
                }

                PlayMinutesEnum playMinutes = await UpdatePlayerGameResult(otherGoals, substitutions, rankingIndex, player.Id, player.Fk_PlayerPosition, teamGameWeak.Id, memberResult, eventResult, scoreTypes, assistCount);
                await _unitOfWork.Save();

                await UpdateGameFinalResult(_365CompetitionsEnum, otherGoals, substitutions, playMinutes, rankingIndex, match.Id, player.Id, inDebug);
            }
        }

        public async Task<PlayMinutesEnum> UpdatePlayerGameResult(
            List<Event> otherGoals,
            List<EventType> substitutions,
            int rankingIndex,
            int fk_Player,
            int fk_PlayerPosition,
            int fk_TeamGameWeak,
            Member memberResult,
            List<EventType> eventResult,
            List<ScoreTypeForCalc> scoreTypes,
            int assistCount)
        {
            PlayMinutesEnum playMinutes = PlayMinutesEnum.NotPlayed;

            if (fk_Player > 0 && memberResult != null)
            {
                int fk_PlayerGameWeak = 0;
                bool isOld = false;

                if (_unitOfWork.PlayerScore.GetPlayerGameWeaks(new PlayerGameWeakParameters { Fk_TeamGameWeak = fk_TeamGameWeak, Fk_Player = fk_Player }).Any())
                {
                    fk_PlayerGameWeak = _unitOfWork.PlayerScore
                                               .GetPlayerGameWeaks(new PlayerGameWeakParameters { Fk_TeamGameWeak = fk_TeamGameWeak, Fk_Player = fk_Player })
                                               .Select(a => a.Id)
                                               .FirstOrDefault();
                    isOld = true;
                }
                else
                {
                    PlayerGameWeak playerGame = new()
                    {
                        Fk_TeamGameWeak = fk_TeamGameWeak,
                        Fk_Player = fk_Player,
                        Ranking = memberResult.Ranking,
                        _365_PlayerId = memberResult.Id.ToString()
                    };

                    _unitOfWork.PlayerScore.CreatePlayerGameWeak(playerGame);
                    await _unitOfWork.Save();

                    fk_PlayerGameWeak = playerGame.Id;
                }

                if (fk_PlayerGameWeak > 0)
                {
                    if (isOld)
                    {
                        _unitOfWork.PlayerScore.DeleteOldPlayerScores(fk_PlayerGameWeak);
                    }

                    if (memberResult.Stats != null && memberResult.Stats.Any())
                    {
                        List<Stat> states = memberResult.Stats.Where(a => a.Value != "0").ToList();

                        int minsTypeId = 30;
                        int goalsTypeId = 27;

                        if (!states.Any(a => a.Type == goalsTypeId))
                        {
                            ScoreTypeForCalc eventType = scoreTypes.Where(a => a.Id == (int)ScoreTypeEnum.Goal_Event).SingleOrDefault();
                            List<EventType> result = eventResult.Where(a => eventType._365_TypeId == a.Id.ToString() &&
                                                                            a.SubTypeId.ToString() != "2").ToList(); // 2 Own Goal

                            if (result.Count > 0)
                            {
                                states.Add(new Stat
                                {
                                    Type = goalsTypeId,
                                    Value = result.Count.ToString(),
                                    Name = "goals"
                                });
                            }
                        }

                        int value = states.Where(a => a.Type == minsTypeId)
                                          .Select(a => a.Value)
                                          .FirstOrDefault()
                                          .GetUntilOrEmpty("'")
                                          .ParseToInt();
                        if (value >= 60)
                        {
                            playMinutes = PlayMinutesEnum.PlayMoreThan60Min;
                        }
                        else if (value >= 1)
                        {
                            playMinutes = PlayMinutesEnum.Played;
                        }

                        foreach (Stat Stat in states)
                        {
                            int fk_ScoreType = scoreTypes.Where(a => a._365_TypeId == Stat.Type.ToString()).Select(a => a.Id).SingleOrDefault();
                            if (fk_ScoreType > 0)
                            {
                                if (fk_ScoreType == (int)ScoreTypeEnum.Goals)
                                {
                                    ScoreTypeForCalc eventType = scoreTypes.Where(a => a.Id == (int)ScoreTypeEnum.Goal_Event).SingleOrDefault();
                                    List<EventType> result = eventResult.Where(a => eventType._365_TypeId == a.Id.ToString() && a.SubTypeId.ToString() != "2").ToList();// 2 Own Goal
                                    Stat.Value = result.Count.ToString();
                                }
                                else if (fk_ScoreType == (int)ScoreTypeEnum.Assists)
                                {
                                    Stat.Value = assistCount.ToString();
                                }

                                _gameResultLogic.UpdatePlayerStateScore(otherGoals, substitutions, rankingIndex, playMinutes, fk_ScoreType, Stat.Value, fk_PlayerPosition, fk_PlayerGameWeak);
                                _unitOfWork.Save().Wait();

                            }
                        }
                    }
                    if (eventResult != null && eventResult.Any())
                    {
                        foreach (EventType events in eventResult)
                        {
                            int fk_ScoreType = scoreTypes.Where(a => a._365_TypeId == events.SubTypeId.ToString() && a._365_EventTypeId == events.Id.ToString()).Select(a => a.Id).SingleOrDefault();
                            if (fk_ScoreType > 0)
                            {
                                _gameResultLogic.UpdatePlayerEventScore(otherGoals, substitutions, rankingIndex, playMinutes, events, fk_ScoreType, fk_PlayerPosition, fk_PlayerGameWeak);
                                _unitOfWork.Save().Wait();
                            }
                        }
                    }
                }
            }

            return playMinutes;
        }

        public async Task UpdateGameFinalResult(
            _365CompetitionsEnum _365CompetitionsEnum,
            List<Event> otherGoals,
            List<EventType> substitutions,
            PlayMinutesEnum playMinutes,
            int rankingIndex,
            int fk_TeamGameWeak,
            int fk_Player,
            bool inDebug)
        {
            List<PlayerGameWeakForCalc> players = _unitOfWork.PlayerScore.GetPlayerGameWeaks(new PlayerGameWeakParameters
            {
                Fk_TeamGameWeak = fk_TeamGameWeak,
                Fk_Player = fk_Player
            }, otherLang: false)
                .Select(a => new PlayerGameWeakForCalc
                {
                    Fk_Player = a.Fk_Player,
                    Fk_PlayerPosition = a.Player.Fk_PlayerPosition,
                    Fk_PlayerGameWeak = a.Id,
                    Fk_GameWeak = a.TeamGameWeak.Fk_GameWeak,
                    _365_MatchId = a.TeamGameWeak._365_MatchId
                }).ToList();

            if (players != null && players.Any())
            {
                foreach (PlayerGameWeakForCalc player in players)
                {
                    _gameResultLogic.UpdatePlayerStateScore(otherGoals, substitutions, rankingIndex, playMinutes, (int)ScoreTypeEnum.CleanSheet, "", player.Fk_PlayerPosition, player.Fk_PlayerGameWeak);
                    _gameResultLogic.UpdatePlayerStateScore(otherGoals, substitutions, rankingIndex, playMinutes, (int)ScoreTypeEnum.ReceiveGoals, "", player.Fk_PlayerPosition, player.Fk_PlayerGameWeak);
                    _gameResultLogic.UpdatePlayerStateScore(otherGoals, substitutions, rankingIndex, playMinutes, (int)ScoreTypeEnum.Ranking, "", player.Fk_PlayerPosition, player.Fk_PlayerGameWeak);
                    _gameResultLogic.UpdatePlayerGameWeakTotalPoints(player.Fk_PlayerGameWeak);
                }
                await _unitOfWork.Save();

                int fk_GameWeak = players.First().Fk_GameWeak;
                string _365_MatchId = players.First()._365_MatchId;

                _playerStateCalc.RunPlayersStateCalculations(_365CompetitionsEnum, fk_GameWeak, _365_MatchId, players.Select(a => a.Fk_Player).ToList(), null, false, inDebug);
            }
        }

        public async Task UpdateStatistic(
            List<Statistics> statistics,
            List<StatisticScoreModelForCalc> statisticScores,
            int fk_TeamGameWeak,
            int fk_Team)
        {
            if (statistics != null && statistics.Any())
            {
                statistics.ForEach(a =>
                {
                    int fk_StatisticScore = statisticScores.Where(b => b._365_Id == a.Id.ToString())
                    .Select(a => a.Id).FirstOrDefault();

                    if (fk_StatisticScore > 0)
                    {
                        _unitOfWork.MatchStatistic.CreateMatchStatisticScore(new MatchStatisticScore
                        {
                            Fk_StatisticScore = fk_StatisticScore,
                            Fk_Team = fk_Team,
                            Fk_TeamGameWeak = fk_TeamGameWeak,
                            Value = a.Value,
                            ValuePercentage = a.ValuePercentage
                        });
                    }
                });
                await _unitOfWork.Save();
            }
        }
    }

    public class TeamGameWeakForCalc
    {
        public int Id { get; set; }

        public string _365_MatchId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime => StartTime.AddHours(4);

        public int Fk_Season { get; set; }

        public int Fk_GameWeek { get; set; }

        public bool IsEnded { get; set; }

        public bool HalfTimeEnded { get; set; }
    }

    public class PlayerGameWeakForCalc
    {
        public int Fk_PlayerGameWeak { get; set; }
        public int Fk_PlayerPosition { get; set; }
        public int Fk_Player { get; set; }
        public int Fk_GameWeak { get; set; }
        public string _365_MatchId { get; set; }
    }

    public class ScoreTypeForCalc
    {
        public int Id { get; set; }

        public string _365_TypeId { get; set; }

        public string _365_EventTypeId { get; set; }
    }

    public class PlayerForCalc
    {
        public int Id { get; set; }

        public string _365_PlayerId { get; set; }

        public int Fk_PlayerPosition { get; set; }

        public int Fk_Team { get; set; }
    }
}
