using System.ComponentModel.DataAnnotations;

namespace FreightBKShipping.Models
{
    public class DocumentsSaved
    {
        [Key]
        public long DocumentId { get; set; }

        public int CompanyId { get; set; }

        public string Category { get; set; }
        public string? SubCategory { get; set; }

        public string? ReferenceType { get; set; }
        public string? ReferenceId { get; set; }

        public string ContainerName { get; set; }
        public string BlobName { get; set; }
        public string? BlobUrl { get; set; }

        public string OriginalFileName { get; set; }
        public string StoredFileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public long FileSizeBytes { get; set; }

        public string UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

}
