﻿using CoreServices;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;
using FantasyLogic.Calculations;
using IntegrationWith365.Entities.GameModels;
using static Contracts.EnumData.DBModelsEnum;
using static Entities.EnumData.LogicEnumData;

namespace FantasyLogic.SharedLogic
{
    public class GameResultLogic
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly PlayerScoreCalc _playerScoreCalc;

        public GameResultLogic(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _playerScoreCalc = new PlayerScoreCalc();
        }

        #region Scores
        public void UpdatePlayerStateScore(
            List<Event> otherGoals,
            List<EventType> substitutions,
            int rankingIndex,
            PlayMinutesEnum playMinutes,
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

            _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(otherGoals, substitutions, score, rankingIndex, playMinutes, fk_PlayerPosition), ignoreCheck: false);
        }

        public void UpdatePlayerEventScore(
            List<Event> otherGoals,
            List<EventType> substitutions,
            int rankingIndex,
            PlayMinutesEnum playMinutes,
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

                _unitOfWork.PlayerScore.CreatePlayerGameWeakScore(_playerScoreCalc.GetPlayerScore(otherGoals, substitutions, score, rankingIndex, playMinutes, fk_PlayerPosition), ignoreCheck: false);
            }
        }
        #endregion

        #region TotalPoints
        public void UpdatePlayerGameWeakTotalPoints(int fk_PlayerGameWeak)
        {
            _unitOfWork.PlayerScore.UpdatePlayerGameWeakTotalPoints(fk_PlayerGameWeak);
        }
        #endregion
    }
}
