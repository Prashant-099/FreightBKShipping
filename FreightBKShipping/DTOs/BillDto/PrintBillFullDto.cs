namespace FreightBKShipping.DTOs.BillDto
{
    public class PrintBillFullDto
    {
      
        public BillPrintDto Bill { get; set; } = new();
        public List<BillDetailPrintDto> BillDetails { get; set; } = new();
    }

  
    public class BillPrintDto
    {

        public string? bill_ack_no{ get; set; }
        public string bill_ack_date { get; set; }
        public string? bill_irn_no { get; set; }
        public int BillId { get; set; }
        public string? bill_no { get; set; }
        public DateTime bill_date { get; set; }
        public DateTime? bill_duedate { get; set; }
        public string? account_print_name { get; set; }
      //  public string? account_pan { get; set; }
        public string? account_state { get; set; }
        public string? account_address1 { get; set; }
        public string? account_gstno { get; set; }
     
        public string? PlaceofSupply { get; set; }
        public string? ShipmentType { get; set; }
        public string? Cargo { get; set; }
        public string? bill_blno { get; set; }
        public string? bill_hblno { get; set; }
        public string? bill_sbno { get; set; }
        public DateTime? bill_bldate { get; set; }
        public DateTime? bill_hbldate { get; set; }
        public DateTime? bill_sbdate { get; set; }
        public string? bill_jobno { get; set; }
        public string? bill_jobtype { get; set; }
        public double? bill_grosswt { get; set; }
        public double? bill_netwt { get; set; }
        public string? Vessel { get; set; }
        public string? Line { get; set; }
        public string? bill_20ft { get; set; }
        public string? bill_40ft { get; set; }
        public string? shipper_invno { get; set; }
        public string? Shipper { get; set; }
        public string? Consignee { get; set; }
       
        public string? POL { get; set; }
        public string? POD { get; set; }
        public string? bill_container_no { get; set; }
       
        public double GrossAmount { get; set; }
        public double bill_taxableamt { get; set; }
        public double bill_sgst { get; set; }
        public double bill_cgst { get; set; }
        public double bill_igst { get; set; }

     
        public double bill_roundamt { get; set; }
        public double bill_total { get; set; }
        public string? bill_AmountInword { get; set; }
        public string? bill_detail_remarks { get; set; }

        public string? place_of_receipt { get; set; }
        public string? place_of_delivery { get; set; }
        public string? destination { get; set; }



        // ==== Company Info (header/footer in bill) ====
        public string? company_printname { get; set; }
      
        public string? company_address1 { get; set; }
        public string? company_gstin { get; set; }
        public string? State_Company { get; set; }
        public string? company_mobile { get; set; }
        public string? company_email { get; set; }
        public string? company_panno { get; set; }
        public string? company_website { get; set; }

        //banks details
        public string? bankname { get; set; }
        public string? bank_branch { get; set; }
        public string? bank_accountno { get; set; }
        public string? bank_ifsc { get; set; }
        public string? bank_address { get; set; }


    }

    public class BillDetailPrintDto
    {
       
        public string?service_printname { get; set; }
        public string? bill_detail_hsncode { get; set; }
        public string? bill_detail_exchunit { get; set; }
        public double bill_detail_qty { get; set; }
        public double bill_detail_rate { get; set; }
        public double bill_detail_amount { get; set; }
        public double bill_detail_exchrate { get; set; }
        public double bill_detail_sgst { get; set; }
        public double bill_detail_cgst { get; set; }    
        public double  bill_detail_igst{ get; set; }
        public double bill_detail_sgstper { get; set; }
        public double bill_detail_cgstper { get; set; }
        public double bill_detail_igstper { get; set; }
        public double bill_detail_taxableamt { get; set; }
        public double bill_detail_total { get; set; }
    }
}
