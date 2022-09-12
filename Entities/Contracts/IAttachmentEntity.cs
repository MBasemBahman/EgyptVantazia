namespace Entities.Contracts
{
    public interface IAttachmentEntity
    {
        public string StorageUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public double FileLength { get; set; }
        public string FileUrl { get; set; }
    }
}
