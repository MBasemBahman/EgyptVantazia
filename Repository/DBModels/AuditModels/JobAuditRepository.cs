using Entities.CoreServicesModels.AuditModels;
using Entities.DBModels.AuditModels;

namespace Repository.DBModels.AuditModels
{
    public class JobAuditRepository : RepositoryBase<JobAudit>
    {
        public JobAuditRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<JobAudit> FindAll(JobAuditParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.HangfireJobId,
                           parameters.MyJobId);
        }

        public async Task<JobAudit> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public JobAudit FindByJobId(string myJobId, bool trackChanges)
        {
            return FindAll(new JobAuditParameters
            {
                MyJobId = myJobId
            }, trackChanges).FirstOrDefault();
        }

        public new string Create(JobAudit entity)
        {
            string jobId = null;

            if (FindAll(new JobAuditParameters
            {
                MyJobId = entity.MyJobId
            }, trackChanges: false).Any())
            {
                JobAudit oldJob = FindAll(new JobAuditParameters
                {
                    MyJobId = entity.MyJobId
                }, trackChanges: true).First();

                jobId = oldJob.HangfireJobId;

                oldJob.HangfireJobId = entity.HangfireJobId;
            }
            else
            {
                base.Create(entity);
            }

            return jobId;
        }

        public new void Delete(JobAudit entity)
        {
            base.Delete(entity);
        }
    }

    public static class JobAuditRepositoryExtension
    {
        public static IQueryable<JobAudit> Filter(
            this IQueryable<JobAudit> JobAudits,
            int id,
            string hangfireJobId,
            string myJobId)
        {
            return JobAudits.Where(a => (id == 0 || a.Id == id) &&
                                        (string.IsNullOrWhiteSpace(hangfireJobId) || a.HangfireJobId == hangfireJobId) &&
                                        (string.IsNullOrWhiteSpace(myJobId) || a.MyJobId == myJobId));
        }

    }
}
