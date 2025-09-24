namespace FreightBKShipping.DTOs.ServiceDto
{
    public class ServiceCreateDto
    {
        public string ServiceName { get; set; } = string.Empty;
        public string? ServiceCode { get; set; }
        public int ServiceGroupId { get; set; }
        public double ServiceSRate { get; set; }
        public float ServicePRate { get; set; }
        public bool ServiceStatus { get; set; } = true;
        public string? ServiceRemarks { get; set; }
    }
}
