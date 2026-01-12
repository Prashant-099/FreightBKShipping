using System.ComponentModel.DataAnnotations;
namespace FreightBKShipping.DTOs.User;
public class UserAddDto
{

    [Required(ErrorMessage = "Please select a role.")]
    public string UserRoleId { get; set; }

    public string? UserParentId { get; set; } 

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100)]
    public string UserFirstName { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100)]
    public string UserName { get; set; }

    public string? UserLastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string UserEmail { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    public string UserPassword { get; set; }

    public string? UserZipcode { get; set; }

    public string? UserPhone { get; set; }

    public string? UserCountryCode { get; set; } = "1";

    [Required(ErrorMessage = "Mobile number is required.")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter valid 10-digit mobile number.")]
    public string UserMobile { get; set; }

    public string? UserImage { get; set; }

    public string? UserAddress { get; set; }

    public List<int> AssignedBranchIds { get; set; } = new();

}
