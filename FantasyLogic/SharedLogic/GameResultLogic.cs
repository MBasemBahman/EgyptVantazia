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
        public void UpdatePlayerStateScore(
            List<Event> otherGoals, 
            List<EventType> substitutions, 
            int rankingIndex, 
            bool canGetCleanSheat, 
            int fk_ScoreType, 
            string value, 
            int fk_PlayerPosition, 
            int fk_PlayerGameWeak)
        {
            PlayerGameWeakScore score = new()
            {
                Fk_PlayerGameWeak = fk_PlayerGameWeak,
                Fk_ScoreType = fk_ScoreType,
                Value = value,
            };

            _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(otherGoals, substitutions, score, rankingIndex, canGetCleanSheat, fk_PlayerPosition));
        }

        public void UpdatePlayerEventScore(
            List<Event> otherGoals, 
            List<EventType> substitutions, 
            int rankingIndex, 
            bool canGetCleanSheat,
            EventType events, 
            int fk_ScoreType, 
            int fk_PlayerPosition, 
            int fk_PlayerGameWeak)
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

                _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(otherGoals, substitutions, score, rankingIndex, canGetCleanSheat, fk_PlayerPosition));
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
        }
        #endregion
    }
}
