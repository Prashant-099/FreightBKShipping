using System.ComponentModel.DataAnnotations;

namespace FreightBKShipping.DTOs.User
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Please select a role.")]
        public string UserRoleId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name can't be more than 100 characters.")]
        public string UserFirstName { get; set; } = string.Empty;

      
        public string? UserLastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string UserEmail { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(15, ErrorMessage = "Phone number can't exceed 15 digits.")]
        public string? UserPhone { get; set; }

        [StringLength(5, ErrorMessage = "Country code should not exceed 5 digits.")]
        public string? UserCountryCode { get; set; } = "1";

        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile must be exactly 10 digits.")]
        public string UserMobile { get; set; } = string.Empty;

        public string? UserAddress { get; set; }

        public bool? UserStatus { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters.")]
        public string UserName { get; set; } = string.Empty;
    }
}
