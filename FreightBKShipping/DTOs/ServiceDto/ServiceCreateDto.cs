namespace FreightBKShipping.DTOs.ServiceDto
{
    public class ServiceCreateDto
    {
        public int ServiceCompanyId { get; set; }
        public int? ServiceGroupId { get; set; }
        public int? ServiceUnitId { get; set; }

        public string ServiceName { get; set; } = string.Empty;
        public string? ServiceCode { get; set; }
        public string? ServiceType { get; set; }

        public double? ServiceSRate { get; set; }
        public float? ServicePRate { get; set; }

        public string? ServiceChargeType { get; set; }
        public int? ServiceHsnId { get; set; }
        public bool? ServiceExempt { get; set; }

        public string? ServiceRemarks { get; set; }
        public string? ServicePrintName { get; set; }
        public string? ServiceTallyName { get; set; }

        public bool? ServiceStatus { get; set; } = true;

        public float? ServiceExtraCharge { get; set; }
        public string? ServiceCeilingType { get; set; }
        public float? ServiceCeilingValue { get; set; }

        public int? ServiceVoucherId { get; set; }
        public int? ServiceAccountId { get; set; }
        public int? ServiceIsGoods { get; set; }

        public string? ServiceAddedByUserId { get; set; }
        public string? ServiceUpdatedByUserId { get; set; }

        public DateTime? ServiceCreated { get; set; } = DateTime.UtcNow;
        public DateTime? ServiceUpdated { get; set; }
    }
}
