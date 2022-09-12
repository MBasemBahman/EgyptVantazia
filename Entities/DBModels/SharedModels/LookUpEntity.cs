namespace Entities.DBModels.SharedModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class LookUpEntity : BaseEntity, ILookUpEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }

    [Index(nameof(Name), IsUnique = true)]
    public class AuditLookUpEntity : AuditEntity, ILookUpEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
