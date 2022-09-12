using Microsoft.AspNetCore.Http;

namespace Contracts.Services
{
    public interface IExcelService
    {
        Task<string> Export<T>(List<T> Data, string storagePath);
        Task<List<List<string>>> Import(IFormFile formFile, int rowStart, int colStart, int rowEnd, int colEnd);
    }
}
