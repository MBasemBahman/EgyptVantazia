﻿using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;


namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamRepository : RepositoryBase<AccountTeam>
    {
        public AccountTeamRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeam> FindAll(AccountTeamParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Account,
                           parameters.Fk_PrivateLeague,
                           parameters.Fk_Season,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters.AccountFullName,
                           parameters.AccountUserName);
        }

        public async Task<AccountTeam> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }
    }

    public static class AccountTeamRepositoryExtension
    {
        public static IQueryable<AccountTeam> Filter(
            this IQueryable<AccountTeam> AccountTeams,
            int id,
            int Fk_Account,
            int Fk_PrivateLeague,
            int Fk_Season,
            DateTime? CreatedAtFrom,
            DateTime? CreatedAtTo,
            string AccountFullName,
            string AccountUserName)

        {
            return AccountTeams.Where(a => (id == 0 || a.Id == id) &&
                                           (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                           (Fk_PrivateLeague == 0 || a.Account.PrivateLeagueMembers.Any(b => b.Fk_PrivateLeague == Fk_PrivateLeague)) &&
                                           (CreatedAtFrom == null || a.CreatedAt >= CreatedAtFrom) &&
                                           (CreatedAtTo == null || a.CreatedAt <= CreatedAtTo) &&
                                           (string.IsNullOrWhiteSpace(AccountUserName) || a.Account.User.UserName.ToLower().Contains(AccountUserName)) &&
                                           (string.IsNullOrWhiteSpace(AccountFullName) || a.Account.FullName.ToLower().Contains(AccountFullName)) &&
                                           (Fk_Season == 0 || a.Fk_Season == Fk_Season));
        }

    }
}
