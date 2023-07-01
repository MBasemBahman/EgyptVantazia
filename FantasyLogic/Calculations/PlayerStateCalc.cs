using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerStateModels;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.Calculations
{
    public class PlayerStateCalc
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly AccountTeamCalc _accountTeamCalc;
        private readonly HangFireCustomJob _hangFireCustomJob;

        private readonly string JobGameWeekPlayerId = "PlayerGameWeekStatesCalc-";
        private readonly string JobSeasonPlayerId = "PlayerSeasonStatesCalc-";

        public PlayerStateCalc(
            UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _hangFireCustomJob = new HangFireCustomJob(unitOfWork);
            _accountTeamCalc = new(_unitOfWork);
        }

        #region Calculations
        public void RunPlayersStateCalculations(_365CompetitionsEnum _365CompetitionsEnum,int fk_GameWeak, string _365_MatchId, List<int> fk_Players, List<int> fk_Teams, bool ignoreGameWeaks, bool inDebug)
        {
            int season = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);

            List<int> gameWeaks = ignoreGameWeaks ? new List<int>() :
                                             _unitOfWork.Season
                                             .GetGameWeaks(new GameWeakParameters
                                             {
                                                 Fk_Season = season,
                                                 Id = fk_GameWeak
                                             }).Select(a => a.Id).ToList();

            PlayersStateCalculations(season, gameWeaks, _365_MatchId, fk_Players, fk_Teams, inDebug);
        }

        public void PlayersStateCalculations(int fk_Season, List<int> fk_GameWeaks, string _365_MatchId, List<int> fk_Players, List<int> fk_Teams, bool inDebug)
        {
            List<int> players = null;
            int playerSelectionCount = 0;
            int playerCaptainCount = 0;

            foreach (int fk_GameWeak in fk_GameWeaks)
            {
                players = _unitOfWork.Team.GetPlayers(new PlayerParameters
                {
                    Fk_Season = fk_Season,
                    Fk_GameWeak = fk_GameWeak,
                    _365_MatchId = _365_MatchId,
                    Fk_Players = fk_Players
                }).Select(a => a.Id)
                .ToList();

                if (players != null && players.Any())
                {
                    playerSelectionCount = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                    {
                        Fk_GameWeak = fk_GameWeak
                    }).Select(a => a.AccountTeamPlayer.Fk_Player).Distinct().Count();

                    playerCaptainCount = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                    {
                        Fk_GameWeak = fk_GameWeak,
                        Fk_TeamPlayerType = (int)TeamPlayerTypeEnum.Captian
                    }).Select(a => a.AccountTeamPlayer.Fk_Player).Distinct().Count();

                    foreach (int player in players)
                    {
                        if (inDebug)
                        {
                            PlayerStateCalculations(player, 0, fk_GameWeak, playerSelectionCount, playerCaptainCount, inDebug);
                        }
                        else
                        {
                            string recurringId = JobGameWeekPlayerId + $"{fk_GameWeak}-{player}";

                            string hangfireJobId = BackgroundJob.Enqueue(() => PlayerStateCalculations(player, 0, fk_GameWeak, playerSelectionCount, playerCaptainCount, inDebug));

                            _hangFireCustomJob.ReplaceJob(hangfireJobId, recurringId);
                        }
                    }
                }
            }

            players = _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_Season = fk_Season,
                _365_MatchId = _365_MatchId,
                Fk_Players = fk_Players,
                Fk_Teams = fk_Teams,
            }, otherLang: false)
               .Select(a => a.Id)
               .ToList();

            if (players != null && players.Any())
            {
                playerSelectionCount = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                {
                    Fk_Season = fk_Season
                }, otherLang: false).Count();

                playerCaptainCount = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                {
                    Fk_Season = fk_Season,
                    Fk_TeamPlayerType = (int)TeamPlayerTypeEnum.Captian
                }, otherLang: false).Count();

                foreach (int player in players)
                {
                    if (inDebug)
                    {
                        PlayerStateCalculations(player, fk_Season, 0, playerSelectionCount, playerCaptainCount, inDebug);
                    }
                    else
                    {
                        string recurringId = JobSeasonPlayerId + $"{fk_Season}-{player}";

                        string hangfireJobId = BackgroundJob.Enqueue(() => PlayerStateCalculations(player, fk_Season, 0, playerSelectionCount, playerCaptainCount, inDebug));

                        _hangFireCustomJob.ReplaceJob(hangfireJobId, recurringId);
                    }
                }
            }
        }

        public void PlayerStateCalculations(
            int fk_Player,
            int fk_Season,
            int fk_GameWeak,
            int playerSelectionCount,
            int playerCaptainCount,
            bool inDebug)
        {
            List<int> scoreStates = new()
            {
                    (int)ScoreStateEnum.CleanSheet,
                    (int)ScoreStateEnum.Goals,
                    (int)ScoreStateEnum.Assists,
                    (int)ScoreStateEnum.GoalkeeperSaves,
                    (int)ScoreStateEnum.PenaltiesSaved,
                    (int)ScoreStateEnum.YellowCard,
                    (int)ScoreStateEnum.RedCard,
                };

            foreach (int scoreState in scoreStates)
            {
                List<int> ScoreTypes = GetScoreTypesByScoreState(scoreState);
                if (ScoreTypes != null && ScoreTypes.Any())
                {
                    PlayerTotalScoreModel playescore = _unitOfWork.PlayerScore.GetPlayerTotalScores(fk_Player, fk_Season, fk_GameWeak, ScoreTypes);
                    if (playescore != null)
                    {
                        if (fk_Season > 0)
                        {
                            _unitOfWork.PlayerState.CreatePlayerSeasonScoreState(new PlayerSeasonScoreState
                            {
                                Fk_Season = fk_Season,
                                Fk_Player = fk_Player,
                                Fk_ScoreState = scoreState,
                                Points = playescore.Points,
                                Value = playescore.FinalValue
                            });
                        }
                        if (fk_GameWeak > 0)
                        {
                            _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(new PlayerGameWeakScoreState
                            {
                                Fk_GameWeak = fk_GameWeak,
                                Fk_Player = fk_Player,
                                Fk_ScoreState = scoreState,
                                Points = playescore.Points,
                                Value = playescore.FinalValue
                            });
                        }

                        _unitOfWork.Save().Wait();
                    }
                }
            }

            PlayerCalculations(fk_Player, fk_Season, fk_GameWeak, playerSelectionCount, playerCaptainCount, inDebug);
        }

        public void PlayerCalculations(
            int fk_Player,
            int fk_Season,
            int fk_GameWeak,
            int playerSelectionCount,
            int playerCaptainCount,
            bool inDebug)
        {
            PlayerCustomStateResult playescore = _unitOfWork.Team.GetPlayerCustomStateResult(fk_Player, fk_Season, fk_GameWeak);

            if (playescore != null)
            {
                if (fk_Season > 0)
                {
                    _unitOfWork.PlayerState.CreatePlayerSeasonScoreState(new PlayerSeasonScoreState
                    {
                        Fk_Season = fk_Season,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.BuyingPrice,
                        Value = playescore.BuyingPrice,
                    });
                    _unitOfWork.PlayerState.CreatePlayerSeasonScoreState(new PlayerSeasonScoreState
                    {
                        Fk_Season = fk_Season,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.SellingPrice,
                        Value = playescore.SellingPrice,
                    });
                    _unitOfWork.PlayerState.CreatePlayerSeasonScoreState(new PlayerSeasonScoreState
                    {
                        Fk_Season = fk_Season,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.BuyingCount,
                        Value = playescore.BuyingCount,
                    });
                    _unitOfWork.PlayerState.CreatePlayerSeasonScoreState(new PlayerSeasonScoreState
                    {
                        Fk_Season = fk_Season,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.SellingCount,
                        Value = playescore.SellingCount,
                    });
                    _unitOfWork.PlayerState.CreatePlayerSeasonScoreState(new PlayerSeasonScoreState
                    {
                        Fk_Season = fk_Season,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.PlayerSelection,
                        Value = playescore.PlayerSelection,
                        Percent = playescore.PlayerSelection / playerSelectionCount * 100
                    });
                    _unitOfWork.PlayerState.CreatePlayerSeasonScoreState(new PlayerSeasonScoreState
                    {
                        Fk_Season = fk_Season,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.PlayerCaptain,
                        Value = playescore.PlayerCaptain,
                        Percent = playescore.PlayerCaptain / playerCaptainCount * 100
                    });
                }
                if (fk_GameWeak > 0)
                {
                    _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(new PlayerGameWeakScoreState
                    {
                        Fk_GameWeak = fk_GameWeak,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.BuyingPrice,
                        Value = playescore.BuyingPrice,
                    });
                    _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(new PlayerGameWeakScoreState
                    {
                        Fk_GameWeak = fk_GameWeak,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.SellingPrice,
                        Value = playescore.SellingPrice,
                    });
                    _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(new PlayerGameWeakScoreState
                    {
                        Fk_GameWeak = fk_GameWeak,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.BuyingCount,
                        Value = playescore.BuyingCount,
                    });
                    _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(new PlayerGameWeakScoreState
                    {
                        Fk_GameWeak = fk_GameWeak,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.SellingCount,
                        Value = playescore.SellingCount,
                    });
                    _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(new PlayerGameWeakScoreState
                    {
                        Fk_GameWeak = fk_GameWeak,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.PlayerSelection,
                        Value = playescore.PlayerSelection,
                        Percent = playescore.PlayerSelection / playerSelectionCount * 100
                    });
                    _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(new PlayerGameWeakScoreState
                    {
                        Fk_GameWeak = fk_GameWeak,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.PlayerCaptain,
                        Value = playescore.PlayerCaptain,
                        Percent = playescore.PlayerCaptain / playerCaptainCount * 100
                    });
                }
                _unitOfWork.Save().Wait();
            }

            PlayerTotalScoreModel totalPlayescore = _unitOfWork.PlayerScore.GetPlayerTotalScores(fk_Player, fk_Season, fk_GameWeak, fk_ScoreTypes: null);

            if (totalPlayescore != null)
            {
                if (fk_Season > 0)
                {
                    _unitOfWork.PlayerState.CreatePlayerSeasonScoreState(new PlayerSeasonScoreState
                    {
                        Fk_Season = fk_Season,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.Total,
                        Points = totalPlayescore.Points,
                        Value = totalPlayescore.FinalValue
                    });
                }
                if (fk_GameWeak > 0)
                {
                    _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(new PlayerGameWeakScoreState
                    {
                        Fk_GameWeak = fk_GameWeak,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.Total,
                        Points = totalPlayescore.Points,
                        Value = totalPlayescore.FinalValue
                    });
                }
                _unitOfWork.Save().Wait();
            }
        }

        public async Task UpdateTop15(int fk_GameWeak, int fk_Season)
        {
            if (fk_GameWeak > 0)
            {
                _unitOfWork.PlayerState.ResetPlayerGameWeakScoreStateTop15(fk_GameWeak);

                List<PlayerGameWeakScoreState> players = _unitOfWork.PlayerState.FindPlayerGameWeakScoreStates(new PlayerGameWeakScoreStateParameters
                {
                    Fk_ScoreState = (int)ScoreStateEnum.Total,
                    Fk_GameWeak = fk_GameWeak,
                }, trackChanges: true)
                .OrderByDescending(a => a.Points)
                .ThenBy(a => a.Fk_Player)
                .Skip(0)
                .Take(15)
                .ToList();

                int ranking = 1;

                players.ForEach(PlayerGameWeakScore => PlayerGameWeakScore.Top15 = ranking++);

                await _unitOfWork.Save();
            }

            if (fk_Season > 0)
            {
                _unitOfWork.PlayerState.ResetPlayerSeasonScoreStateTop15(fk_Season);

                List<PlayerSeasonScoreState> players = _unitOfWork.PlayerState.FindPlayerSeasonScoreStates(new PlayerSeasonScoreStateParameters
                {
                    Fk_ScoreState = (int)ScoreStateEnum.Total,
                    Fk_Season = fk_Season,
                }, trackChanges: true)
                .OrderByDescending(a => a.Points)
                .ThenBy(a => a.Fk_Player)
                .Skip(0)
                .Take(15)
                .ToList();

                int ranking = 1;

                players.ForEach(PlayerGameWeakScore => PlayerGameWeakScore.Top15 = ranking++);

                await _unitOfWork.Save();
            }

        }
        #endregion

        // Helper

        private List<int> GetScoreTypesByScoreState(int scoreState)
        {
            return scoreState == (int)ScoreStateEnum.CleanSheet
                ? new List<int>
                {
                    (int)ScoreTypeEnum.CleanSheet
                }
                : scoreState == (int)ScoreStateEnum.Goals
                ? new List<int>
                {
                    (int)ScoreTypeEnum.Goals
                }
                : scoreState == (int)ScoreStateEnum.Assists
                ? new List<int>
                {
                    (int)ScoreTypeEnum.Assists
                }
                : scoreState == (int)ScoreStateEnum.GoalkeeperSaves
                ? new List<int>
                {
                    (int)ScoreTypeEnum.GoalkeeperSaves
                }
                : scoreState == (int)ScoreStateEnum.PenaltiesSaved
                ? new List<int>
                {
                    (int)ScoreTypeEnum.PenaltiesSaved
                }
                : scoreState == (int)ScoreStateEnum.YellowCard
                ? new List<int>
                {
                    (int)ScoreTypeEnum.YellowCard_Event,
                    (int)ScoreTypeEnum.SecondYellowCard_Event
                }
                : scoreState == (int)ScoreStateEnum.RedCard
                ? new List<int>
                {
                    (int)ScoreTypeEnum.RedCard_Event
                }
                : null;
        }
    }
}
