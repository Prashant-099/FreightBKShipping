using System;

namespace FreightBKShipping.DTOs
{
    public class VesselReadDto
    {
        public int VesselId { get; set; }
        public string? VesselUuid { get; set; }
        public string VesselName { get; set; }
        public bool VesselStatus { get; set; }
        public double? VesselQty { get; set; }
        public double? VesselCbm { get; set; }
        public int? VesselNoOfBL { get; set; }
        public DateTime? VesselDtSailing { get; set; }
        public DateTime? VesselDtBerting { get; set; }
        public DateTime? VesselDtDemmurate { get; set; }
        public DateTime? VesselEta { get; set; }
        public DateTime? VesselEdt { get; set; }
        public DateTime? VesselAta { get; set; }
    }

  
}
