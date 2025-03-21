﻿using Entities.CoreServicesModels.NewsModels;
using Entities.DBModels.NewsModels;


namespace Repository.DBModels.NewsModels
{
    public class NewsAttachmentRepository : RepositoryBase<NewsAttachment>
    {
        public NewsAttachmentRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<NewsAttachment> FindAll(NewsAttachmentParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_News);
        }

        public async Task<NewsAttachment> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }
    }

    public static class NewsAttachmentRepositoryExtension
    {
        public static IQueryable<NewsAttachment> Filter(
            this IQueryable<NewsAttachment> NewsAttachments,
            int id,
            int Fk_News)
        {
            return NewsAttachments.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_News == 0 || a.Fk_News == Fk_News));

        }

    }
}
