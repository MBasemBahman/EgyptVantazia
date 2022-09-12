namespace Entities.DBModels.SharedModels
{
    public class LangEntity<T> : BaseEntity, ILangEntity<T>
    {
        [DisplayName(nameof(Source))]
        [ForeignKey(nameof(Source))]
        public int Fk_Source { get; set; }

        [DisplayName(nameof(Source))]
        public T Source { get; set; }
    }

    public class AuditLangEntity<T> : AuditEntity, ILangEntity<T>
    {
        [DisplayName(nameof(Source))]
        [ForeignKey(nameof(Source))]
        public int Fk_Source { get; set; }

        [DisplayName(nameof(Source))]
        public T Source { get; set; }
    }
}
