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
    public class StatisticCategoryRepository : RepositoryBase<StatisticCategory>
    {
        public StatisticCategoryRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<StatisticCategory> FindAll(StatisticCategoryParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<StatisticCategory> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.StatisticCategoryLang)
                        .FirstOrDefaultAsync();
        }
        public new void Create(StatisticCategory entity)
        {
            entity.StatisticCategoryLang ??= new StatisticCategoryLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class StatisticCategoryRepositoryExtension
    {
        public static IQueryable<StatisticCategory> Filter(
            this IQueryable<StatisticCategory> data,
            int id)
        {
            return data.Where(a => (id == 0 || a.Id == id));

        }

    }
}
