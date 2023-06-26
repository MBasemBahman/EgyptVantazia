using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.DBModels.PlayerScoreModels;
using IntegrationWith365.Entities.GameModels;
using IntegrationWith365.Entities.GamesModels;

namespace FantasyLogic.DataMigration.PlayerScoreData
{
    public class ScoreTypeDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;

        public ScoreTypeDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
        }

        public void RunUpdateStates(int delayMinutes, bool inDedug)
        {
            int season = _unitOfWork.Season.GetCurrentSeasonId();

            List<string> teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
                Fk_Season = season
            }).Select(a => a._365_MatchId).ToList();

            foreach (string _365_MatchId in teamGameWeaks)
            {
                if (inDedug)
                {
                    UpdateMatchStates(_365_MatchId.ParseToInt(), delayMinutes, inDedug).Wait();
                }
                else
                {
                    _ = BackgroundJob.Schedule(() => UpdateMatchStates(_365_MatchId.ParseToInt(), delayMinutes, inDedug), TimeSpan.FromMinutes(delayMinutes));
                }
            }
        }

        public async Task UpdateMatchStates(int _365_MatchId, int delayMinutes, bool inDedug)
        {
            GameReturn gameReturnInArabic = await _365Services.GetGame(new _365GameParameters
            {
                GameId = _365_MatchId,
                IsArabic = true
            });

            if (gameReturnInArabic != null && gameReturnInArabic.Game != null)
            {
                GameReturn gameReturnInEnglish = await _365Services.GetGame(new _365GameParameters
                {
                    GameId = _365_MatchId,
                });

                //UpdateStats(gameReturnInArabic.Game, gameReturnInEnglish.Game, delayMinutes);
                //UpdateEvents(gameReturnInArabic.Game, gameReturnInEnglish.Game, delayMinutes);
                UpdateStatistics(gameReturnInArabic.Game, gameReturnInEnglish.Game, delayMinutes, inDedug);
            }
        }

        public void UpdateStats(Game gameInArabic, Game gameInEnglish, int delayMinutes)
        {
            List<Stat> statsInArabic = new();
            List<Stat> statsInEnglish = new();

            if (gameInArabic != null)
            {
                statsInArabic.AddRange(gameInArabic.HomeCompetitor
                                                   .Lineups
                                                   .Members
                                                   .Where(a => a.Stats != null && a.Stats.Any())
                                                   .SelectMany(a => a.Stats)
                                                   .GroupBy(a => new Stat
                                                   {
                                                       Type = a.Type,
                                                       Name = a.Name,
                                                   })
                                                   .Select(a => a.Key)
                                                   .ToList());

                statsInArabic.AddRange(gameInArabic.AwayCompetitor
                                                   .Lineups
                                                   .Members
                                                   .Where(a => a.Stats != null && a.Stats.Any())
                                                   .SelectMany(a => a.Stats)
                                                   .GroupBy(a => new Stat
                                                   {
                                                       Type = a.Type,
                                                       Name = a.Name,
                                                   })
                                                   .Select(a => a.Key)
                                                   .ToList());

                statsInArabic = statsInArabic.GroupBy(a => new Stat
                {
                    Type = a.Type,
                    Name = a.Name,
                }).Select(a => a.Key).ToList();
            }
            if (gameInEnglish != null)
            {
                statsInEnglish.AddRange(gameInEnglish.HomeCompetitor
                                                     .Lineups
                                                     .Members
                                                     .Where(a => a.Stats != null && a.Stats.Any())
                                                     .SelectMany(a => a.Stats)
                                                      .GroupBy(a => new Stat
                                                      {
                                                          Type = a.Type,
                                                          Name = a.Name,
                                                      })
                                                     .Select(a => a.Key)
                                                     .ToList());

                statsInEnglish.AddRange(gameInEnglish.AwayCompetitor
                                                     .Lineups
                                                     .Members
                                                     .Where(a => a.Stats != null && a.Stats.Any())
                                                     .SelectMany(a => a.Stats)
                                                     .GroupBy(a => new Stat
                                                     {
                                                         Type = a.Type,
                                                         Name = a.Name,
                                                     })
                                                     .Select(a => a.Key)
                                                     .ToList());

                statsInEnglish = statsInEnglish.GroupBy(a => new Stat
                {
                    Type = a.Type,
                    Name = a.Name,
                }).Select(a => a.Key).ToList();
            }

            for (int i = 0; i < statsInArabic.Count; i++)
            {
                _ = BackgroundJob.Schedule(() => UpdateStat(statsInArabic[i], statsInEnglish[i]), TimeSpan.FromMinutes(delayMinutes));

            }
        }
        public async Task UpdateStat(Stat statInArabic, Stat statInEnglish)
        {
            _unitOfWork.PlayerScore.CreateScoreType(new ScoreType
            {
                Name = statInArabic.Name,
                _365_TypeId = statInArabic.Type.ToString(),
                ScoreTypeLang = new ScoreTypeLang
                {
                    Name = statInEnglish.Name,
                }
            });
            await _unitOfWork.Save();
        }

        public void UpdateEvents(Game gameInArabic, Game gameInEnglish, int delayMinutes)
        {
            List<EventType> eventsInArabic = new();
            List<EventType> eventsInEnglish = new();

            if (gameInArabic != null && gameInArabic.Events != null && gameInArabic.Events.Any())
            {
                eventsInArabic.AddRange(gameInArabic.Events
                                                    .Where(a => a.EventType != null)
                                                    .Select(a => a.EventType)
                                                    .Distinct()
                                                    .ToList());
            }
            if (gameInEnglish != null && gameInEnglish.Events != null && gameInEnglish.Events.Any())
            {
                eventsInEnglish.AddRange(gameInEnglish.Events
                                                      .Where(a => a.EventType != null)
                                                      .Select(a => a.EventType)
                                                      .Distinct()
                                                      .ToList());
            }

            for (int i = 0; i < eventsInArabic.Count; i++)
            {
                _ = BackgroundJob.Schedule(() => UpdateEvent(eventsInArabic[i], eventsInEnglish[i]), TimeSpan.FromMinutes(delayMinutes));

            }
        }
        public async Task UpdateEvent(EventType eventInArabic, EventType eventInEnglish)
        {
            _unitOfWork.PlayerScore.CreateScoreType(new ScoreType
            {
                Name = $"{eventInArabic.Name} - {eventInArabic.SubTypeName}",
                _365_TypeId = eventInArabic.SubTypeId.ToString(),
                IsEvent = true,
                _365_EventTypeId = eventInArabic.Id.ToString(),
                ScoreTypeLang = new ScoreTypeLang
                {
                    Name = $"{eventInEnglish.Name} - {eventInEnglish.SubTypeName}",
                }
            });
            await _unitOfWork.Save();
        }

        public void UpdateStatistics(Game gameInArabic, Game gameInEnglish, int delayMinutes, bool inDedug)
        {
            List<Statistics> statisticsInArabic = new();
            List<Statistics> statisticsInEnglish = new();

            if (gameInArabic != null &&
                gameInArabic.HomeCompetitor != null &&
                gameInArabic.HomeCompetitor.Statistics != null &&
                gameInArabic.HomeCompetitor.Statistics.Any())
            {
                statisticsInArabic.AddRange(gameInArabic.HomeCompetitor
                                                        .Statistics
                                                        .Select(a => new Statistics
                                                        {
                                                            CategoryId = a.CategoryId,
                                                            Name = a.Name,
                                                            CategoryName = a.CategoryName,
                                                            Id = a.Id,
                                                        })
                                                        .Distinct()
                                                        .ToList());
            }

            if (gameInArabic != null &&
                gameInArabic.AwayCompetitor != null &&
                gameInArabic.AwayCompetitor.Statistics != null &&
                gameInArabic.AwayCompetitor.Statistics.Any())
            {
                statisticsInArabic.AddRange(gameInArabic.AwayCompetitor
                                                        .Statistics
                                                        .Select(a => new Statistics
                                                        {
                                                            CategoryId = a.CategoryId,
                                                            Name = a.Name,
                                                            CategoryName = a.CategoryName,
                                                            Id = a.Id,
                                                        })
                                                        .Distinct()
                                                        .ToList());
            }

            statisticsInArabic = statisticsInArabic.Distinct().ToList();

            if (gameInEnglish != null &&
                gameInEnglish.HomeCompetitor != null &&
                gameInEnglish.HomeCompetitor.Statistics != null &&
                gameInEnglish.HomeCompetitor.Statistics.Any())
            {
                statisticsInEnglish.AddRange(gameInEnglish.HomeCompetitor
                                                        .Statistics
                                                        .Select(a => new Statistics
                                                        {
                                                            CategoryId = a.CategoryId,
                                                            Name = a.Name,
                                                            CategoryName = a.CategoryName,
                                                            Id = a.Id,
                                                        })
                                                        .Distinct()
                                                        .ToList());
            }

            if (gameInEnglish != null &&
                gameInEnglish.AwayCompetitor != null &&
                gameInEnglish.AwayCompetitor.Statistics != null &&
                gameInEnglish.AwayCompetitor.Statistics.Any())
            {
                statisticsInEnglish.AddRange(gameInEnglish.AwayCompetitor
                                                        .Statistics
                                                        .Select(a => new Statistics
                                                        {
                                                            CategoryId = a.CategoryId,
                                                            Name = a.Name,
                                                            CategoryName = a.CategoryName,
                                                            Id = a.Id,
                                                        })
                                                        .Distinct()
                                                        .ToList());
            }

            statisticsInEnglish = statisticsInEnglish.Distinct().ToList();

            for (int i = 0; i < statisticsInArabic.Count; i++)
            {
                if (inDedug)
                {
                    UpdateStatistics(statisticsInArabic[i], statisticsInEnglish[i]).Wait();
                }
                else
                {
                    _ = BackgroundJob.Schedule(() => UpdateStatistics(statisticsInArabic[i], statisticsInEnglish[i]), TimeSpan.FromMinutes(delayMinutes));
                }
            }
        }

        public async Task UpdateStatistics(Statistics statisticsInArabic, Statistics statisticsInEnglish)
        {
            _unitOfWork.MatchStatistic.CreateStatisticCategory(new StatisticCategory
            {
                Name = statisticsInArabic.CategoryName,
                _365_Id = statisticsInArabic.CategoryId.ToString(),
                StatisticCategoryLang = new StatisticCategoryLang
                {
                    Name = statisticsInEnglish.CategoryName
                }
            });
            await _unitOfWork.Save();

            int fk_StatisticCategory = _unitOfWork.MatchStatistic.GetStatisticCategorys(new StatisticCategoryParameters
            {
                _365_Id = statisticsInArabic.CategoryId.ToString()
            }).Select(a => a.Id).First();

            _unitOfWork.MatchStatistic.CreateStatisticScore(new StatisticScore
            {
                Fk_StatisticCategory = fk_StatisticCategory,
                Name = statisticsInArabic.Name,
                _365_Id = statisticsInArabic.Id.ToString(),
                StatisticScoreLang = new StatisticScoreLang { Name = statisticsInEnglish.Name }
            });
            await _unitOfWork.Save();
        }

    }
}
