﻿using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using FantasyLogic.Calculations;
using IntegrationWith365.Entities.GameModels;
using IntegrationWith365.Entities.GamesModels;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.DataMigration.PlayerScoreData
{
    public class GameResultDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;
        private readonly PlayerScoreCalc _playerScoreCalc;

        public GameResultDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
            _playerScoreCalc = new PlayerScoreCalc(unitOfWork);
        }

        public void RunUpdateGameResult(TeamGameWeakParameters parameters, int delayMinutes)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            parameters.Fk_Season = season.Id;
            parameters.IsEnded = true;

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
            foreach (TeamGameWeakDto teamGameWeak in teamGameWeaks)
            {
                _ = BackgroundJob.Schedule(() => UpdateGameResult(teamGameWeak, scoreTypes, delayMinutes), TimeSpan.FromMinutes(delayMinutes));
            }
        }

        public async Task UpdateGameResult(TeamGameWeakDto teamGameWeak, List<ScoreTypeDto> scoreTypes, int delayMinutes)
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

                        foreach (GameMember member in allMembers)
                        {
                            var player = players.Where(a => a._365_PlayerId == member.AthleteId.ToString())
                                                   .Select(a => new
                                                   {
                                                       a.Id,
                                                       a.Fk_PlayerPosition,
                                                       a.Fk_Team
                                                   })
                                                   .FirstOrDefault();
                            if (player != null)
                            {
                                Member memberResult = allMembersResults.Where(a => a.Id == member.Id).FirstOrDefault();

                                List<EventType> eventResult = gameReturn.Game
                                                                        .Events
                                                                        .Where(a => a.PlayerId == member.Id)
                                                                        .GroupBy(a => a.EventType)
                                                                        .Select(a => new EventType
                                                                        {
                                                                            Id = a.Key.Id,
                                                                            Name = a.Key.Name,
                                                                            SubTypeId = a.Key.SubTypeId,
                                                                            SubTypeName = a.Key.SubTypeName,
                                                                            GameTime = a.First().GameTime,
                                                                            Value = a.Count()
                                                                        })
                                                                        .ToList();

                                _ = BackgroundJob.Schedule(() => UpdatePlayerGameResult(player.Id, player.Fk_PlayerPosition, player.Fk_Team, teamGameWeak.Id, memberResult, eventResult, scoreTypes, delayMinutes), TimeSpan.FromMinutes(delayMinutes));
                            }
                        }
                    }

                    if (match.IsEnded)
                    {
                        UpdateGameFinalResult(match.Id, allMembers.Count);
                    }
                }
            }
        }

        public void UpdateGameFinalResult(int fk_TeamGameWeak, int delayMinutes)
        {
            List<PlayerGameWeakScoreModel> goals = _unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
            {
                Fk_TeamGameWeak = fk_TeamGameWeak,
                Fk_ScoreTypes = new List<int>
                {
                    (int)ScoreTypeEnum.Goal,
                    (int)ScoreTypeEnum.PenaltyKick
                }
            }, otherLang: false).OrderBy(a => a.GameTime).ToList();

            List<PlayerGameWeakDto> players = _unitOfWork.PlayerScore.GetPlayerGameWeaks(new PlayerGameWeakParameters
            {
                Fk_TeamGameWeak = fk_TeamGameWeak
            }, otherLang: false)
                .Select(a => new PlayerGameWeakDto
                {
                    Fk_Player = a.Fk_Player,
                    Fk_PlayerPosition = a.Player.Fk_PlayerPosition,
                    Fk_PlayerGameWeak = a.Id,
                    Fk_Team = a.Player.Fk_Team
                }).ToList();

            foreach (PlayerGameWeakDto player in players)
            {
                _ = BackgroundJob.Schedule(() => UpdatePlayerStateScore((int)ScoreTypeEnum.CleanSheet, "", player.Fk_Player, player.Fk_Team, player.Fk_PlayerPosition, player.Fk_PlayerGameWeak, fk_TeamGameWeak, goals), TimeSpan.FromMinutes(delayMinutes));
                _ = BackgroundJob.Schedule(() => UpdatePlayerStateScore((int)ScoreTypeEnum.ReceiveGoals, "", player.Fk_Player, player.Fk_Team, player.Fk_PlayerPosition, player.Fk_PlayerGameWeak, fk_TeamGameWeak, goals), TimeSpan.FromMinutes(delayMinutes));
                _ = BackgroundJob.Schedule(() => UpdatePlayerStateScore((int)ScoreTypeEnum.Ranking, "", player.Fk_Player, player.Fk_Team, player.Fk_PlayerPosition, player.Fk_PlayerGameWeak, fk_TeamGameWeak, goals), TimeSpan.FromMinutes(delayMinutes));
            }
        }

        public async Task UpdatePlayerGameResult(
            int fk_Player,
            int fk_Team,
            int fk_PlayerPosition,
            int fk_TeamGameWeak,
            Member memberResult,
            List<EventType> eventResult,
            List<ScoreTypeDto> scoreTypes,
            int delayMinutes)
        {
            if (fk_Player > 0 && memberResult != null)
            {
                PlayerGameWeak playerGame = new()
                {
                    Fk_TeamGameWeak = fk_TeamGameWeak,
                    Fk_Player = fk_Player,
                    Ranking = memberResult.Ranking
                };
                _unitOfWork.PlayerScore.CreatePlayerGameWeak(playerGame);
                await _unitOfWork.Save();

                int fk_PlayerGameWeak = _unitOfWork.PlayerScore
                                                   .GetPlayerGameWeaks(new PlayerGameWeakParameters { Fk_TeamGameWeak = fk_TeamGameWeak, Fk_Player = fk_Player }, false)
                                                   .Select(x => x.Id)
                                                   .FirstOrDefault();
                if (fk_PlayerGameWeak > 0)
                {
                    if (memberResult.Stats != null && memberResult.Stats.Any())
                    {
                        foreach (Stat Stat in memberResult.Stats)
                        {
                            int fk_ScoreType = scoreTypes.Where(a => a._365_TypeId == Stat.Type.ToString()).Select(a => a.Id).FirstOrDefault();
                            if (fk_ScoreType > 0)
                            {
                                _ = BackgroundJob.Schedule(() => UpdatePlayerStateScore(fk_ScoreType, Stat.Value, fk_Player, fk_Team, fk_PlayerPosition, fk_PlayerGameWeak, fk_TeamGameWeak, null), TimeSpan.FromMinutes(delayMinutes));
                            }
                        }
                    }
                    if (eventResult != null && eventResult.Any())
                    {
                        foreach (EventType events in eventResult)
                        {
                            int fk_ScoreType = scoreTypes.Where(a => a.IsEvent = true && a._365_EventTypeId == events.Id.ToString() && a._365_TypeId == events.SubTypeId.ToString()).Select(a => a.Id).FirstOrDefault();
                            if (fk_ScoreType > 0)
                            {
                                _ = BackgroundJob.Schedule(() => UpdatePlayerEventScore(events, fk_ScoreType, fk_Player, fk_Team, fk_PlayerPosition, fk_PlayerGameWeak, fk_TeamGameWeak), TimeSpan.FromMinutes(delayMinutes));
                            }
                        }
                    }
                }
            }
        }

        public async Task UpdatePlayerStateScore(int fk_ScoreType, string value, int fk_Player, int fk_Team, int fk_PlayerPosition, int fk_PlayerGameWeak, int fk_TeamGameWeak, List<PlayerGameWeakScoreModel> goals)
        {
            PlayerGameWeakScore score = new()
            {
                Fk_PlayerGameWeak = fk_PlayerGameWeak,
                Fk_ScoreType = fk_ScoreType,
                Value = value,
            };

            _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(score, fk_Player, fk_Team, fk_PlayerGameWeak, fk_PlayerPosition, fk_TeamGameWeak, goals));
            await _unitOfWork.Save();
        }

        public async Task UpdatePlayerEventScore(EventType events, int fk_ScoreType, int fk_Player, int fk_Team, int fk_PlayerPosition, int fk_PlayerGameWeak, int fk_TeamGameWeak)
        {
            PlayerGameWeakScore score = new()
            {
                Fk_PlayerGameWeak = fk_PlayerGameWeak,
                Fk_ScoreType = fk_ScoreType,
                Value = events.Value.ToString(),
                GameTime = events.GameTime,
            };
            _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(score, fk_Player, fk_Team, fk_PlayerGameWeak, fk_PlayerPosition, fk_TeamGameWeak, null));
            await _unitOfWork.Save();
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
