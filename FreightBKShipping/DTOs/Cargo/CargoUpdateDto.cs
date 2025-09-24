namespace FreightBKShipping.DTOs.Cargo
{
    public class CargoUpdateDto
    {
        public string? CargoUpdatedbyUserId { get; set; }
        public string CargoName { get; set; } = string.Empty;
        public string? CargoType { get; set; }
        public string? CargoRemarks { get; set; }
        public int? CargoHsn { get; set; }
        public float CargoGstPer { get; set; }
        public float CargoCess { get; set; }
        public bool CargoStatus { get; set; }
    }
}
