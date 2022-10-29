using Entities.CoreServicesModels.PlayerScoreModels;
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

        public void RunPlayersStateCalculations(int fk_Season, int fk_GameWeak, List<int> scoreStates)
        {
            List<int> players = _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_GameWeak = fk_GameWeak
            }, otherLang: false)
                .Select(a => a.Id)
                .ToList();

            string jobId = null;
            foreach (int player in players)
            {
                _ = PlayerStateCalculations(player, fk_Season, fk_GameWeak, scoreStates, jobId);
            }
        }

        public string PlayerStateCalculations(int fk_Player, int fk_Season, int fk_GameWeak, List<int> scoreStates, string jobId)
        {
            if (scoreStates == null || !scoreStates.Any())
            {
                scoreStates.Add((int)ScoreStateEnum.CleanSheet);
                scoreStates.Add((int)ScoreStateEnum.Goals);
                scoreStates.Add((int)ScoreStateEnum.Assists);
                scoreStates.Add((int)ScoreStateEnum.GoalkeeperSaves);
                scoreStates.Add((int)ScoreStateEnum.PenaltiesSaved);
                scoreStates.Add((int)ScoreStateEnum.YellowCard);
                scoreStates.Add((int)ScoreStateEnum.RedCard);
                scoreStates.Add((int)ScoreStateEnum.YellowCard);
            }

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

            jobId = PlayerCalculations(fk_Player, fk_Season, fk_GameWeak, jobId);

            return jobId;
        }

        public string PlayerCalculations(int fk_Player, int fk_Season, int fk_GameWeak, string jobId)
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

            PlayerTotalScoreModel totalPlayescore = _unitOfWork.PlayerScore.GetPlayerTotalScores(fk_Player, fk_Season: 0, fk_GameWeak, fk_ScoreTypes: null);
            _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(new PlayerGameWeakScoreState
            {
                Fk_GameWeak = fk_GameWeak,
                Fk_Player = fk_Player,
                Fk_ScoreState = (int)ScoreStateEnum.Total,
                Points = totalPlayescore.Points,
                Value = totalPlayescore.FinalValue
            });
            _unitOfWork.Save().Wait();

            return jobId;
        }

        // Helper

        private List<int> GetScoreTypesByScoreState(int scoreState)
        {
            if (scoreState == (int)ScoreStateEnum.CleanSheet)
            {
                return new List<int>
                {
                    (int)ScoreTypeEnum.CleanSheet
                };
            }
            return scoreState == (int)ScoreStateEnum.Goals
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
