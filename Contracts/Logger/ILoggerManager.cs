using Entities.ResponseFeatures;
using Microsoft.AspNetCore.Http;

namespace Contracts.Logger
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
        Response LogError(HttpRequest request, Exception exception);
    }
}
