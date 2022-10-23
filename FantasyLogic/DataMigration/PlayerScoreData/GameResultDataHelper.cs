using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using IntegrationWith365.Entities.GameModels;
using IntegrationWith365.Entities.GamesModels;

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

        public void UpdateGameResult()
        {
            List<TeamGameWeakDto> teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
            }, otherLang: false).Select(a => new TeamGameWeakDto
            {
                Id = a.Id,
                _365_MatchId = a._365_MatchId
            }).ToList();

            List<ScoreTypeDto> scoreTypes = _unitOfWork.PlayerScore
                                                       .GetScoreTypes(new ScoreTypeParameters(), otherLang: false)
                                                       .Select(a => new ScoreTypeDto
                                                       {
                                                           Id = a.Id,
                                                           IsEvent = a.IsEvent,
                                                           _365_EventTypeId = a._365_EventTypeId,
                                                           _365_TypeId = a._365_TypeId
                                                       })
                                                       .ToList();
            int minutes = 30;
            foreach (TeamGameWeakDto teamGameWeak in teamGameWeaks)
            {
                _ = BackgroundJob.Schedule(() => UpdateGameResult(teamGameWeak, scoreTypes), TimeSpan.FromMinutes(minutes));
                minutes++;
            }
        }

        public async Task UpdateGameResult(TeamGameWeakDto teamGameWeak, List<ScoreTypeDto> scoreTypes)
        {
            GameReturn gameReturn = await _365Services.GetGame(new _365GameParameters
            {
                GameId = teamGameWeak._365_MatchId.ParseToInt()
            });

            if (gameReturn.Game != null)
            {
                TeamGameWeak match = await _unitOfWork.Season.FindTeamGameWeakbyId(teamGameWeak.Id, trackChanges: true);
                match.IsEnded = gameReturn.Game.IsEnded;
                match.AwayScore = (int)gameReturn.Game.AwayCompetitor.Score;
                match.HomeScore = (int)gameReturn.Game.HomeCompetitor.Score;
                await _unitOfWork.Save();

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

                    int minutes = 30;
                    foreach (GameMember member in allMembers)
                    {
                        var player = players.Where(a => a._365_PlayerId == member.AthleteId.ToString())
                                               .Select(a => new
                                               {
                                                   a.Id,
                                                   a.Fk_PlayerPosition
                                               })
                                               .FirstOrDefault();

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

                        _ = BackgroundJob.Schedule(() => UpdateGameResult(player.Id, player.Fk_PlayerPosition, teamGameWeak.Id, memberResult, eventResult, scoreTypes), TimeSpan.FromMinutes(minutes));

                        minutes++;
                    }
                }
            }
        }

        public async Task UpdateGameResult(
            int fk_Player,
            int fk_PlayerPosition,
            int fk_TeamGameWeak,
            Member memberResult,
            List<EventType> eventResult,
            List<ScoreTypeDto> scoreTypes)
        {
            if (fk_Player > 0 && memberResult != null)
            {
                PlayerGameWeak playerGame = new()
                {
                    Fk_TeamGameWeak = fk_TeamGameWeak,
                    Fk_Player = fk_Player,
                    Ranking = memberResult.Ranking,
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
                        int minutes = 30;
                        foreach (Stat Stat in memberResult.Stats)
                        {
                            _ = BackgroundJob.Schedule(() => UpdatePlayerState(Stat, fk_Player, fk_PlayerPosition, fk_PlayerGameWeak, scoreTypes), TimeSpan.FromMinutes(minutes));
                            minutes++;
                        }
                    }
                    if (eventResult != null && eventResult.Any())
                    {
                        int minutes = 30;
                        foreach (EventType events in eventResult)
                        {
                            _ = BackgroundJob.Schedule(() => UpdatePlayerEvent(events, fk_Player, fk_PlayerPosition, fk_PlayerGameWeak, scoreTypes), TimeSpan.FromMinutes(minutes));
                            minutes++;
                        }
                    }
                }
            }
        }

        public async Task UpdatePlayerState(Stat Stat, int fk_Player, int fk_PlayerPosition, int fk_PlayerGameWeak, List<ScoreTypeDto> scoreTypes)
        {
            PlayerGameWeakScore score = new()
            {
                Fk_PlayerGameWeak = fk_PlayerGameWeak,
                Fk_ScoreType = scoreTypes.Where(a => a._365_TypeId == Stat.Type.ToString()).Select(a => a.Id).First(),
                Value = Stat.Value,
            };

            _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(score, fk_Player, fk_PlayerGameWeak, fk_PlayerPosition));
            await _unitOfWork.Save();
        }

        public async Task UpdatePlayerEvent(EventType events, int fk_Player, int fk_PlayerPosition, int fk_PlayerGameWeak, List<ScoreTypeDto> scoreTypes)
        {
            PlayerGameWeakScore score = new()
            {
                Fk_PlayerGameWeak = fk_PlayerGameWeak,
                Fk_ScoreType = scoreTypes.Where(a => a.IsEvent = true && a._365_EventTypeId == events.Id.ToString() && a._365_TypeId == events.SubTypeId.ToString()).Select(a => a.Id).First(),
                Value = events.Value.ToString(),
                GameTime = events.GameTime,
            };
            _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(score, fk_Player, fk_PlayerGameWeak, fk_PlayerPosition));
            await _unitOfWork.Save();
        }
    }

    public class TeamGameWeakDto
    {
        public int Id { get; set; }

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
