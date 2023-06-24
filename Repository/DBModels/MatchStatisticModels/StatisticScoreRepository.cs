using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.DBModels.PlayerStateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DBModels.MatchStatisticModels
{
    public class StatisticScoreRepository : RepositoryBase<StatisticScore>
    {
        public StatisticScoreRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<StatisticScore> FindAll(StatisticScoreParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,parameters.Fk_StatisticCategory);
        }

        public async Task<StatisticScore> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.StatisticScoreLang)
                        .FirstOrDefaultAsync();
        }
        public new void Create(StatisticScore entity)
        {
            entity.StatisticScoreLang ??= new StatisticScoreLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }

    }

    public static class StatisticScoreRepositoryExtension
    {
        public static IQueryable<StatisticScore> Filter(
            this IQueryable<StatisticScore> data,
            int id, int fk_StatisticCategory)
        {
            return data.Where(a => (id == 0 || a.Id == id) &&
                                   (fk_StatisticCategory == 0 || a.Fk_StatisticCategory == fk_StatisticCategory));

        }

    }
}
