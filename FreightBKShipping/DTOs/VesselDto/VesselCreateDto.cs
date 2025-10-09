namespace FreightBKShipping.DTOs.VesselDto
{
    public class VesselCreateDto
    {
        public string VesselName { get; set; }
        public bool VesselStatus { get; set; } = true;
        public double? VesselQty { get; set; }
        public double? VesselCbm { get; set; }
        public int? VesselNoOfBL { get; set; }
        public double? VesselQtyOpening { get; set; }
        public double? VesselCbmOpening { get; set; }
        public double? VesselNoOfBLOpening { get; set; }
        public DateTime? VesselDtSailing { get; set; }
        public DateTime? VesselDtBerting { get; set; }
        public DateTime? VesselDtDemmurate { get; set; }
        public DateTime? VesselEta { get; set; }
        public DateTime? VesselEdt { get; set; }
        public DateTime? VesselAta { get; set; }
        public string? VesselsCol { get; set; }
    }

    public class VesselUpdateDto : VesselCreateDto
    {
        public int VesselId { get; set; }
    }
}
