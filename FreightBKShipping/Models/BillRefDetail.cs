using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("billref_details")]
    public class BillRefDetail
    {
        [Key]
        [Column("billref_detail_id")]
        public int BillRefDetailId { get; set; }

        [Column("billref_against_id")]
        public int BillRefAgainstId { get; set; }

        [Column("billref_vch_type")]
        public string? BillRefVchType { get; set; }

        [Column("billref_vch_no")]
        public string? BillRefVchNo { get; set; }

        [Column("billref_vch_id")]
        public int BillRefVchId { get; set; }

        [Column("billref_vch_date")]
        public DateTime BillRefVchDate { get; set; }

        [Column("billref_vch_amount")]
        public double BillRefVchAmount { get; set; }

        [Column("billref_vch_dis")]
        public float BillRefVchDis { get; set; }

        [Column("billref_vch_tds")]
        public float BillRefVchTds { get; set; }

        [Column("billref_vch_short")]
        public float BillRefVchShort { get; set; }

        [Column("billref_vch_balance")]
        public float BillRefVchBalance { get; set; }

        [Column("billref_accountid")]
        public int BillRefAccountId { get; set; }

        // ✅ Navigation property back to Journal
        [ForeignKey("BillRefAgainstId")]
        public virtual Journal? Journal { get; set; }
    }
}