using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("notifies")]
    public class Notify
    {
        [Key]
        [Column("notify_id")]
        public int NotifyId { get; set; }

        [Column("notify_company_id")]
        public int NotifyCompanyId { get; set; }

        [Column("notify_state_id")]
        public int NotifyStateId { get; set; }

        [Column("notify_addedby_user_id")]
        public string NotifyAddedByUserId { get; set; }

        [Column("notify_updatedby_user_id")]
        public string NotifyUpdatedByUserId { get; set; }

        [Column("notify_name")]
        public string NotifyName { get; set; }

        [Column("notify_type")]
        public string NotifyType { get; set; }

        [Column("notify_address1")]
        public string NotifyAddress1 { get; set; }

        [Column("notify_address2")]
        public string NotifyAddress2 { get; set; }

        [Column("notify_address3")]
        public string NotifyAddress3 { get; set; }

        [Column("notify_city")]
        public string NotifyCity { get; set; }

        [Column("notify_tel")]
        public string NotifyTel { get; set; }

        [Column("notify_email")]
        public string NotifyEmail { get; set; }

        [Column("notify_contactno")]
        public string NotifyContactNo { get; set; }

        [Column("notify_contactperson")]
        public string NotifyContactPerson { get; set; }

        [Column("notify_gstno")]
        public string NotifyGstNo { get; set; }

        [Column("notify_pan")]
        public string NotifyPan { get; set; }

        [Column("notify_status")]
        public bool NotifyStatus { get; set; }

        [Column("notify_state")]
        public string NotifyState { get; set; }

        [Column("notify_pincode")]
        public string NotifyPincode { get; set; }

        [Column("notify_statecode")]
        public string NotifyStateCode { get; set; }

        [Column("notify_created")]
        public DateTime NotifyCreated { get; set; }

        [Column("notify_updated")]
        public DateTime NotifyUpdated { get; set; }

        [Column("notify_country")]
        public string NotifyCountry { get; set; }
    }
}
