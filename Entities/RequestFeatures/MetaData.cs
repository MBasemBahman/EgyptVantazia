namespace Entities.RequestFeatures
{
    public class MetaData
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public string PrevoisPageLink { get; set; }
        public string NextPageLink { get; set; }

        public static string PaginationMetaData(MetaData metaData, RequestParameters requestParameters, string actionUri)
        {
            metaData.PrevoisPageLink = metaData.HasPrevious ? UrlLink(requestParameters.PageNumber, requestParameters.PageSize - 1, requestParameters.OrderBy, requestParameters.SearchTerm, actionUri) : null;
            metaData.NextPageLink = metaData.HasPrevious ? UrlLink(requestParameters.PageNumber, requestParameters.PageSize + 1, requestParameters.OrderBy, requestParameters.SearchTerm, actionUri) : null;

            return JsonConvert.SerializeObject(metaData);
        }

        private static string UrlLink(int pageNumber, int pageSize, string orderBy, string searchTerm, string actionUri)
        {
            return $"{actionUri}?orderBy={orderBy}&searchTerm={searchTerm}&pageNumber={pageNumber}&pageSize={pageSize}";
        }
    }
}
