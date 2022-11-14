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

        public PlayerStateCalc(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Calculations
        public void RunPlayersStateCalculations(GameWeakParameters parameters)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            parameters.Fk_Season = season.Id;

            List<int> gameWeaks = _unitOfWork.Season
                                             .GetGameWeaks(parameters, otherLang: false)
                                             .Select(a => a.Id).ToList();

            List<int> players = _unitOfWork.Team.GetPlayers(new PlayerParameters(), otherLang: false)
                .Select(a => a.Id)
                .ToList();

            //PlayersStateCalculations(players, season.Id, gameWeaks);

            _ = BackgroundJob.Enqueue(() => PlayersStateCalculations(players, season.Id, gameWeaks));
        }

        public void PlayersStateCalculations(List<int> players, int fk_Season, List<int> fk_GameWeaks)
        {
            string jobId = null;
            foreach (int player in players)
            {
                foreach (int fk_GameWeak in fk_GameWeaks)
                {
                    //PlayerStateCalculations(player, 0, fk_GameWeak, jobId);

                    jobId = jobId.IsExisting()
                        ? BackgroundJob.ContinueJobWith(jobId, () => PlayerStateCalculations(player, 0, fk_GameWeak, jobId))
                        : BackgroundJob.Enqueue(() => PlayerStateCalculations(player, 0, fk_GameWeak, jobId));
                }

                //PlayerStateCalculations(player, fk_Season, 0, jobId);

                jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => PlayerStateCalculations(player, fk_Season, 0, jobId))
                    : BackgroundJob.Enqueue(() => PlayerStateCalculations(player, fk_Season, 0, jobId));
            }
        }

        public string PlayerStateCalculations(int fk_Player, int fk_Season, int fk_GameWeak, string jobId)
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
                        else if (fk_GameWeak > 0)
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

            PlayerCalculations(fk_Player, fk_Season, fk_GameWeak);

            return jobId;
        }

        public void PlayerCalculations(int fk_Player, int fk_Season, int fk_GameWeak)
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
                    });
                    _unitOfWork.PlayerState.CreatePlayerSeasonScoreState(new PlayerSeasonScoreState
                    {
                        Fk_Season = fk_Season,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.PlayerCaptain,
                        Value = playescore.PlayerCaptain,
                    });
                }
                else if (fk_GameWeak > 0)
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
                    });
                    _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(new PlayerGameWeakScoreState
                    {
                        Fk_GameWeak = fk_GameWeak,
                        Fk_Player = fk_Player,
                        Fk_ScoreState = (int)ScoreStateEnum.PlayerCaptain,
                        Value = playescore.PlayerCaptain,
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
                else if (fk_GameWeak > 0)
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

        #region Position

        public void RunPlayersStatePositions(GameWeakParameters parameters)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            parameters.Fk_Season = season.Id;

            List<int> gameWeaks = _unitOfWork.Season
                                             .GetGameWeaks(parameters, otherLang: false)
                                             .Select(a => a.Id).ToList();

            List<int> scoreStates = _unitOfWork.PlayerState
                                               .GetScoreStates(new ScoreStateParameters(), otherLang: false)
                                               .Select(a => a.Id).ToList();
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
