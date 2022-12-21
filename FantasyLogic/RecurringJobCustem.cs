namespace FantasyLogic
{
    public static class RecurringJobCustom
    {
        public static void TriggerJob(string recurringId)
        {
            _ = BackgroundJob.Delete(recurringId);
            _ = RecurringJob.TriggerJob(recurringId);
        }
    }
}
