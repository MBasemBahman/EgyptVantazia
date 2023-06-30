﻿using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using IntegrationWith365.Helpers;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.DataMigration.SeasonData
{
    public class GameWeakDataHelper
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly GamesHelper _gamesHelper;

        public GameWeakDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            _unitOfWork = unitOfWork;
            _gamesHelper = new GamesHelper(unitOfWork, _365Services);
        }

        public void UpdateSeasonGameWeaks(_365CompetitionsEnum _365CompetitionsEnum, int _365_SeasonId)
        {
            List<int> rounds = _gamesHelper.GetAllGames(_365CompetitionsEnum, _365_SeasonId).Result.Select(a => a.RoundNum).Distinct().OrderBy(a => a).ToList();

            string jobId = null;
            foreach (int round in rounds)
            {
                jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGameWeak(round, _365_SeasonId))
                    : BackgroundJob.Enqueue(() => UpdateGameWeak(round, _365_SeasonId));
            }

            //_ = BackgroundJob.ContinueJobWith(jobId, () => UpdateCurrentGameWeak(fk_season, _365_SeasonId));
        }

        public async Task UpdateGameWeak(int round, int fk_Season)
        {
            _unitOfWork.Season.CreateGameWeak(new GameWeak
            {
                Name = round.ToString(),
                Fk_Season = fk_Season,
                _365_GameWeakId = round.ToString(),
                GameWeakLang = new GameWeakLang
                {
                    Name = round.ToString()
                }
            });
            await _unitOfWork.Save();
        }

        public async Task UpdateCurrentGameWeak(_365CompetitionsEnum _365CompetitionsEnum, int fk_Season, int _365_SeasonId)
        {
            int round = _gamesHelper.GetCurrentRound(_365CompetitionsEnum, _365_SeasonId);
            if (round > 0)
            {
                GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakby365Id(round.ToString(), fk_Season, trackChanges: true);
                if (gameWeak != null)
                {
                    gameWeak.IsCurrent = true;
                    await _unitOfWork.Save();
                }
            }
        }
    }
}
