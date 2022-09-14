namespace Entities.DBModels.LogModels
{
    public class Log : BaseEntity
    {
        [DisplayName(nameof(Level))]
        public string Level { get; set; }

        [DisplayName(nameof(Details))]
        public string Details { get; set; }

        [DisplayName(nameof(StackTrace))]
        public string StackTrace { get; set; }

        [DisplayName(nameof(Logger))]
        public string Logger { get; set; }
    }
}
