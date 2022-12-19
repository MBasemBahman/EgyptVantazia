using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;
using FantasyLogic.Calculations;
using IntegrationWith365.Entities.GameModels;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.SharedLogic
{
    public class GameResultLogic
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly PlayerScoreCalc _playerScoreCalc;

        public GameResultLogic(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _playerScoreCalc = new PlayerScoreCalc(unitOfWork);
        }

        #region Scores
        public async Task UpdatePlayerStateScore(int fk_ScoreType, string value, int fk_Player, int fk_Team, int fk_PlayerPosition, int fk_PlayerGameWeak, int fk_TeamGameWeak)
        {
            PlayerGameWeakScore score = new()
            {
                Fk_PlayerGameWeak = fk_PlayerGameWeak,
                Fk_ScoreType = fk_ScoreType,
                Value = value,
            };

            _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(score, fk_Player, fk_Team, fk_PlayerGameWeak, fk_PlayerPosition, fk_TeamGameWeak));
            await _unitOfWork.Save();
        }

        public async Task UpdatePlayerEventScore(EventType events, int fk_ScoreType, int fk_Player, int fk_Team, int fk_PlayerPosition, int fk_PlayerGameWeak, int fk_TeamGameWeak)
        {
            if (events.GameTime > 0)
            {
                PlayerGameWeakScore score = new()
                {
                    Fk_PlayerGameWeak = fk_PlayerGameWeak,
                    Fk_ScoreType = fk_ScoreType,
                    Value = events.Value.ToString(),
                    GameTime = events.GameTime,
                    IsOut = events.IsOut,
                };

                //if (fk_ScoreType == (int)ScoreTypeEnum.Goal_Event)
                //{
                //    PlayerGameWeakScore extraScore = new()
                //    {
                //        Fk_PlayerGameWeak = fk_PlayerGameWeak,
                //        Fk_ScoreType = (int)ScoreTypeEnum.Goals,
                //        Value = events.Value.ToString(),
                //    };

                //    if (_unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
                //    {
                //        Fk_ScoreType = extraScore.Fk_ScoreType,
                //        Fk_PlayerGameWeak = extraScore.Fk_PlayerGameWeak
                //    }, otherLang: false).Any())
                //    {
                //        string oldValue = _unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
                //        {
                //            Fk_ScoreType = extraScore.Fk_ScoreType,
                //            Fk_PlayerGameWeak = extraScore.Fk_PlayerGameWeak
                //        }, otherLang: false).Select(a => a.Value).FirstOrDefault();

                //        int newValue = extraScore.Value.ParseToInt() + oldValue.ParseToInt();

                //        extraScore.Value = newValue.ToString();
                //    }

                //    _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(extraScore, fk_Player, fk_Team, fk_PlayerGameWeak, fk_PlayerPosition, fk_TeamGameWeak));
                //}

                _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(score, fk_Player, fk_Team, fk_PlayerGameWeak, fk_PlayerPosition, fk_TeamGameWeak));
                await _unitOfWork.Save();
            }
        }
        #endregion

        #region TotalPoints
        public async Task UpdatePlayerGameWeakTotalPoints(int fk_PlayerGameWeak)
        {
            PlayerGameWeak playerGameWeak = await _unitOfWork.PlayerScore.FindPlayerGameWeakbyId(fk_PlayerGameWeak, trackChanges: true);
            playerGameWeak.TotalPoints = _unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
            {
                Fk_PlayerGameWeak = fk_PlayerGameWeak
            }, otherLang: false).Select(a => a.Points).Sum();
            await _unitOfWork.Save();
        }
        #endregion
    }
}
