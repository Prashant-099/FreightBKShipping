

namespace FreightBKShipping.DTOs.User
{
    public class UserReadDTo
    {

        public string UserId { get; set; }
        public required string UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public required string UserEmail { get; set; }
        public string? UserName { get; set; }
        public string? UserPhone { get; set; }
        public string? UserCountryCode { get; set; }
        public required string UserMobile { get; set; }
        public required byte UserStatus { get; set; }
        public int UserCompanyId { get; set; }


        public required string UserRoleName { get; set; }
        public string? UserRoleId { get; set; }
        public string? UserGender { get; set; }
        public string? UserAddress { get; set; }
        public int? UserBranchId { get; set; }



        // Add other properties as needed
    }
}
