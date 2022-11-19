﻿using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.PrivateLeagueModels;
using Services;

namespace Repository.DBModels.PrivateLeagueModels
{
    public class PrivateLeagueRepository : RepositoryBase<PrivateLeague>
    {
        public PrivateLeagueRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PrivateLeague> FindAll(PrivateLeagueParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Account,
                           parameters.HaveMembers,
                           parameters.UniqueCode,
                           parameters.IsAdmin,
                           parameters.Fk_Season);
        }

        public async Task<PrivateLeague> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PrivateLeague entity)
        {
            do
            {
                entity.UniqueCode = RandomGenerator.GenerateString(2) + RandomGenerator.GenerateInteger(2, 00, 99).ToString() + RandomGenerator.GenerateString(2);
            } while (FindByCondition(a => a.UniqueCode == entity.UniqueCode, trackChanges: false).Any());

            base.Create(entity);
        }
    }

    public static class PrivateLeagueRepositoryExtension
    {
        public static IQueryable<PrivateLeague> Filter(
            this IQueryable<PrivateLeague> PrivateLeagues,
            int id,
            int Fk_Account,
            bool? haveMembers,
            string UniqueCode,
            bool? IsAdmin,
            int Fk_Season)
        {
            return PrivateLeagues.Where(a => (id == 0 || a.Id == id) &&
                                             (Fk_Season == 0 || a.PrivateLeagueMembers.Any(b => b.Account.AccountTeams.Any(c => c.Fk_Season == Fk_Season))) &&
                                             (haveMembers == null || (haveMembers == true ? a.PrivateLeagueMembers.Any() : !a.PrivateLeagueMembers.Any())) &&
                                             (Fk_Account == 0 || a.PrivateLeagueMembers.Any(b => b.Fk_Account == Fk_Account)) &&
                                             (IsAdmin == null || a.PrivateLeagueMembers.Any(b => b.IsAdmin == IsAdmin)) &&
                                             (string.IsNullOrEmpty(UniqueCode) || a.UniqueCode == UniqueCode));
        }

    }
}
