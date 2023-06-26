using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.DBModels.SeasonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DBModels.MatchStatisticModels
{
    public class MatchStatisticScoreRepository : RepositoryBase<MatchStatisticScore>
    {
        public MatchStatisticScoreRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<MatchStatisticScore> FindAll(MatchStatisticScoreParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_TeamGameWeak,
                           parameters.Fk_Team,
                           parameters.Fk_Teams,
                           parameters.Fk_StatisticScores,
                           parameters.Fk_GameWeak,
                           parameters.Fk_Season,
                           parameters.IsCanNotEdit,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo
                         );
        }

        public async Task<MatchStatisticScore> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }


        public new void Create(MatchStatisticScore entity)
        {
            if (FindByCondition(a => a.Fk_TeamGameWeak == entity.Fk_TeamGameWeak &&
                                     a.Fk_Team == entity.Fk_Team &&
                                     a.Fk_StatisticScore == entity.Fk_StatisticScore, trackChanges: false).Any())
            {
                MatchStatisticScore oldEntity = FindByCondition(a => a.Fk_TeamGameWeak == entity.Fk_TeamGameWeak &&
                                     a.Fk_Team == entity.Fk_Team &&
                                     a.Fk_StatisticScore == entity.Fk_StatisticScore, trackChanges: true)
                                .First();

                if (!oldEntity.IsCanNotEdit)
                {
                    oldEntity.ValuePercentage = entity.ValuePercentage;
                    oldEntity.Value = entity.Value;
                }
            }
            else
            {
                base.Create(entity);
            }
        }
    }

    public static class MatchStatisticScoreRepositoryExtension
    {
        public static IQueryable<MatchStatisticScore> Filter(
            this IQueryable<MatchStatisticScore> data,
            int id,
            int fk_TeamGameWeak,
            int fk_Team,
            List<int> fk_Teams,
            List<int> fk_StatisticScores,
            int fk_GameWeak,
            int fk_Season,
            bool? isCanNotEdit,
            DateTime? createdAtFrom,
            DateTime? createdAtTo)
        {
            return data.Where(a => (id == 0 || a.Id == id) &&
                                   (fk_TeamGameWeak == 0 || a.Fk_TeamGameWeak == fk_TeamGameWeak) &&
                                   (fk_Team == 0 || a.Fk_Team == fk_Team) &&
                                   (fk_Teams == null || !fk_Teams.Any() || fk_Teams.Contains(a.Fk_Team)) &&
                                   (fk_Season == 0 || a.TeamGameWeak.GameWeak.Fk_Season == fk_Season) &&
                                   (fk_GameWeak == 0 || a.TeamGameWeak.Fk_GameWeak == fk_GameWeak) &&
                                   (isCanNotEdit == null || a.IsCanNotEdit == isCanNotEdit) &&
                                   (fk_StatisticScores == null || !fk_StatisticScores.Any() || fk_StatisticScores.Contains(a.Fk_StatisticScore)) &&
                                   (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&
                                   (createdAtTo == null || a.CreatedAt <= createdAtTo));

        }

    }
}
