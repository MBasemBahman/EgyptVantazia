using Entities.CoreServicesModels.NewsModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.NewsModels;

namespace CoreServices.Logic
{
    public class NewsServices
    {
        private readonly RepositoryManager _repository;

        public NewsServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region News Services
        public IQueryable<NewsModel> GetNews(NewsParameters parameters,
                bool otherLang)
        {
            return _repository.News
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new NewsModel
                       {
                           Title = otherLang ? a.NewsLang.Title : a.Title,
                           ShortDescription = otherLang ? a.NewsLang.ShortDescription : a.ShortDescription,
                           LongDescription = otherLang ? a.NewsLang.LongDescription : a.LongDescription,
                           NewsTypeEnum = a.NewsTypeEnum,
                           Fk_GameWeak = a.Fk_GameWeak,
                           GameWeak = a.Fk_GameWeak > 0 ? new GameWeakModel
                           {
                               Name = otherLang ? a.GameWeak.GameWeakLang.Name : a.GameWeak.Name,
                           } : null,
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           AttachmentsCount = a.NewsAttachments.Count,
                           NewsAttachments = parameters.GetAttachments ? a.NewsAttachments
                                                                          .Select(b => new NewsAttachmentModel
                                                                          {
                                                                              FileLength = b.FileLength,
                                                                              CreatedAt = b.CreatedAt,
                                                                              FileName = b.FileName,
                                                                              FileType = b.FileType,
                                                                              FileUrl = b.StorageUrl + b.FileUrl,
                                                                              Id = b.Id,
                                                                          })
                                                                          .ToList() : null
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<NewsModel>> GetNewsPaged(
                  NewsParameters parameters,
                  bool otherLang)
        {
            return await PagedList<NewsModel>.ToPagedList(GetNews(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<News> FindNewsbyId(int id, bool trackChanges)
        {
            return await _repository.News.FindById(id, trackChanges);
        }

        public void CreateNews(News News)
        {
            _repository.News.Create(News);
        }

        public async Task DeleteNews(int id)
        {
            News News = await FindNewsbyId(id, trackChanges: true);
            _repository.News.Delete(News);
        }

        public NewsModel GetNewsbyId(int id, bool otherLang)
        {
            return GetNews(new NewsParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetNewsCount()
        {
            return _repository.News.Count();
        }
        #endregion

        #region News Attachment Services
        public IQueryable<NewsAttachmentModel> GetNewsAttachments(NewsAttachmentParameters parameters)
        {
            return _repository.NewsAttachment
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new NewsAttachmentModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           Fk_News = a.Fk_News,
                           FileLength = a.FileLength,
                           FileName = a.FileName,
                           FileType = a.FileType,
                           FileUrl = a.StorageUrl + a.FileUrl
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<NewsAttachmentModel>> GetNewsAttachmentsPaged(
                  NewsAttachmentParameters parameters)
        {
            return await PagedList<NewsAttachmentModel>.ToPagedList(GetNewsAttachments(parameters), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<NewsAttachment> FindNewsAttachmentbyId(int id, bool trackChanges)
        {
            return await _repository.NewsAttachment.FindById(id, trackChanges);
        }

        public void CreateNewsAttachment(NewsAttachment NewsAttachment)
        {
            _repository.NewsAttachment.Create(NewsAttachment);
        }

        public async Task DeleteNewsAttachment(int id)
        {
            NewsAttachment NewsAttachment = await FindNewsAttachmentbyId(id, trackChanges: true);
            _repository.NewsAttachment.Delete(NewsAttachment);
        }

        public NewsAttachmentModel GetNewsAttachmentbyId(int id)
        {
            return GetNewsAttachments(new NewsAttachmentParameters { Id = id }).SingleOrDefault();
        }

        public int GetNewsAttachmentCount()
        {
            return _repository.NewsAttachment.Count();
        }

        public async Task<string> UploudFile(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/NewsAttachment");
        }
        #endregion

    }
}
