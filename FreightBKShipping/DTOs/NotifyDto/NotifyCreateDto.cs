namespace FreightBKShipping.DTOs
{
    public class NotifyCreateDto
    {
        public string NotifyName { get; set; }
        public string NotifyType { get; set; }
        public string NotifyAddress1 { get; set; }
        public string NotifyAddress2 { get; set; }
        public string NotifyAddress3 { get; set; }
        public string NotifyCity { get; set; }
        //public string NotifyState { get; set; }
        public int NotifyStateId { get; set; }
        public string NotifyStateCode { get; set; }
        public string NotifyPincode { get; set; }
        public string NotifyCountry { get; set; }
        public string NotifyTel { get; set; }
        public string NotifyEmail { get; set; }
        public string NotifyContactNo { get; set; }
        public string NotifyContactPerson { get; set; }
        public string NotifyGstNo { get; set; }
        public string NotifyPan { get; set; }
        public bool NotifyStatus { get; set; }
    }

    public class NotifyUpdateDto : NotifyCreateDto
    {
        // For now, same as CreateDto; can extend if needed
    }
}
