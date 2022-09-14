﻿using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.PrivateLeagueModels;
using Entities.RequestFeatures;


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
                           parameters.Fk_PrivateLeague);
        }

        public async Task<PrivateLeagueMember> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PrivateLeagueMember entity)
        {
            base.Create(entity);
        }

        public new void Delete(PrivateLeagueMember entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class PrivateLeagueMemberRepositoryExtension
    {
        public static IQueryable<PrivateLeagueMember> Filter(
            this IQueryable<PrivateLeagueMember> PrivateLeagueMembers, 
            int id,
            int Fk_Account,
            int Fk_PrivateLeague
            )
        {
            return PrivateLeagueMembers.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                                   (Fk_PrivateLeague == 0 || a.Fk_PrivateLeague == Fk_PrivateLeague));

        }

    }
}
