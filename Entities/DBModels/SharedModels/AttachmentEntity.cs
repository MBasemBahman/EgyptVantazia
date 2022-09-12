namespace Entities.DBModels.SharedModels
{
    public class AttachmentEntity : BaseEntity, IAttachmentEntity
    {
        [DisplayName(nameof(FileName))]
        public string FileName { get; set; }

        [DisplayName(nameof(FileType))]
        public string FileType { get; set; }

        [DisplayName(nameof(FileLength))]
        public double FileLength { get; set; }

        [DisplayName(nameof(FileUrl))]
        public string FileUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string StorageUrl { get; set; }
    }

    public class AuditAttachmentEntity : AuditEntity, IAttachmentEntity
    {
        [DisplayName(nameof(FileName))]
        public string FileName { get; set; }

        [DisplayName(nameof(FileType))]
        public string FileType { get; set; }

        [DisplayName(nameof(FileLength))]
        public double FileLength { get; set; }

        [DisplayName(nameof(FileUrl))]
        public string FileUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string StorageUrl { get; set; }
    }
}
