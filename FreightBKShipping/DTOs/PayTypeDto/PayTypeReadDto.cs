namespace FreightBKShipping.DTOs.PayTypeDto
{
    public class PayTypeReadDto
    {
        public int PayTypeId { get; set; }
        public string PayTypeName { get; set; } = string.Empty;
        public bool PayTypeStatus { get; set; }
    }
}
