namespace FreightBKShipping.DTOs.ServiceGroup
{
    public class ServiceGroupReadDto
    {
        public int ServiceGroupsId { get; set; }
        public string ServiceGroupsName { get; set; } = string.Empty;
        public bool ServiceGroupsStatus { get; set; }
        public string? ServiceGroupsRemarks { get; set; }
        public DateTime? ServiceGroupsUpdated { get; set; }
        public DateTime ServiceGroupsAdded { get; set; }
    }
}
