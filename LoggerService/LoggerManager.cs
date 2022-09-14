using Contracts;
using Contracts.Logger;
using Entities.LogFeatures;
using Entities.ResponseFeatures;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using Services;

namespace LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly ILocalizationManager _localization;

        public LoggerManager(ILocalizationManager localization)
        {
            _localization = localization;
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }

        public Response LogError(HttpRequest request, Exception exception)
        {
            LogDetails logModel = new();

            if (exception != null)
            {
                logModel.ErrorMessage = _localization.Get(exception.Message ?? "");
                logModel.ExceptionMessage = exception.InnerException == null ? exception.Message : exception.InnerException.Message;

                logModel.Host = request.Host.Value;
                logModel.Path = request.Method;
                logModel.Method = request.Path.Value;
                logModel.ContentType = request.ContentType;
                logModel.QueryString = request.QueryString.Value;

                if (request.ContentLength > 0)
                {
                    if (request.HasFormContentType)
                    {
                        logModel.Body = JsonConvert.SerializeObject(request.Form);
                    }
                    else
                    {
                        using StreamReader reader = new(request.Body);
                        _ = request.Body.Seek(0, SeekOrigin.Begin);
                        logModel.Body = reader.ReadToEnd();
                    }
                }
                logger.Error(logModel.ToString());
            }

            return new Response
            {
                ErrorMessage = logModel.ErrorMessage.Length > 80 ? "Something error, please try again!" : logModel.ErrorMessage,
                ExceptionMessage = StringChecker.HasArabicCharacters(logModel.ExceptionMessage) ? "" : logModel.ExceptionMessage
            };
        }
    }
}