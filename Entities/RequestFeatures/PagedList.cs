﻿namespace Entities.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            };
            AddRange(items);
        }

        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize, int? dataCount = null)
        {
            int count = dataCount != null ? (int)dataCount : source.Count();

            List<T> items = await source.Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static PagedList<T> ToPagedList(IList<T> source, int pageNumber, int pageSize, int? dataCount = null)
        {
            int count = dataCount != null ? (int)dataCount : source.Count();

            List<T> items = source.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
