using Entities.ServicesModels;

namespace Contracts.Services
{
    public interface IDataTable<T> where T : class
    {
        DataTableResult<T> LoadTable(DtParameters dtParameters, IEnumerable<T> resultData, int filteredCount, int totalCount);
        DtResult<T> ReturnTable(DataTableResult<T> dataTableResult);
    }
}
