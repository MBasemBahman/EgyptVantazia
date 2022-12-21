using Entities.DBModels.PlayerScoreModels;
using IntegrationWith365.Entities.GameModels;
using System.Linq.Dynamic.Core;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.Calculations
{
    public class PlayerScoreCalc
    {
        private readonly UnitOfWork _unitOfWork;

        public PlayerScoreCalc(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PlayerGameWeakScore GetPlayerScore(
            List<Event> otherGoals,
            List<EventType> substitutions,
            PlayerGameWeakScore score,
            int rankingIndex,
            bool canGetCleanSheat,
            int fk_PlayerPosition)
        {
            if (score.Fk_ScoreType == (int)ScoreTypeEnum.Minutes)
            {
                score.FinalValue = score.Value.GetUntilOrEmpty("'").ParseToInt();
                if (score.FinalValue is > 0 and < 60)
                {
                    score.Points = 1;
                }
                else if (score.FinalValue >= 60)
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
                score.FinalValue = score.Value.Contains('(') ? score.Value.GetUntilOrEmpty("(").ParseToInt() : score.Value.ParseToInt();
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
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.RedCard_Event)
            {
                score.FinalValue = score.Value.ParseToInt();
                score.Points = score.FinalValue * -3;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.SecondYellowCard_Event)
            {
                score.FinalValue = score.Value.ParseToInt();
                score.Points = score.FinalValue * -2;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.YellowCard_Event)
            {
                score.FinalValue = score.Value.ParseToInt();
                score.Points = score.FinalValue * -1;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.SelfGoal_Event)
            {
                score.FinalValue = score.Value.ParseToInt();
                score.Points = score.FinalValue * -2;
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.CleanSheet && canGetCleanSheat)
            {
                if (otherGoals == null || !otherGoals.Any())
                {
                    score = CalcCleanSheet(score, fk_PlayerPosition);
                }
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.ReceiveGoals)
            {
                if (otherGoals != null && otherGoals.Any())
                {
                    int gaolsCount = otherGoals.Count;

                    if (substitutions != null && substitutions.Any())
                    {
                        if (substitutions.Any(a => a.IsOut == true)) // TRUE
                        {
                            gaolsCount = otherGoals.Count(a => a.GameTime < substitutions.First().GameTime);
                        }
                        else if (substitutions.Any(a => a.IsOut == false)) // FALSE
                        {
                            gaolsCount = otherGoals.Count(a => a.GameTime > substitutions.First().GameTime);
                        }
                    }

                    if (gaolsCount > 0)
                    {
                        if (fk_PlayerPosition is ((int)PlayerPositionEnum.Defender) or
                            ((int)PlayerPositionEnum.Goalkeeper))
                        {
                            score.FinalValue = gaolsCount;
                            score.Points = score.FinalValue / 2 * -1;
                        }
                    }
                    else
                    {
                        if (canGetCleanSheat)
                        {
                            score = CalcCleanSheet(score, fk_PlayerPosition);
                        }
                    }
                }
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.Ranking)
            {
                score.FinalValue = rankingIndex;
                if (score.FinalValue == 1)
                {
                    score.Points = 3;
                }
                else if (score.FinalValue == 2)
                {
                    score.Points = 2;
                }
                else if (score.FinalValue == 3)
                {
                    score.Points = 1;
                }
            }

            return score;
        }

        private PlayerGameWeakScore CalcCleanSheet(PlayerGameWeakScore score, int fk_PlayerPosition)
        {
            score.FinalValue = 1;
            score.Fk_ScoreType = (int)ScoreTypeEnum.CleanSheet;
            score.Points = 0;

            if (fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder)
            {
                score.Points = 1;
            }
            else if (fk_PlayerPosition is ((int)PlayerPositionEnum.Defender) or
                                    ((int)PlayerPositionEnum.Goalkeeper))
            {
                score.Points = 4;
            }

            return score;
        }
    }
}
