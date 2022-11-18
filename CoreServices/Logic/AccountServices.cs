using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.SubscriptionModels;
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
                                  Address = a.Address,
                                  Fk_Country = a.Fk_Country,
                                  Fk_FavouriteTeam = a.Fk_FavouriteTeam,
                                  Fk_Nationality = a.Fk_Nationality,
                                  PhoneNumberTwo = a.PhoneNumberTwo,
                                  RefCode = a.RefCode,
                                  RefCodeCount = a.RefCodeCount,
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
                                      Name = otherLang ? a.FavouriteTeam.TeamLang.Name : a.FavouriteTeam.Name,
                                      ShortName = otherLang ? a.FavouriteTeam.TeamLang.ShortName : a.FavouriteTeam.ShortName,
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
            foreach (AccountSubscriptionModel subscription in subscriptions)
            {
                AccountSubscription accountSubscription = new()
                {
                    Fk_Account = fk_account,
                    Fk_Subscription = subscription.Fk_Subscription,
                    IsAction = subscription.IsAction
                };

                _repository.AccountSubscription.Create(accountSubscription);
            }
        }

        private void RemoveAccountSubscriptions(int fk_account)
        {
            IQueryable<AccountSubscription> subscriptions = _repository.AccountSubscription.FindAll(new AccountSubscriptionParameters
            {
                Fk_Account = fk_account
            }, trackChanges: false);

            if (!subscriptions.Any())
            {
                return;
            }

            foreach (AccountSubscription subscription in subscriptions)
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

        #region Account RefCode Services

        public IQueryable<AccountRefCodeModel> GetAccountRefCodes(
            AccountRefCodeParameters parameters)
        {
            return _repository.AccountRefCode
                              .FindAll(parameters, trackChanges: false)
                              .Select(a => new AccountRefCodeModel
                              {
                                  Id = a.Id,
                                  CreatedAt = a.CreatedAt,
                                  Fk_NewAccount = a.Fk_NewAccount,
                                  Fk_RefAccount = a.Fk_RefAccount
                              })
                              .Search(parameters.SearchColumns, parameters.SearchTerm)
                              .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<AccountRefCodeModel>> GetAccountRefCodesPaged(
            AccountRefCodeParameters parameters)
        {
            return await PagedList<AccountRefCodeModel>.ToPagedList(GetAccountRefCodes(parameters), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AccountRefCode> FindAccountRefCodeById(int id, bool trackChanges)
        {
            return await _repository.AccountRefCode.FindById(id, trackChanges);
        }

        public AccountRefCodeModel GetAccountRefCodebyId(int id)
        {
            return GetAccountRefCodes(new AccountRefCodeParameters { Id = id }).SingleOrDefault();
        }

        public void CreateAccountRefCode(AccountRefCode account)
        {
            _repository.AccountRefCode.Create(account);
        }

        public async Task CreateAccountRefCode(string refCode, int fk_NewAccount)
        {
            if (refCode.IsExisting() && fk_NewAccount > 0)
            {
                var refAccount = GetAccounts(new AccountParameters
                {
                    RefCode = refCode
                }, otherLang: false).Select(a => new
                {
                    a.RefCode,
                    a.RefCodeCount,
                    a.Id
                }).FirstOrDefault();

                if (refAccount != null)
                {
                    CreateAccountRefCode(new AccountRefCode
                    {
                        Fk_NewAccount = fk_NewAccount,
                        Fk_RefAccount = refAccount.Id
                    });

                    Account account = await FindAccountById(refAccount.Id, trackChanges: true);
                    account.RefCodeCount++;

                    if (account.RefCodeCount == 20)
                    {
                        int subscription = _repository.Subscription
                                              .FindAll(new SubscriptionParameters
                                              {
                                                  ForAction = true
                                              }, trackChanges: false)
                                              .Select(a => a.Id)
                                              .FirstOrDefault();

                        if (subscription > 0)
                        {
                            var season = _repository.Season.FindAll(new SeasonParameters
                            {
                                IsCurrent = true
                            }, trackChanges: false).First();

                            if (subscription > 0)
                            {
                                CreateAccountSubscription(new AccountSubscription
                                {
                                    Fk_Account = refAccount.Id,
                                    Fk_Subscription = subscription,
                                    IsAction = true,
                                    Fk_Season = season.Id,
                                    IsActive = true
                                });

                                account.RefCodeCount = 0;
                            }
                        }
                    }
                }
            }
        }

        public int GetAccountRefCodesCount()
        {
            return _repository.AccountRefCode.Count();
        }

        #endregion

        #region Account Subscription Services

        public IQueryable<AccountSubscriptionModel> GetAccountSubscriptions(
            AccountSubscriptionParameters parameters,
            bool otherLang)
        {
            return _repository.AccountSubscription
                              .FindAll(parameters, trackChanges: false)
                              .Select(a => new AccountSubscriptionModel
                              {
                                  Id = a.Id,
                                  CreatedAt = a.CreatedAt,
                                  Fk_Season = a.Fk_Season,
                                  Season = new SeasonModel
                                  {
                                      Name = otherLang ? a.Season.SeasonLang.Name : a.Season.Name
                                  },
                                  IsAction = a.IsAction,
                                  Fk_Account = a.Fk_Account,
                                  Fk_Subscription = a.Fk_Subscription,
                                  Cost = a.Cost,
                                  IsActive = a.IsActive,
                                  Order_id = a.Order_id,
                                  Subscription = new SubscriptionModel
                                  {
                                      Id = a.Fk_Subscription,
                                      Cost = a.Subscription.Cost,
                                      Description = otherLang ? a.Subscription.SubscriptionLang.Description : a.Subscription.Description,
                                      Discount = a.Subscription.Discount,
                                      ForAction = a.Subscription.ForAction,
                                      ImageUrl = a.Subscription.StorageUrl + a.Subscription.ImageUrl,
                                      Name = otherLang ? a.Subscription.SubscriptionLang.Name : a.Subscription.Name,
                                      IsActive = a.Subscription.IsActive,
                                  }
                              })
                              .Search(parameters.SearchColumns, parameters.SearchTerm)
                              .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<AccountSubscriptionModel>> GetAccountSubscriptionsPaged(
            AccountSubscriptionParameters parameters,
            bool otherLang)
        {
            return await PagedList<AccountSubscriptionModel>.ToPagedList(GetAccountSubscriptions(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AccountSubscription> FindAccountSubscriptionById(int id, bool trackChanges)
        {
            return await _repository.AccountSubscription.FindById(id, trackChanges);
        }

        public AccountSubscriptionModel GetAccountSubscriptionbyId(int id, bool otherLang)
        {
            return GetAccountSubscriptions(new AccountSubscriptionParameters { Id = id }, otherLang).SingleOrDefault();
        }


        public void CreateAccountSubscription(AccountSubscription account)
        {
            _repository.AccountSubscription.Create(account);
        }

        public int GetAccountSubscriptionsCount()
        {
            return _repository.AccountSubscription.Count();
        }

        #endregion

        #region Payment Services

        public IQueryable<PaymentModel> GetPayments(
            PaymentParameters parameters,
            bool otherLang)
        {
            return _repository.Payment
                              .FindAll(parameters, trackChanges: false)
                              .Select(a => new PaymentModel
                              {
                                  Id = a.Id,
                                  CreatedAt = a.CreatedAt,
                                  TransactionId = a.TransactionId,
                                  Fk_Account = a.Fk_Account,
                                  Amount = a.Amount
                              })
                              .Search(parameters.SearchColumns, parameters.SearchTerm)
                              .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<PaymentModel>> GetPaymentsPaged(
            PaymentParameters parameters,
            bool otherLang)
        {
            return await PagedList<PaymentModel>.ToPagedList(GetPayments(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Payment> FindPaymentById(int id, bool trackChanges)
        {
            return await _repository.Payment.FindById(id, trackChanges);
        }

        public PaymentModel GetPaymentbyId(int id, bool otherLang)
        {
            return GetPayments(new PaymentParameters { Id = id }, otherLang).SingleOrDefault();
        }


        public void CreatePayment(Payment payment)
        {
            _repository.Payment.Create(payment);
        }

        public int GetPaymentsCount()
        {
            return _repository.Payment.Count();
        }

        #endregion
    }
}
