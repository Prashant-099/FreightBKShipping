//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace FreightBKShipping.Models
//{
//    [Table("billref_details")]
//    public class BillRefDetail
//    {
//        [Key]
//        [Column("billref_detail_id")]
//        [StringLength(50)]
//        public string BillRefDetailId { get; set; } = string.Empty;



//        //[Column("billref_detail_bill_id")]   // <-- actual FK column in table
//        //public int BillRefDetailBillId { get; set; }

//        //[ForeignKey(nameof(BillRefDetailBillId))]
//        //public Bill Bill { get; set; } = null!;   // navigation property




//        [Column("billref_against_id")]
//        [StringLength(45)]
//        public string? BillRefAgainstId { get; set; }

//        [Column("billref_vch_type")]
//        [StringLength(50)]
//        public string? BillRefVchType { get; set; }

//        [Column("billref_vch_no")]
//        [StringLength(50)]
//        public string? BillRefVchNo { get; set; }

//        [Column("billref_vch_id")]
//        [StringLength(50)]
//        public string? BillRefVchId { get; set; }

//        [Column("billref_vch_date")]
//        public DateTime? BillRefVchDate { get; set; }

//        [Column("billref_vch_amount")]
//        public double BillRefVchAmount { get; set; }

//        [Column("billref_vch_dis")]
//        public float BillRefVchDis { get; set; }

//        [Column("billref_vch_tds")]
//        public float BillRefVchTds { get; set; }

//        [Column("billref_vch_short")]
//        public float BillRefVchShort { get; set; }

//        [Column("billref_vch_balance")]
//        public float BillRefVchBalance { get; set; }

//        [Column("billref_accountid")]
//        public int BillRefAccountId { get; set; }
//    }
//}
