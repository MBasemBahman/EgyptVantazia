using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.LocationModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.AccountModels
{
    public class AccountRepository : RepositoryBase<Account>
    {
        public AccountRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Account> FindAll(AccountParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Country,
                           parameters.Fk_Nationality,
                           parameters.Fk_FavouriteTeam,
                           parameters.Fk_User);
        }

        public async Task<Account> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(Account entity)
        {
            base.Create(entity);
        }

        public new void Delete(Account entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class AccountRepositoryExtension
    {
        public static IQueryable<Account> Filter(
            this IQueryable<Account> Accounts,
            int id,
            int Fk_User,
            int Fk_Country,
            int Fk_Nationality,
            int Fk_FavouriteTeam
 )
        {
            return Accounts.Where(a => (id == 0 || a.Id == id) &&
                                       (Fk_User == 0 || a.Fk_User == Fk_User) &&
                                       (Fk_Country == 0 || a.Fk_Country == Fk_Country) &&
                                       (Fk_Nationality == 0 || a.Fk_Nationality == Fk_Nationality) &&
                                       (Fk_FavouriteTeam == 0 || a.Fk_FavouriteTeam == Fk_FavouriteTeam));
        }

    }
}
