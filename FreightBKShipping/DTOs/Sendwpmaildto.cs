using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FreightBKShipping.DTOs
{
    public class SendWpMailDto
    {
        public int SendWpMailId { get; set; }
        public string? SendWpMailUserName { get; set; }
        public string? SendWpMailWpToken { get; set; }
        public string? SendWpMailBalanceToken { get; set; }
        public DateTime? SendWpMailCreated { get; set; }
        public DateTime? SendWpMailUpdated { get; set; }
        public string? SendWpMailCreateUid { get; set; }
        public string? SendWpMailEditedUid { get; set; }
        public int? SendWpMailCompanyid { get; set; }
    
        public string SendWpMailWpBalanceToken { get; set; }

    }

    public class SendWpMailAddDto
    {
        public string? SendWpMailUserName { get; set; }
        public string? SendWpMailWpToken { get; set; }
        public string? SendWpMailBalanceToken { get; set; }
        public int? SendWpMailCompanyid { get; set; }
        public string SendWpMailWpBalanceToken { get; set; }


    }

    public class SendWpMailConfigDto
    {
        public string? EmailFrom { get; set; }
        public string? SmtpServer { get; set; }
        public string? SmtpPort { get; set; }
        public string? EmailUser { get; set; }
        public string? EmailPass { get; set; }
        public bool EnableSsl { get; set; }

    }

    public class SendEmailRequestDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? CcEmails { get; set; } 

        public string? AttachmentFileName { get; set; }
        public string? AttachmentBase64 { get; set; }
    }
    public class WpMailConfigDto
    {
        public int Id { get; set; }
        public string? MsgType { get; set; }
        public string? TemplateText { get; set; }
        public string? MailSub { get; set; }
        public int? CompanyId { get; set; }
        public bool IsDefault { get; set; } // <-- add this

    }

    public class WpMailConfigAddDto
    {
        public bool IsDefault { get; set; } // <-- add this

        public string? MsgType { get; set; }
        public string? TemplateText { get; set; }
        public string? MailSub { get; set; }
    }
}
