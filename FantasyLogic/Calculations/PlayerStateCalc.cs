using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerStateModels;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.Calculations
{
    public class PlayerStateCalc
    {
        private readonly UnitOfWork _unitOfWork;

        public PlayerStateCalc(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Calculations
        public void RunPlayersStateCalculations(int fk_GameWeak, string _365_MatchId)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            List<int> gameWeaks = _unitOfWork.Season
                                             .GetGameWeaks(new GameWeakParameters
                                             {
                                                 Fk_Season = season.Id,
                                                 Id = fk_GameWeak
                                             }, otherLang: false)
                                             .Select(a => a.Id).ToList();

            //PlayersStateCalculations(season.Id, gameWeaks, _365_MatchId);

            _ = BackgroundJob.Enqueue(() => PlayersStateCalculations(season.Id, gameWeaks, _365_MatchId));
        }

        public void PlayersStateCalculations(int fk_Season, List<int> fk_GameWeaks, string _365_MatchId)
        {
            string jobId = null;
            List<int> players = null;
            int playerSelectionCount = 0;
            int playerCaptainCount = 0;

            foreach (int fk_GameWeak in fk_GameWeaks)
            {
                players = _unitOfWork.Team.GetPlayers(new PlayerParameters
                {
                    Fk_Season = fk_Season,
                    Fk_GameWeak = fk_GameWeak,
                    _365_MatchId = _365_MatchId
                }, otherLang: false)
                .Select(a => a.Id)
                .ToList();

                if (players != null && players.Any())
                {
                    playerSelectionCount = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                    {
                        Fk_GameWeak = fk_GameWeak
                    }, otherLang: false).Count();

                    playerCaptainCount = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                    {
                        Fk_GameWeak = fk_GameWeak,
                        Fk_TeamPlayerType = (int)TeamPlayerTypeEnum.Captian
                    }, otherLang: false).Count();

                    foreach (int player in players)
                    {
                        //PlayerStateCalculations(player, 0, fk_GameWeak, playerSelectionCount, playerCaptainCount, jobId);

                        jobId = jobId.IsExisting()
                            ? BackgroundJob.ContinueJobWith(jobId, () => PlayerStateCalculations(player, 0, fk_GameWeak, playerSelectionCount, playerCaptainCount, jobId))
                            : BackgroundJob.Enqueue(() => PlayerStateCalculations(player, 0, fk_GameWeak, playerSelectionCount, playerCaptainCount, jobId));
                    }
                }

            }

            players = _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_Season = fk_Season,
                _365_MatchId = _365_MatchId
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
                    //PlayerStateCalculations(player, fk_Season, 0, playerSelectionCount, playerCaptainCount, jobId);

                    jobId = jobId.IsExisting()
                        ? BackgroundJob.ContinueJobWith(jobId, () => PlayerStateCalculations(player, fk_Season, 0, playerSelectionCount, playerCaptainCount, jobId))
                        : BackgroundJob.Enqueue(() => PlayerStateCalculations(player, fk_Season, 0, playerSelectionCount, playerCaptainCount, jobId));
                }
            }
        }

        public string PlayerStateCalculations(
            int fk_Player,
            int fk_Season,
            int fk_GameWeak,
            int playerSelectionCount,
            int playerCaptainCount,
            string jobId)
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

            PlayerCalculations(fk_Player, fk_Season, fk_GameWeak, playerSelectionCount, playerCaptainCount);

            return jobId;
        }

        public void PlayerCalculations(
            int fk_Player,
            int fk_Season,
            int fk_GameWeak,
            int playerSelectionCount,
            int playerCaptainCount)
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
                    (int)ScoreTypeEnum.YellowCard,
                    (int)ScoreTypeEnum.SecondYellowCard
                }
                : scoreState == (int)ScoreStateEnum.RedCard
                ? new List<int>
                {
                    (int)ScoreTypeEnum.RedCard
                }
                : null;
        }
    }
}
