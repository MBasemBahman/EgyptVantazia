﻿using Entities.CoreServicesModels.AccountModels;
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
                                  StorageUrl = a.StorageUrl,
                                  FirstName = a.FirstName,
                                  LastName = a.LastName,
                                  EmailAddress = a.User.EmailAddress,
                                  PhoneNumber = a.User.PhoneNumber,
                                  UserName = a.User.UserName,
                                  LastActive = a.User.RefreshTokens.Any()
                                  ? a.User.RefreshTokens.OrderByDescending(b => b.Id).Select(a => a.CreatedAt).FirstOrDefault()
                                  : null,
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
