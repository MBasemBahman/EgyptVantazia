using Entities.CoreServicesModels.LogModels;
using Entities.DBModels.LogModels;

namespace CoreServices.Logic
{
    public class LogServices
    {
        private readonly RepositoryManager _repository;

        public LogServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Log Services

        public IQueryable<LogModel> GetLogs(LogParameters parameters,
            bool trackChanges)
        {
            return _repository.Log
                       .FindAll(parameters, trackChanges)
                       .Select(a => new LogModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           Level = a.Level,
                           Logger = a.Logger,
                           StackTrace = a.StackTrace,
                           Details = a.Details,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public int GetLogsCount()
        {
            return _repository.Log.Count();
        }

        public LogModel GetLogbyId(int id, bool trackChanges)
        {
            return GetLogs(new LogParameters { Id = id }, trackChanges).SingleOrDefault();
        }


        public async Task<PagedList<LogModel>> GetLogsPaged(
             LogParameters parameters,
             bool trackChanges)
        {
            return await PagedList<LogModel>.ToPagedList(GetLogs(parameters, trackChanges), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Log> FindLogbyId(int id, bool trackChanges)
        {
            return await _repository.Log.FindById(id, trackChanges);
        }


        public async Task DeleteLog(int id)
        {
            Log log = await FindLogbyId(id, trackChanges: false);
            _repository.Log.Delete(log);
        }


        #endregion

    }
}
