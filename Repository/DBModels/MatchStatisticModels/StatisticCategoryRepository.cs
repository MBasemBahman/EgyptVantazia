using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.DBModels.PlayerScoreModels;
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
                   .Filter(parameters.Id,
                           parameters._365_Id);
        }

        public async Task<StatisticCategory> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.StatisticCategoryLang)
                        .FirstOrDefaultAsync();
        }
        public new void Create(StatisticCategory entity)
        {
            if (FindByCondition(a => a._365_Id == entity._365_Id, trackChanges: false).Any())
            {
                StatisticCategory oldEntity = FindByCondition(a => a._365_Id == entity._365_Id, trackChanges: true)
                                .Include(a => a.StatisticCategoryLang)
                                .First();

                oldEntity.Name = entity.Name;
                oldEntity.StatisticCategoryLang.Name = entity.StatisticCategoryLang.Name;
            }
            else
            {
                entity.StatisticCategoryLang ??= new StatisticCategoryLang
                {
                    Name = entity.Name,
                };
                base.Create(entity);
            }
        }
    }

    public static class StatisticCategoryRepositoryExtension
    {
        public static IQueryable<StatisticCategory> Filter(
            this IQueryable<StatisticCategory> data,
            int id,
            string _365_Id)
        {
            return data.Where(a => (id == 0 || a.Id == id) &&
                                   (string.IsNullOrEmpty(_365_Id) || a._365_Id == _365_Id));

        }

    }
}
