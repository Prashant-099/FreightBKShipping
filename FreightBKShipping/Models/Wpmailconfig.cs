using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("wpmailconfig")]
    public class WpMailConfig
    {
        [Key]
        [Column("wpmailconfig_id")]
        public int Id { get; set; }

        [Column("wpmailconfig_msgtype")]
        public string? MsgType { get; set; }

        [Column("wpmailconfig_templatetext")]
        public string? TemplateText { get; set; }

        [Column("wpmailconfig_mailsub")]
        public string? MailSub { get; set; }

        [Column("wpmailconfig_companyid")]
        public int? CompanyId { get; set; }

        [Column("wpmailconfig_created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Column("wpmailconfig_updated")]
        public DateTime Updated { get; set; } = DateTime.UtcNow;

        [Column("wpmailconfig_create_uid")]
        public string? CreateUid { get; set; }

        [Column("wpmailconfig_edited_uid")]
        public string? EditedUid { get; set; }

        [Column("wpmailconfig_isdefault")]
        public bool IsDefault { get; set; }
    }
}
