using Entities.CoreServicesModels.SeasonModels;
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

        public void UpdateStates()
        {
            List<string> teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
            }, otherLang: false).Select(a => a._365_MatchId).ToList();

            int minutes = 30;
            foreach (string _365_MatchId in teamGameWeaks)
            {
                _ = BackgroundJob.Schedule(() => UpdateStates(_365_MatchId.ParseToInt()), TimeSpan.FromMinutes(minutes));
                minutes++;
            }
        }

        public async Task UpdateStates(int _365_MatchId)
        {
            GameReturn gameReturnInArabic = await _365Services.GetGame(new _365GameParameters
            {
                GameId = _365_MatchId,
                IsArabic = true
            });

            GameReturn gameReturnInEnglish = await _365Services.GetGame(new _365GameParameters
            {
                GameId = _365_MatchId,
            });

            UpdateStats(gameReturnInArabic.Game, gameReturnInEnglish.Game);
            UpdateEvents(gameReturnInArabic.Game, gameReturnInEnglish.Game);
        }

        public void UpdateStats(Game gameInArabic, Game gameInEnglish)
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

            int minutes = 30;
            for (int i = 0; i < statsInArabic.Count; i++)
            {
                _ = BackgroundJob.Schedule(() => UpdateStat(statsInArabic[i], statsInEnglish[i]), TimeSpan.FromMinutes(minutes));
                minutes++;
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

        public void UpdateEvents(Game gameInArabic, Game gameInEnglish)
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

            int minutes = 30;
            for (int i = 0; i < eventsInArabic.Count; i++)
            {
                _ = BackgroundJob.Schedule(() => UpdateEvent(eventsInArabic[i], eventsInEnglish[i]), TimeSpan.FromMinutes(minutes));
                minutes++;
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
    }
}
