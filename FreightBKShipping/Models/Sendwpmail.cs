namespace FreightBKShipping.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("sendwpmail")]
    public class SendWpMail
    {
        [Key]
        [Column("sendwpmail_id")]
        public int SendWpMailId { get; set; }

        [Column("sendwpmail_username")]
        [MaxLength(45)]
        public string? SendWpMailUserName { get; set; }

        [Column("sendwpmail_wptoken")]
        [MaxLength(255)]
        public string? SendWpMailWpToken { get; set; }

        [Column("sendwpmail_balancetoken")]
        [MaxLength(255)]
        public string? SendWpMailBalanceToken { get; set; }

        [Column("sendwpmail_wpbalancetoken")]
        [MaxLength(255)]
        public string SendWpMailWpBalanceToken { get; set; }

        [Column("sendwpmail_created")]
        public DateTime? SendWpMailCreated { get; set; }

        [Column("sendwpmail_updated")]
        public DateTime? SendWpMailUpdated { get; set; }

        [Column("sendwpmail_create_uid")]
        [MaxLength(60)]
        public string? SendWpMailCreateUid { get; set; }

        [Column("sendwpmail_edited_uid")]
        [MaxLength(60)]
        public string? SendWpMailEditedUid { get; set; }

        [Column("sendwpmail_companyid")]
        public int? SendWpMailCompanyid { get; set; }


        [Column("sendwpmail_email_from")]
        [MaxLength(255)]
        public string? SendWpMailEmailFrom { get; set; }

        [Column("sendwpmail_smtp_server")]
        [MaxLength(255)]
        public string? SendWpMailSmtpServer { get; set; }

        [Column("sendwpmail_smtp_port")]
        [MaxLength(255)]
        public string? SendWpMailSmtpPort { get; set; }

        [Column("sendwpmail_email_user")]
        [MaxLength(255)]
        public string? SendWpMailEmailUser { get; set; }

        [Column("sendwpmail_email_pass")]
        [MaxLength(255)]
        public string? SendWpMailEmailPass { get; set; }

        [Column("sendwpmail_enable_ssl")]
        public bool? SendWpMailEnableSsl { get; set; }

    }

}
