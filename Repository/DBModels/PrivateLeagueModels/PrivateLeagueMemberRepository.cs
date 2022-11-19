﻿using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.PrivateLeagueModels;


namespace Repository.DBModels.PrivateLeagueModels
{
    public class PrivateLeagueMemberRepository : RepositoryBase<PrivateLeagueMember>
    {
        public PrivateLeagueMemberRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PrivateLeagueMember> FindAll(PrivateLeagueMemberParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Account,
                           parameters.Fk_PrivateLeague,
                           parameters.IsAdmin,
                           parameters.Fk_Season);
        }

        public async Task<PrivateLeagueMember> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(PrivateLeagueMember entity)
        {
            if (FindByCondition(a => a.Fk_PrivateLeague == entity.Fk_PrivateLeague && a.Fk_Account == entity.Fk_Account, trackChanges: false).Any())
            {
                PrivateLeagueMember oldEntity = FindByCondition(a => a.Fk_PrivateLeague == entity.Fk_PrivateLeague && a.Fk_Account == entity.Fk_Account, trackChanges: true).First();

                oldEntity.Ranking = entity.Ranking;
            }
            else
            {
                base.Create(entity);
            }
        }
    }

    public static class PrivateLeagueMemberRepositoryExtension
    {
        public static IQueryable<PrivateLeagueMember> Filter(
            this IQueryable<PrivateLeagueMember> PrivateLeagueMembers,
            int id,
            int Fk_Account,
            int Fk_PrivateLeague,
            bool? IsAdmin,
            int Fk_Season)
        {
            return PrivateLeagueMembers.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                                   (Fk_Season == 0 || a.Account.AccountTeams.Any(b => b.Fk_Season == Fk_Season)) &&
                                                   (IsAdmin == null || a.IsAdmin == IsAdmin) &&
                                                   (Fk_PrivateLeague == 0 || a.Fk_PrivateLeague == Fk_PrivateLeague));

        }

    }
}
