using Entities.DBModels.AuditModels;

namespace FantasyLogic
{
    public class HangFireCustomJob
    {
        private readonly UnitOfWork _unitOfWork;
        public HangFireCustomJob(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void ReplaceJob(string hangfireJobId, string myJobId)
        {
            hangfireJobId = _unitOfWork.Audit.Create(new JobAudit
            {
                HangfireJobId = hangfireJobId,
                MyJobId = myJobId
            });

            if (!string.IsNullOrEmpty(hangfireJobId))
            {
                _ = BackgroundJob.Delete(hangfireJobId.ToString());
            }

            _unitOfWork.Save().Wait();
        }
    }
}
