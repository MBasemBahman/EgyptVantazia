﻿using Contracts.EnumData;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.SubscriptionModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.CoreServicesModels.UserModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.SubscriptionModels;
using static Contracts.EnumData.DBModelsEnum;

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
                                  Fk_Nationality = a.Fk_Nationality,
                                  Fk_Season = a.Fk_Season,
                                  Season = new SeasonModel
                                  {
                                      Name = otherLang ? a.Season.SeasonLang.Name : a.Season.Name,
                                      _365_CompetitionsId = a.Season._365_CompetitionsId,
                                      _365_SeasonId = a.Season._365_SeasonId,
                                      ImageUrl = a.Season.StorageUrl + a.Season.ImageUrl,
                                  },
                                  PhoneNumberTwo = a.PhoneNumberTwo,
                                  RemoveAds = a.ShowAds == false ||
                                              a.AccountSubscriptions.Any(b => (b.Fk_Subscription == (int)SubscriptionEnum.RemoveAds ||
                                                                                b.Fk_Subscription == (int)SubscriptionEnum.Gold) &&
                                                                                b.IsActive &&
                                                                                b.Fk_Season == a.Fk_Season),
                                  Fk_AccountTeam = a.AccountTeams
                                                    .Where(b => b.Fk_Season == a.Fk_Season)
                                                    .Select(b => b.Id)
                                                    .FirstOrDefault(),
                                  AccountTeam = a.AccountTeams
                                                  .Where(b => b.Fk_Season == a.Fk_Season)
                                                  .Select(b => new AccountTeamModel
                                                  {
                                                      Id = b.Id,
                                                      Name = b.Name,
                                                      Season = new SeasonModel
                                                      {
                                                          _365_CompetitionsId = b.Season._365_CompetitionsId,
                                                          _365_SeasonId = b.Season._365_SeasonId,
                                                          Name = otherLang ? b.Season.SeasonLang.Name : b.Season.Name,
                                                      },
                                                      Fk_FavouriteTeam = b.Fk_FavouriteTeam,
                                                      FavouriteTeam = b.Fk_FavouriteTeam > 0 ? new TeamModel
                                                      {
                                                          Id = b.FavouriteTeam.Id,
                                                          Name = otherLang ? b.FavouriteTeam.TeamLang.Name : b.FavouriteTeam.Name
                                                      } : null
                                                  })
                                                  .FirstOrDefault(),
                                  Country = new CountryModel
                                  {
                                      Id = a.Fk_Country,
                                      Name = otherLang ? a.Country.CountryLang.Name : a.Country.Name
                                  },
                                  Nationality = new CountryModel
                                  {
                                      Id = a.Fk_Nationality,
                                      Name = otherLang ? a.Nationality.CountryLang.Name : a.Nationality.Name
                                  },
                                  LastActive = a.User.RefreshTokens.Any()
                                  ? a.User.RefreshTokens.OrderByDescending(b => b.Id).Select(a => a.CreatedAt).FirstOrDefault()
                                  : null,
                                  AccountSubscriptionsCount = a.AccountSubscriptions.Count(b => b.IsActive),
                                  GoldSubscriptionsCount = a.AccountSubscriptions.Count(b => b.IsActive && b.Fk_Subscription == (int)DBModelsEnum.SubscriptionEnum.Gold)
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
            return GetAccounts(new AccountParameters { Id = id }, otherLang).FirstOrDefault();
        }


        public async Task<Account> FindByUserId(int fK_User, bool trackChanges)
        {
            return await _repository.Account.FindByUserId(fK_User, trackChanges);
        }

        public Dictionary<string, string> GetAccountLookUp(AccountParameters parameters, bool otherLang)
        {
            return GetAccounts(parameters, otherLang)
                .OrderByDescending(a => a.Id).ToDictionary(a => a.Id.ToString(), a => a.FullName);
        }

        public async Task<AccountModel> GetByUserId(int fK_User, bool otherLang)
        {
            return await GetAccounts(new AccountParameters
            {
                Fk_User = fK_User
            }, otherLang).FirstOrDefaultAsync();
        }

        public IQueryable<Account> GetAccountByCondition(Expression<Func<Account, bool>> expression)
        {
            return _repository.Account.FindByCondition(expression, trackChanges: false);
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

        public async Task DeleteAccount(List<int> ids)
        {
            await _repository.User.FindAll(new UserParameters
            {
                Fk_Accounts = ids
            }, trackChanges: false).ExecuteDeleteAsync();

            await _repository.Account.FindAll(new AccountParameters
            {
                Fk_Accounts = ids
            }, trackChanges: false).ExecuteDeleteAsync();
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
                                  Account = new AccountModel
                                  {
                                      Name = a.Account.FullName,
                                      PhoneNumber = a.Account.User.PhoneNumber,
                                      ImageUrl = a.Account.StorageUrl + a.Account.ImageUrl,
                                      FullName = a.Account.FullName,
                                      Fk_Country = a.Account.Fk_Country,
                                      Fk_Nationality = a.Account.Fk_Nationality,
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
            return GetAccountSubscriptions(new AccountSubscriptionParameters { Id = id }, otherLang).FirstOrDefault();
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
                                  Amount = a.Amount,
                                  PaymentProvider = a.PaymentProvider,
                                  Account = new AccountModel
                                  {
                                      Name = a.Account.FullName
                                  }
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
            return GetPayments(new PaymentParameters { Id = id }, otherLang).FirstOrDefault();
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
