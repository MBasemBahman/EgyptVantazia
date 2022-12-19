﻿using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using FantasyLogic.Calculations;
using FantasyLogic.SharedLogic;
using IntegrationWith365.Entities.GameModels;
using IntegrationWith365.Entities.GamesModels;
using static Contracts.EnumData.DBModelsEnum;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace FantasyLogic.DataMigration.PlayerScoreData
{
    public class GameResultDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;
        private readonly GameResultLogic _gameResultLogic;
        private readonly PlayerStateCalc _playerStateCalc;

        public GameResultDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
            _gameResultLogic = new GameResultLogic(unitOfWork);
            _playerStateCalc = new(_unitOfWork);
        }

        public void RunUpdateGameResult(TeamGameWeakParameters parameters, bool ignore365Points = false)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            parameters.Fk_Season = season.Id;

            List<TeamGameWeakDto> teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(parameters, otherLang: false).Select(a => new TeamGameWeakDto
            {
                Id = a.Id,
                _365_MatchId = a._365_MatchId,
                Fk_Away = a.Fk_Away,
                Fk_Home = a.Fk_Home
            }).ToList();

            List<ScoreTypeDto> scoreTypes = _unitOfWork.PlayerScore
                                                       .GetScoreTypes(new ScoreTypeParameters
                                                       {
                                                           HavePoints = true,
                                                       }, otherLang: false)
                                                       .Select(a => new ScoreTypeDto
                                                       {
                                                           Id = a.Id,
                                                           IsEvent = a.IsEvent,
                                                           _365_EventTypeId = a._365_EventTypeId,
                                                           _365_TypeId = a._365_TypeId
                                                       })
                                                       .ToList();
            string jobId = null;
            foreach (TeamGameWeakDto teamGameWeak in teamGameWeaks)
            {
                jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGameResult(teamGameWeak, scoreTypes, ignore365Points))
                    : BackgroundJob.Enqueue(() => UpdateGameResult(teamGameWeak, scoreTypes, ignore365Points));
            }
        }

        public async Task UpdateGameResult(TeamGameWeakDto teamGameWeak, List<ScoreTypeDto> scoreTypes, bool ignore365Points)
        {
            GameReturn gameReturn = await _365Services.GetGame(new _365GameParameters
            {
                GameId = teamGameWeak._365_MatchId.ParseToInt()
            });

            if (gameReturn != null && gameReturn.Game != null &&
                gameReturn.Game.AwayCompetitor != null &&
                gameReturn.Game.HomeCompetitor != null)
            {
                TeamGameWeak match = await _unitOfWork.Season.FindTeamGameWeakbyId(teamGameWeak.Id, trackChanges: true);
                match.IsEnded = gameReturn.Game.IsEnded;
                match.AwayScore = (int)gameReturn.Game.AwayCompetitor.Score;
                match.HomeScore = (int)gameReturn.Game.HomeCompetitor.Score;
                await _unitOfWork.Save();

                if (gameReturn.Game.Members != null && gameReturn.Game.Members.Any())
                {
                    string jobId = null;

                    if (!ignore365Points)
                    {
                        List<GameMember> allMembers = gameReturn.Game.Members;

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

                            List<Member> allMembersResults = new();
                            allMembersResults.AddRange(gameReturn.Game.HomeCompetitor.Lineups.Members);
                            allMembersResults.AddRange(gameReturn.Game.AwayCompetitor.Lineups.Members);

                            int substitutionId = 1000;
                            int goalId = 1;

                            List<EventType> substitutionPlayers = new();

                            foreach (GameMember member in allMembers)
                            {

                                var player = players.Where(a => a._365_PlayerId == member.AthleteId.ToString())
                                                       .Select(a => new
                                                       {
                                                           a.Id,
                                                           a.Fk_PlayerPosition,
                                                           a.Fk_Team
                                                       })
                                                       .SingleOrDefault();
                                if (player != null)
                                {
                                    Member memberResult = allMembersResults.Where(a => a.Id == member.Id).SingleOrDefault();
                                    if (memberResult != null)
                                    {
                                        List<EventType> eventResult = new();
                                        int assistCount = 0;

                                        if (gameReturn.Game != null && gameReturn.Game.Events != null)
                                        {
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
                                                                    IsAssist = (a.EventType.Id == goalId && a.ExtraPlayers != null && a.ExtraPlayers.Any()) ? true : null
                                                                })
                                                                .ToList();

                                            assistCount = gameReturn.Game
                                                                .Events
                                                                .Where(a => a.EventType.Id == goalId && a.ExtraPlayers != null && a.ExtraPlayers.Any() && a.ExtraPlayers.First() == member.Id && a.GameTime > 0)
                                                                .Count();

                                            if (eventResult.Any(a => a.IsOut == true))
                                            {
                                                EventType eventResultItem = eventResult.First(a => a.IsOut == true);

                                                substitutionPlayers.Add(new EventType
                                                {
                                                    Id = eventResultItem.Id,
                                                    Name = eventResultItem.Name,
                                                    SubTypeId = eventResultItem.SubTypeId,
                                                    SubTypeName = eventResultItem.SubTypeName,
                                                    GameTime = eventResultItem.GameTime,
                                                    Value = eventResultItem.Value,
                                                    IsOut = true,
                                                    _365_PlayerId = eventResultItem.ExtraPlayer,
                                                });
                                            }
                                        }

                                        jobId = await UpdatePlayerGameResult(player.Id, player.Fk_Team, player.Fk_PlayerPosition, teamGameWeak.Id, memberResult, eventResult, scoreTypes, assistCount, jobId);
                                    }
                                }
                            }

                            if (substitutionPlayers.Any())
                            {
                                List<int> substitutionPlayersIds = substitutionPlayers.Select(a => a._365_PlayerId).ToList();
                                List<GameMember> substitutionMembers = allMembers.Where(a => substitutionPlayersIds.Contains(a.Id)).ToList();
                                foreach (GameMember member in substitutionMembers)
                                {
                                    var player = players.Where(a => a._365_PlayerId == member.AthleteId.ToString())
                                                       .Select(a => new
                                                       {
                                                           a.Id,
                                                           a.Fk_PlayerPosition,
                                                           a.Fk_Team
                                                       })
                                                       .SingleOrDefault();

                                    if (player != null)
                                    {
                                        Member memberResult = allMembersResults.Where(a => a.Id == member.Id).SingleOrDefault();
                                        if (memberResult != null)
                                        {
                                            List<EventType> eventResult = substitutionPlayers.Where(a => a._365_PlayerId == member.Id).ToList();

                                            jobId = await UpdatePlayerGameResult(player.Id, player.Fk_Team, player.Fk_PlayerPosition, teamGameWeak.Id, memberResult, eventResult, scoreTypes, 0, jobId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    jobId = UpdateGameFinalResult(match.Id, jobId);
                }
            }
        }

        public string UpdateGameFinalResult(int fk_TeamGameWeak, string jobId)
        {
            List<PlayerGameWeakDto> players = _unitOfWork.PlayerScore.GetPlayerGameWeaks(new PlayerGameWeakParameters
            {
                Fk_TeamGameWeak = fk_TeamGameWeak
            }, otherLang: false)
                .Select(a => new PlayerGameWeakDto
                {
                    Fk_Player = a.Fk_Player,
                    Fk_PlayerPosition = a.Player.Fk_PlayerPosition,
                    Fk_PlayerGameWeak = a.Id,
                    Fk_Team = a.Player.Fk_Team,
                    Fk_GameWeak = a.TeamGameWeak.Fk_GameWeak,
                    _365_MatchId = a.TeamGameWeak._365_MatchId
                }).ToList();

            if (players != null && players.Any())
            {
                foreach (PlayerGameWeakDto player in players)
                {
                    jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => _gameResultLogic.UpdatePlayerStateScore((int)ScoreTypeEnum.CleanSheet, "", player.Fk_Player, player.Fk_Team, player.Fk_PlayerPosition, player.Fk_PlayerGameWeak, fk_TeamGameWeak))
                    : BackgroundJob.Enqueue(() => _gameResultLogic.UpdatePlayerStateScore((int)ScoreTypeEnum.CleanSheet, "", player.Fk_Player, player.Fk_Team, player.Fk_PlayerPosition, player.Fk_PlayerGameWeak, fk_TeamGameWeak));

                    //jobId = BackgroundJob.ContinueJobWith(jobId, () => _gameResultLogic.UpdatePlayerStateScore((int)ScoreTypeEnum.CleanSheet, "", player.Fk_Player, player.Fk_Team, player.Fk_PlayerPosition, player.Fk_PlayerGameWeak, fk_TeamGameWeak));

                    jobId = BackgroundJob.ContinueJobWith(jobId, () => _gameResultLogic.UpdatePlayerStateScore((int)ScoreTypeEnum.ReceiveGoals, "", player.Fk_Player, player.Fk_Team, player.Fk_PlayerPosition, player.Fk_PlayerGameWeak, fk_TeamGameWeak));
                    jobId = BackgroundJob.ContinueJobWith(jobId, () => _gameResultLogic.UpdatePlayerStateScore((int)ScoreTypeEnum.Ranking, "", player.Fk_Player, player.Fk_Team, player.Fk_PlayerPosition, player.Fk_PlayerGameWeak, fk_TeamGameWeak));

                    jobId = BackgroundJob.ContinueJobWith(jobId, () => _gameResultLogic.UpdatePlayerGameWeakTotalPoints(player.Fk_PlayerGameWeak));
                }

                int fk_GameWeak = players.First().Fk_GameWeak;
                string _365_MatchId = players.First()._365_MatchId;

                _playerStateCalc.RunPlayersStateCalculations(fk_GameWeak, _365_MatchId);
            }

            return jobId;
        }

        public async Task<string> UpdatePlayerGameResult(
            int fk_Player,
            int fk_Team,
            int fk_PlayerPosition,
            int fk_TeamGameWeak,
            Member memberResult,
            List<EventType> eventResult,
            List<ScoreTypeDto> scoreTypes,
            int assistCount,
            string jobId)
        {
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

                //PlayerGameWeakScore

                if (fk_PlayerGameWeak > 0)
                {
                    _unitOfWork.PlayerScore.DeleteOldPlayerScores(fk_PlayerGameWeak);
                    _unitOfWork.Save().Wait();

                    if (memberResult.Stats != null && memberResult.Stats.Any())
                    {
                        foreach (Stat Stat in memberResult.Stats)
                        {
                            int fk_ScoreType = scoreTypes.Where(a => a._365_TypeId == Stat.Type.ToString()).Select(a => a.Id).SingleOrDefault();
                            if (fk_ScoreType > 0)
                            {
                                if (fk_ScoreType == (int)ScoreTypeEnum.Goals)
                                {
                                    var eventType = scoreTypes.Where(a => a.Id == (int)ScoreTypeEnum.Goal_Event).SingleOrDefault();
                                    var result = eventResult.Where(a => eventType._365_TypeId == a.SubTypeId.ToString() && eventType._365_EventTypeId == a.Id.ToString()).ToList();
                                    Stat.Value = result.Count.ToString();
                                }else if (fk_ScoreType == (int)ScoreTypeEnum.Assists)
                                {
                                    Stat.Value = assistCount.ToString();
                                }

                                jobId = jobId.IsExisting()
                                    ? BackgroundJob.ContinueJobWith(jobId, () => _gameResultLogic.UpdatePlayerStateScore(fk_ScoreType, Stat.Value, fk_Player, fk_Team, fk_PlayerPosition, fk_PlayerGameWeak, fk_TeamGameWeak))
                                    : BackgroundJob.Enqueue(() => _gameResultLogic.UpdatePlayerStateScore(fk_ScoreType, Stat.Value, fk_Player, fk_Team, fk_PlayerPosition, fk_PlayerGameWeak, fk_TeamGameWeak));
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
                                jobId = jobId.IsExisting()
                                    ? BackgroundJob.ContinueJobWith(jobId, () => _gameResultLogic.UpdatePlayerEventScore(events, fk_ScoreType, fk_Player, fk_Team, fk_PlayerPosition, fk_PlayerGameWeak, fk_TeamGameWeak))
                                    : BackgroundJob.Enqueue(() => _gameResultLogic.UpdatePlayerEventScore(events, fk_ScoreType, fk_Player, fk_Team, fk_PlayerPosition, fk_PlayerGameWeak, fk_TeamGameWeak));
                            }
                        }
                    }
                }
            }

            return jobId;
        }
    }

    public class TeamGameWeakDto
    {
        public int Id { get; set; }

        public string _365_MatchId { get; set; }

        public int Fk_Away { get; set; }

        public int Fk_Home { get; set; }
    }

    public class PlayerGameWeakDto
    {
        public int Fk_PlayerGameWeak { get; set; }
        public int Fk_PlayerPosition { get; set; }
        public int Fk_Player { get; set; }
        public int Fk_Team { get; set; }

        public int Fk_GameWeak { get; set; }

        public string _365_MatchId { get; set; }
    }

    public class ScoreTypeDto
    {
        public int Id { get; set; }

        public string _365_TypeId { get; set; }

        public string _365_EventTypeId { get; set; }

        public bool IsEvent { get; set; }
    }

    public class PlayerDto
    {
        public int Id { get; set; }

        public string _365_PlayerId { get; set; }

        public int Fk_PlayerPosition { get; set; }

        public int Fk_Team { get; set; }
    }
}
