using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountModels;

namespace CoreServices.Logic
{
    public class AccountServices
    {
        private readonly RepositoryManager _repository;

        public AccountServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Account Services

        public IQueryable<AccountModel> GetAccounts(
            AccountParameters parameters,
            bool otherLang)
        {
            return _repository.Account
                              .FindAll(parameters, trackChanges: false)
                              .Select(a => new AccountModel
                              {
                                  Id = a.Id,
                                  CreatedAt = a.CreatedAt,
                                  CreatedBy = a.CreatedBy,
                                  LastModifiedAt = a.LastModifiedAt,
                                  LastModifiedBy = a.LastModifiedBy,
                                  ImageUrl = a.StorageUrl + a.ImageUrl,
                                  FullName = a.FullName,
                                  EmailAddress = a.User.EmailAddress,
                                  PhoneNumber = a.User.PhoneNumber,
                                  UserName = a.User.UserName,
                                  Country = new CountryModel
                                  {
                                      Name = otherLang ? a.Country.CountryLang.Name : a.Country.Name
                                  },
                                  Nationality = new CountryModel
                                  {
                                      Name = otherLang ? a.Nationality.CountryLang.Name : a.Nationality.Name
                                  },
                                  LastActive = a.User.RefreshTokens.Any()
                                  ? a.User.RefreshTokens.OrderByDescending(b => b.Id).Select(a => a.CreatedAt).FirstOrDefault()
                                  : null,
                                  FavouriteTeam = new TeamModel
                                  {
                                      Name = a.FavouriteTeam.Name
                                  }
                              })
                              .Search(parameters.SearchColumns, parameters.SearchTerm)
                              .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<AccountModel>> GetAccountsPaged(
            AccountParameters parameters,
            bool otherLang)
        {
            return await PagedList<AccountModel>.ToPagedList(GetAccounts(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }
        
        public void UpdateAccountSubscriptions(int fk_account, List<AccountSubscriptionModel> subscriptions)
        {
            RemoveAccountSubscriptions(fk_account);
            AddAccountSubscriptions(fk_account, subscriptions);
        }

        private void AddAccountSubscriptions(int fk_account, List<AccountSubscriptionModel> subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                var accountSubscription = new AccountSubscription
                {
                    Fk_Account = fk_account,
                    Fk_Subscription = subscription.Fk_Subscription,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    IsAction = subscription.IsAction
                };

                _repository.AccountSubscription.Create(accountSubscription);
            }
        }

        private void RemoveAccountSubscriptions(int fk_account)
        {
            var subscriptions = _repository.AccountSubscription.FindAll(new AccountSubscriptionParameters
            {
                Fk_Account = fk_account
            }, trackChanges: false);

            if (!subscriptions.Any()) return;

            foreach (var subscription in subscriptions)
            {
                _repository.AccountSubscription.Delete(subscription);
            }
        }
        
        public async Task<PagedList<AccountModel>> GetAccountsPaged(
          IQueryable<AccountModel> data,
         AccountParameters parameters)
        {
            return await PagedList<AccountModel>.ToPagedList(data, parameters.PageNumber, parameters.PageSize);
        }
        
        public async Task<Account> FindAccountById(int id, bool trackChanges)
        {
            return await _repository.Account.FindById(id, trackChanges);
        }

        public AccountModel GetAccountbyId(int id, bool otherLang)
        {
            return GetAccounts(new AccountParameters { Id = id }, otherLang).SingleOrDefault();
        }


        public async Task<Account> FindByUserId(int fK_User, bool trackChanges)
        {
            return await _repository.Account.FindByUserId(fK_User, trackChanges);
        }

        public void CreateAccount(Account account)
        {
            _repository.Account.Create(account);
        }

        public async Task<string> UploudAccountImage(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/Account");
        }

        public void RemoveAccountImage(string rootPath, string filePath)
        {
            FileUploader uploader = new(rootPath);
            uploader.DeleteFile(filePath);
        }

        public int GetIdByUserId(int fK_User)
        {
            return _repository.Account.GetIdByUserId(fK_User);
        }

        public int GetAccountsCount()
        {
            return _repository.Account.Count();
        }

        public async Task DeleteAccount(int id)
        {
            Account account = await _repository.Account.FindById(id, trackChanges: false);

            User user = await _repository.User.FindByAccountId(id, trackChanges: false);

            _repository.Account.Delete(account);

            _repository.User.Delete(user);

        }
        #endregion
    }
}
