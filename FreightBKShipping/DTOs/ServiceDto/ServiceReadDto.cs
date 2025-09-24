namespace FreightBKShipping.DTOs.ServiceDto
{
    public class ServiceReadDto
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string? ServiceCode { get; set; }
        public int ServiceGroupId { get; set; }
        public string? ServiceGroupName { get; set; }
        public bool ServiceStatus { get; set; }
        public double ServiceSRate { get; set; }
        public float ServicePRate { get; set; }
        public string? ServiceRemarks { get; set; }
    }
}
