using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;
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
            PlayerGameWeakScore score,
            int fk_Player,
            int fk_Team,
            int fk_PlayerGameWeak,
            int fk_PlayerPosition,
            int fk_TeamGameWeak,
            List<PlayerGameWeakScoreModel> goals)
        {
            List<PlayerGameWeakScoreModel> otherGoals = goals != null && goals.Any() ? goals.Where(a => a.Fk_Team != fk_Team).ToList() : null;
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
                    score = CalcCleanSheet(score, fk_PlayerPosition);
                }
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.ReceiveGoals &&
                     (fk_PlayerPosition is ((int)PlayerPositionEnum.Defender) or
                            ((int)PlayerPositionEnum.Goalkeeper)))
            {
                if (_unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
                {
                    CheckReceiveGoals = true,
                    Fk_Player = fk_Player,
                    Fk_PlayerGameWeak = fk_PlayerGameWeak
                }, otherLang: false).Any())
                {
                    score.FinalValue = otherGoals.Count;
                    bool addPoints = true;
                    if (_unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
                    {
                        Fk_Player = fk_Player,
                        Fk_PlayerGameWeak = fk_PlayerGameWeak,
                        Fk_ScoreType = (int)ScoreTypeEnum.Substitution
                    }, otherLang: false).Any())
                    {
                        PlayerGameWeakScoreModel substitution = _unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
                        {
                            Fk_Player = fk_Player,
                            Fk_PlayerGameWeak = fk_PlayerGameWeak,
                            Fk_ScoreType = (int)ScoreTypeEnum.Substitution
                        }, otherLang: false).First();

                        if (substitution.GameTime < otherGoals.First().GameTime)
                        {
                            addPoints = false;
                        }
                    }

                    if (addPoints)
                    {
                        score.Points = score.FinalValue / 2 * -1;
                    }
                    else
                    {
                        score = CalcCleanSheet(score, fk_PlayerPosition);
                    }
                }
            }
            else if (score.Fk_ScoreType == (int)ScoreTypeEnum.Ranking)
            {
                List<int> fk_Players = _unitOfWork.PlayerScore.GetPlayerGameWeakTop(fk_TeamGameWeak, 3);
                if (fk_Players.Contains(fk_Player))
                {
                    score.FinalValue = fk_Players.IndexOf(fk_Player);
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
            }

            return score;
        }

        private PlayerGameWeakScore CalcCleanSheet(PlayerGameWeakScore score, int fk_PlayerPosition)
        {
            score.FinalValue = 1;
            score.Fk_ScoreType = (int)ScoreTypeEnum.CleanSheet;
            if (fk_PlayerPosition == (int)PlayerPositionEnum.Midfielder)
            {
                score.Points = score.FinalValue * 1;
            }
            else if (fk_PlayerPosition is ((int)PlayerPositionEnum.Defender) or
                    ((int)PlayerPositionEnum.Goalkeeper))
            {
                score.Points = score.FinalValue * 4;
            }

            return score;
        }
    }
}
