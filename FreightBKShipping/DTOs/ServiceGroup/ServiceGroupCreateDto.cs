namespace FreightBKShipping.DTOs.ServiceGroup
{
    public class ServiceGroupCreateDto
    {
        public string ServiceGroupsName { get; set; } = string.Empty;
        public bool ServiceGroupsStatus { get; set; } = true;
        public string? ServiceGroupsRemarks { get; set; }
    }
}
