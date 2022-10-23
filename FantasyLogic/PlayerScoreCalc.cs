using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic
{
    public class PlayerScoreCalc
    {
        private readonly UnitOfWork _unitOfWork;

        public PlayerScoreCalc(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PlayerGameWeakScore GetPlayerScore(PlayerGameWeakScore score, int fk_Player, int fk_PlayerGameWeak, int fk_PlayerPosition)
        {
            if (score.Fk_ScoreType == (int)ScoreTypeEnum.Minutes)
            {
                score.FinalValue = score.Value.GetUntilOrEmpty("'").ParseToInt();
                if (score.FinalValue is > 0 and <= 60)
                {
                    score.Points = 1;
                }
                else if (score.FinalValue > 60)
                {
                    score.Points = 2;
                }
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.GoalkeeperSaves)
            {
                score.FinalValue = score.Value.ParseToInt();
                score.Points = score.FinalValue / 3 * 1;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.Goals)
            {
                score.FinalValue = score.Value.GetUntilOrEmpty("(").ParseToInt();
                if (score.FinalValue > 0)
                {
                    if (fk_PlayerPosition == (int)PlayerPositionEnum.Attacker)
                    {
                        score.Points = score.FinalValue * 4;
                    }
                    else if (fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder)
                    {
                        score.Points = score.FinalValue * 5;
                    }
                    else if (fk_PlayerPosition is ((int)PlayerPositionEnum.Defender) or
                            ((int)PlayerPositionEnum.Goalkeeper))
                    {
                        score.Points = score.FinalValue * 6;
                    }
                }
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.Assists)
            {
                score.FinalValue = score.Value.ParseToInt();
                if (score.FinalValue > 0)
                {
                    score.Points = score.FinalValue * 3;
                }
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.PenaltiesSaved)
            {
                score.FinalValue = score.Value.GetUntilOrEmpty("/").ParseToInt();
                score.Points = score.FinalValue * 5;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.PenaltyMissed)
            {
                score.FinalValue = score.Value.ParseToInt();
                score.Points = score.FinalValue * -2;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.RedCard)
            {
                score.FinalValue = score.Value.ParseToInt();
                score.Points = score.FinalValue * -3;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.SecondYellowCard)
            {
                score.FinalValue = score.Value.ParseToInt();
                score.Points = score.FinalValue * -2;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.YellowCard)
            {
                score.FinalValue = score.Value.ParseToInt();
                score.Points = score.FinalValue * -1;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.SelfGoal)
            {
                score.FinalValue = score.Value.ParseToInt();
                score.Points = score.FinalValue * -2;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.CleanSheet)
            {
                if (_unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
                {
                    CheckCleanSheet = true,
                    Fk_Player = fk_Player,
                    Fk_PlayerGameWeak = fk_PlayerGameWeak
                }, otherLang: false).Any())
                {
                    score.FinalValue = 1;
                    if (fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder)
                    {
                        score.Points = score.FinalValue * 1;
                    }
                    else if (fk_PlayerPosition is ((int)PlayerPositionEnum.Defender) or
                            ((int)PlayerPositionEnum.Goalkeeper))
                    {
                        score.Points = score.FinalValue * 4;
                    }
                }

            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.ReceiveGoals)
            {
                if (_unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
                {
                    CheckReceiveGoals = true,
                    Fk_Player = fk_Player,
                    Fk_PlayerGameWeak = fk_PlayerGameWeak
                }, otherLang: false).Any())
                {
                    score.FinalValue = 1;
                    if (fk_PlayerPosition is ((int)PlayerPositionEnum.Defender) or
                            ((int)PlayerPositionEnum.Goalkeeper))
                    {
                        score.Points = score.FinalValue / 2 * -1;
                    }
                }
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.Ranking)
            {
                score.FinalValue = 0;
                score.Points = 0;
            }

            return score;
        }

    }
}
