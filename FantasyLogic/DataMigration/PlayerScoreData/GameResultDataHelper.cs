using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using FantasyLogic.Calculations;
using FantasyLogic.SharedLogic;
using IntegrationWith365.Entities.GameModels;
using IntegrationWith365.Entities.GamesModels;
using System.Diagnostics;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.DataMigration.PlayerScoreData
{
    public class GameResultDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;
        private readonly GameResultLogic _gameResultLogic;
        private readonly PlayerStateCalc _playerStateCalc;


        private readonly string RecurringJobMatchId = "UpdateMatch-";
        private readonly string RecurringJobPlayerId = "UpdateMatchPlayer-";

        private readonly string RecurringJobGameWeekPlayerId = "PlayerGameWeekStatesCalc-";
        private readonly string RecurringJobSeasonPlayerId = "PlayerSeasonStatesCalc-";

        public GameResultDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
            _gameResultLogic = new GameResultLogic(unitOfWork);
            _playerStateCalc = new(_unitOfWork);
        }

        public void RunUpdateGameResult(TeamGameWeakParameters parameters, bool inDebug, bool runAll)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            parameters.Fk_Season = season.Id;

            List<TeamGameWeakDto> teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(parameters, otherLang: false).Select(a => new TeamGameWeakDto
            {
                Id = a.Id,
                _365_MatchId = a._365_MatchId,
                StartTime = a.StartTime,
                Fk_GameWeek = a.Fk_GameWeak,
                Fk_Season = a.GameWeak.Fk_Season
            }).ToList();

            List<ScoreTypeDto> scoreTypes = _unitOfWork.PlayerScore
                                                       .GetScoreTypes(new ScoreTypeParameters
                                                       {
                                                           HavePoints = true,
                                                       }, otherLang: false)
                                                       .Select(a => new ScoreTypeDto
                                                       {
                                                           Id = a.Id,
                                                           _365_EventTypeId = a._365_EventTypeId,
                                                           _365_TypeId = a._365_TypeId,
                                                       })
                                                       .ToList();

            foreach (TeamGameWeakDto teamGameWeak in teamGameWeaks)
            {
                if (inDebug)
                {
                    UpdateGameResult(teamGameWeak, scoreTypes, inDebug, runAll).Wait();
                }
                else
                {
                    if (runAll || teamGameWeak.EndTime > DateTime.UtcNow.ToEgypt())
                    {
                        RecurringJob.AddOrUpdate(RecurringJobMatchId + teamGameWeak._365_MatchId.ToString(), () => UpdateGameResult(teamGameWeak, scoreTypes, inDebug, runAll), CronExpression.EveryMinutes(15), TimeZoneInfo.Utc);
                    }
                }
            }
        }

        public async Task UpdateGameResult(TeamGameWeakDto teamGameWeak, List<ScoreTypeDto> scoreTypes, bool inDebug, bool runAll)
        {
            TeamGameWeak match = await _unitOfWork.Season.FindTeamGameWeakbyId(teamGameWeak.Id, trackChanges: true);

            GameReturn gameReturn = await _365Services.GetGame(new _365GameParameters
            {
                GameId = teamGameWeak._365_MatchId.ParseToInt()
            });

            bool matchEnded = inDebug || runAll ? false : teamGameWeak.EndTime < DateTime.UtcNow.ToEgypt();

            if (matchEnded)
            {
                RecurringJob.RemoveIfExists(RecurringJobMatchId + teamGameWeak._365_MatchId.ToString());

            }
            if (matchEnded || runAll)
            {
                _ = BackgroundJob.Enqueue(() => _playerStateCalc.UpdateTop15(teamGameWeak.Fk_GameWeek, teamGameWeak.Fk_Season));
            }

            if (!inDebug && !runAll)
            {
                if (match.LastUpdateId == gameReturn.LastUpdateId)
                {
                    return;
                }
            }

            if (gameReturn != null && gameReturn.Game != null &&
                gameReturn.Game.AwayCompetitor != null &&
                gameReturn.Game.HomeCompetitor != null)
            {
                match.LastUpdateId = gameReturn.LastUpdateId;
                match.IsEnded = gameReturn.Game.IsEnded;
                match.StartTime = gameReturn.Game.StartTime;
                match.AwayScore = (int)gameReturn.Game.AwayCompetitor.Score;
                match.HomeScore = (int)gameReturn.Game.HomeCompetitor.Score;
                await _unitOfWork.Save();

                if (gameReturn.Game.Members != null && gameReturn.Game.Members.Any())
                {
                    List<GameMember> allMembers = gameReturn.Game.Members;

                    if (matchEnded)
                    {
                        foreach (int id in allMembers.Select(a => a.Id).ToList())
                        {
                            RecurringJob.RemoveIfExists(RecurringJobPlayerId + teamGameWeak._365_MatchId.ToString() + $"-{id}");
                        }
                    }

                    IQueryable<PlayerModel> playersQuary = _unitOfWork.Team.GetPlayers(new PlayerParameters
                    {
                        _365_PlayerIds = allMembers.Select(a => a.AthleteId.ToString()).ToList(),
                    }, otherLang: false);

                    if (playersQuary.Any())
                    {
                        List<PlayerDto> players = playersQuary.Select(a => new PlayerDto
                        {
                            Id = a.Id,
                            _365_PlayerId = a._365_PlayerId,
                            Fk_PlayerPosition = a.Fk_PlayerPosition,
                            Fk_Team = a.Fk_Team
                        }).ToList();

                        if (matchEnded)
                        {
                            foreach (int player in players.Select(a => a.Id).ToList())
                            {
                                string recurringGameWeekId = RecurringJobGameWeekPlayerId + $"{teamGameWeak.Fk_GameWeek}-{player}";
                                string recurringSeasonId = RecurringJobSeasonPlayerId + $"{teamGameWeak.Fk_Season}-{player}";

                                RecurringJob.RemoveIfExists(recurringGameWeekId);
                                RecurringJob.RemoveIfExists(recurringSeasonId);
                            }
                        }

                        if (!matchEnded)
                        {
                            List<Member> allMembersResults = new();
                            allMembersResults.AddRange(gameReturn.Game.HomeCompetitor.Lineups.Members);
                            allMembersResults.AddRange(gameReturn.Game.AwayCompetitor.Lineups.Members);

                            List<int> membersRanking = allMembersResults.OrderByDescending(a => a.Ranking)
                                                                        .Skip(0)
                                                                        .Take(3)
                                                                        .Select(a => a.Id)
                                                                        .ToList();

                            foreach (GameMember member in allMembers)
                            {
                                PlayerDto player = players.Where(a => a._365_PlayerId == member.AthleteId.ToString())
                                                       .Select(a => new PlayerDto
                                                       {
                                                           Id = a.Id,
                                                           Fk_PlayerPosition = a.Fk_PlayerPosition,
                                                           Fk_Team = a.Fk_Team
                                                       })
                                                       .SingleOrDefault();
                                if (player != null)
                                {
                                    Member memberResult = allMembersResults.Where(a => a.Id == member.Id).SingleOrDefault();

                                    int rankingIndex = 0;

                                    if (membersRanking.Contains(member.Id))
                                    {
                                        rankingIndex = membersRanking.IndexOf(member.Id) + 1;
                                    }

                                    if (memberResult != null)
                                    {
                                        if (inDebug)
                                        {
                                            UpdatePlayerResult(gameReturn, member, player, rankingIndex, teamGameWeak, memberResult, scoreTypes, match, inDebug).Wait();
                                        }
                                        else
                                        {
                                            string recurringJobPlayerId = RecurringJobPlayerId + teamGameWeak._365_MatchId.ToString() + $"-{member.Id}";

                                            RecurringJob.AddOrUpdate(recurringJobPlayerId, () => UpdatePlayerResult(gameReturn, member, player, rankingIndex, teamGameWeak, memberResult, scoreTypes, match, inDebug), CronExpression.EveryDayOfMonth(1, 8, 0), TimeZoneInfo.Utc);
                                            RecurringJobCustom.TriggerJob(recurringJobPlayerId);
                                        }
                                    }
                                }
                            }


                        }
                    }
                }
            }
        }

        public async Task UpdatePlayerResult(
            GameReturn gameReturn,
            GameMember member,
            PlayerDto player,
            int rankingIndex,
            TeamGameWeakDto teamGameWeak,
            Member memberResult,
            List<ScoreTypeDto> scoreTypes,
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

                bool canGetCleanSheat = await UpdatePlayerGameResult(otherGoals, substitutions, rankingIndex, player.Id, player.Fk_Team, player.Fk_PlayerPosition, teamGameWeak.Id, memberResult, eventResult, scoreTypes, assistCount);
                await _unitOfWork.Save();

                await UpdateGameFinalResult(otherGoals, substitutions, canGetCleanSheat, rankingIndex, match.Id, player.Id, inDebug);
            }
        }

        public async Task<bool> UpdatePlayerGameResult(
            List<Event> otherGoals,
            List<EventType> substitutions,
            int rankingIndex,
            int fk_Player,
            int fk_Team,
            int fk_PlayerPosition,
            int fk_TeamGameWeak,
            Member memberResult,
            List<EventType> eventResult,
            List<ScoreTypeDto> scoreTypes,
            int assistCount)
        {
            bool canGetCleanSheat = false;

            if (fk_Player > 0 && memberResult != null)
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

                int fk_PlayerGameWeak = _unitOfWork.PlayerScore
                                                   .GetPlayerGameWeaks(new PlayerGameWeakParameters { Fk_TeamGameWeak = fk_TeamGameWeak, Fk_Player = fk_Player }, false)
                                                   .Select(x => x.Id)
                                                   .SingleOrDefault();

                if (fk_PlayerGameWeak > 0)
                {
                    _unitOfWork.PlayerScore.DeleteOldPlayerScores(fk_PlayerGameWeak);
                    _unitOfWork.Save().Wait();


                    if (memberResult.Stats != null && memberResult.Stats.Any())
                    {
                        List<Stat> states = memberResult.Stats.Where(a => a.Value != "0").ToList();
                        foreach (Stat Stat in states)
                        {
                            int fk_ScoreType = scoreTypes.Where(a => a._365_TypeId == Stat.Type.ToString()).Select(a => a.Id).SingleOrDefault();
                            if (fk_ScoreType > 0)
                            {
                                if (fk_ScoreType == (int)ScoreTypeEnum.Goals)
                                {
                                    ScoreTypeDto eventType = scoreTypes.Where(a => a.Id == (int)ScoreTypeEnum.Goal_Event).SingleOrDefault();
                                    List<EventType> result = eventResult.Where(a => eventType._365_TypeId == a.SubTypeId.ToString() && eventType._365_EventTypeId == a.Id.ToString()).ToList();
                                    Stat.Value = result.Count.ToString();
                                }
                                else if (fk_ScoreType == (int)ScoreTypeEnum.Assists)
                                {
                                    Stat.Value = assistCount.ToString();
                                }
                                else if (fk_ScoreType == (int)ScoreTypeEnum.Minutes)
                                {
                                    int value = Stat.Value.GetUntilOrEmpty("'").ParseToInt();
                                    if (value >= 60)
                                    {
                                        canGetCleanSheat = true;
                                    }
                                }

                                _gameResultLogic.UpdatePlayerStateScore(otherGoals, substitutions, rankingIndex, canGetCleanSheat, fk_ScoreType, Stat.Value, fk_PlayerPosition, fk_PlayerGameWeak);
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
                                _gameResultLogic.UpdatePlayerEventScore(otherGoals, substitutions, rankingIndex, canGetCleanSheat, events, fk_ScoreType, fk_PlayerPosition, fk_PlayerGameWeak);
                            }
                        }
                    }
                }
            }

            return canGetCleanSheat;
        }

        public async Task UpdateGameFinalResult(
            List<Event> otherGoals,
            List<EventType> substitutions,
            bool canGetCleanSheat,
            int rankingIndex,
            int fk_TeamGameWeak,
            int fk_Player,
            bool inDebug)
        {
            List<PlayerGameWeakDto> players = _unitOfWork.PlayerScore.GetPlayerGameWeaks(new PlayerGameWeakParameters
            {
                Fk_TeamGameWeak = fk_TeamGameWeak,
                Fk_Player = fk_Player
            }, otherLang: false)
                .Select(a => new PlayerGameWeakDto
                {
                    Fk_Player = a.Fk_Player,
                    Fk_PlayerPosition = a.Player.Fk_PlayerPosition,
                    Fk_PlayerGameWeak = a.Id,
                    Fk_GameWeak = a.TeamGameWeak.Fk_GameWeak,
                    _365_MatchId = a.TeamGameWeak._365_MatchId
                }).ToList();

            if (players != null && players.Any())
            {
                foreach (PlayerGameWeakDto player in players)
                {
                    _gameResultLogic.UpdatePlayerStateScore(otherGoals, substitutions, rankingIndex, canGetCleanSheat, (int)ScoreTypeEnum.CleanSheet, "", player.Fk_PlayerPosition, player.Fk_PlayerGameWeak);
                    _gameResultLogic.UpdatePlayerStateScore(otherGoals, substitutions, rankingIndex, canGetCleanSheat, (int)ScoreTypeEnum.ReceiveGoals, "", player.Fk_PlayerPosition, player.Fk_PlayerGameWeak);
                    _gameResultLogic.UpdatePlayerStateScore(otherGoals, substitutions, rankingIndex, canGetCleanSheat, (int)ScoreTypeEnum.Ranking, "", player.Fk_PlayerPosition, player.Fk_PlayerGameWeak);
                    await _gameResultLogic.UpdatePlayerGameWeakTotalPoints(player.Fk_PlayerGameWeak);
                }
                await _unitOfWork.Save();

                int fk_GameWeak = players.First().Fk_GameWeak;
                string _365_MatchId = players.First()._365_MatchId;

                _playerStateCalc.RunPlayersStateCalculations(fk_GameWeak, _365_MatchId, players.Select(a => a.Fk_Player).ToList(), inDebug);
            }
        }
    }

    public class TeamGameWeakDto
    {
        public int Id { get; set; }

        public string _365_MatchId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime => StartTime.AddHours(3);

        public int Fk_Season { get; set; }

        public int Fk_GameWeek { get; set; }
    }

    public class PlayerGameWeakDto
    {
        public int Fk_PlayerGameWeak { get; set; }
        public int Fk_PlayerPosition { get; set; }
        public int Fk_Player { get; set; }
        public int Fk_GameWeak { get; set; }
        public string _365_MatchId { get; set; }
    }

    public class ScoreTypeDto
    {
        public int Id { get; set; }

        public string _365_TypeId { get; set; }

        public string _365_EventTypeId { get; set; }
    }

    public class PlayerDto
    {
        public int Id { get; set; }

        public string _365_PlayerId { get; set; }

        public int Fk_PlayerPosition { get; set; }

        public int Fk_Team { get; set; }
    }
}
