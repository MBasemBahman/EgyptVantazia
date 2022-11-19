using Entities.CoreServicesModels.LogModels;
using Entities.DBModels.LogModels;

namespace Repository.DBModels.LogModels
{
    public class LogRepository : RepositoryBase<Log>
    {
        public LogRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Log> FindAll(
          LogParameters parameters,
          bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo);

        }

        public async Task<Log> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }
    }

    public static class RefreshTokenRepositoryExtensions
    {
        public static IQueryable<Log> Filter(
            this IQueryable<Log> logs,
            int id,
            DateTime? createdAtFrom,
            DateTime? createdAtTo)
        {

            return logs.Where(a => (id == 0 || a.Id == id) &&
                                   (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&
                                   (createdAtTo == null || createdAtTo == createdAtFrom || a.CreatedAt <= createdAtTo));
        }


    }
}
