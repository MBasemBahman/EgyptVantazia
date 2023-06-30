using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using static Contracts.EnumData.DBModelsEnum;

namespace Repository.DBModels.SeasonModels
{
    public class GameWeakRepository : RepositoryBase<GameWeak>
    {
        public GameWeakRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<GameWeak> FindAll(GameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Season,
                           parameters._365_GameWeakId,
                           parameters.IsCurrent,
                           parameters.IsCurrentSeason,
                           parameters._365_CompetitionsId,
                           parameters.BiggerThanWeak,
                           parameters.LowerThanWeak,
                           parameters.Deadline,
                           parameters.DeadlineFrom,
                           parameters.DeadlineTo,
                           parameters.IsNext,
                           parameters.IsPrev,
                           parameters.GameWeakFrom,
                           parameters.GameWeakTo);
        }

        public async Task<GameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.GameWeakLang)
                        .FirstOrDefaultAsync();
        }

        public async Task<GameWeak> FindBy365Id(string id, int fk_Season, bool trackChanges)
        {
            return await FindByCondition(a => a.Fk_Season == fk_Season && a._365_GameWeakId == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public void ResetCurrent(int _365CompetitionsEnum)
        {
            List<GameWeak> gameWeaks = FindByCondition(a => a.Season._365_CompetitionsId == _365CompetitionsEnum.ToString() &&
                                                            (a.IsCurrent ||
                                                             a.IsPrev ||
                                                             a.IsNext), trackChanges: true).ToList();
            gameWeaks.ForEach(a =>
            {
                a.IsCurrent = false;
                a.IsPrev = false;
                a.IsNext = false;
            });
        }

        public new void Create(GameWeak entity)
        {
            if (entity._365_GameWeakId.IsExisting() && FindByCondition(a => a.Fk_Season == entity.Fk_Season && a._365_GameWeakId == entity._365_GameWeakId, trackChanges: false).Any())
            {
                GameWeak oldEntity = FindByCondition(a => a.Fk_Season == entity.Fk_Season && a._365_GameWeakId == entity._365_GameWeakId, trackChanges: true)
                                .Include(a => a.GameWeakLang)
                                .First();

                oldEntity.Name = entity.Name;
                oldEntity._365_GameWeakId = entity._365_GameWeakId;
                oldEntity.GameWeakLang.Name = entity.GameWeakLang.Name;
            }
            else
            {
                entity.GameWeakLang ??= new GameWeakLang
                {
                    Name = entity.Name,
                };
                base.Create(entity);
            }
        }

        public GameWeak GetGameWeak(DateTime matchStartMatch)
        {
            return FindByCondition(a => a.Deadline != null &&
                                        a.Deadline.Value.Date <= matchStartMatch.Date, trackChanges: false)
                   .OrderByDescending(a => a._365_GameWeakIdValue)
                   .FirstOrDefault();
        }

        public bool CheckIfGameWeakEnded(int fk_GameWeak)
        {
            if (!DBContext.TeamGameWeaks.Any(a => a.Fk_GameWeak == fk_GameWeak))
            {
                return false;
            }
            return DBContext.TeamGameWeaks
                            .Where(a => a.Fk_GameWeak == fk_GameWeak && a.IsActive)
                            .OrderByDescending(a => a.StartTime)
                            .Select(a => a.IsEnded)
                            .FirstOrDefault() ||
                  DBContext.GameWeaks
                           .Any(a => a.Id == fk_GameWeak && a.IsPrev);
        }
    }

    public static class GameWeakRepositoryExtension
    {
        public static IQueryable<GameWeak> Filter(
            this IQueryable<GameWeak> GameWeaks,
            int id,
            int Fk_Season,
            string _365_GameWeakId,
            bool? isCurrent,
            bool? isCurrentSeason,
            int _365_CompetitionsId,
            int? biggerThanWeak,
            int? lowerThanWeak,
            DateTime? deadline,
            DateTime? deadlineFrom,
            DateTime? deadlineTo,
            bool? isNext,
            bool? isPrev,
            int GameWeakFrom,
            int GameWeakTo)
        {
            return GameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                        (deadline == null || a.Deadline == deadline) &&
                                        (deadlineFrom == null || a.Deadline >= deadlineFrom) &&
                                        (GameWeakFrom == 0 || a._365_GameWeakIdValue >= GameWeakFrom) &&
                                        (GameWeakTo == 0 || a._365_GameWeakIdValue <= GameWeakTo) &&
                                        (deadlineTo == null || a.Deadline <= deadlineTo) &&
                                        (Fk_Season == 0 || a.Fk_Season == Fk_Season) &&
                                        (biggerThanWeak == null || (!string.IsNullOrEmpty(a._365_GameWeakId) && Convert.ToInt32(a._365_GameWeakId) > biggerThanWeak.Value)) &&
                                        (lowerThanWeak == null || (!string.IsNullOrEmpty(a._365_GameWeakId) && Convert.ToInt32(a._365_GameWeakId) < lowerThanWeak.Value)) &&
                                        (isCurrent == null || a.IsCurrent == isCurrent) &&
                                        (isNext == null || a.IsNext == isNext) &&
                                        (isPrev == null || a.IsPrev == isPrev) &&
                                        (isCurrentSeason == null || a.Season.IsCurrent == isCurrentSeason) &&
                                        (_365_CompetitionsId == 0 || a.Season._365_CompetitionsId == _365_CompetitionsId.ToString()) &&

                                        (string.IsNullOrWhiteSpace(_365_GameWeakId) || a._365_GameWeakId == _365_GameWeakId));

        }

    }
}
