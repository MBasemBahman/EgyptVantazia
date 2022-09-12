using Microsoft.AspNetCore.Http;

namespace Contracts.Services
{
    public interface IFileUploader
    {
        Task<string> UploudFile(IFormFile file, string path);
        void DeleteFile(string filePath);
    }
}
