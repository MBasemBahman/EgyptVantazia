using Entities.DBModels.AuditModels;

namespace CoreServices.Logic
{
    public class AuditServices
    {
        private readonly RepositoryManager _repository;

        public AuditServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region JobAudit
        public JobAudit FindByJobId(string myJobId, bool trackChanges)
        {
            return _repository.JobAudit.FindByJobId(myJobId, trackChanges);
        }
        public string Create(JobAudit entity)
        {
            return _repository.JobAudit.Create(entity);
        }
        #endregion
    }
}
