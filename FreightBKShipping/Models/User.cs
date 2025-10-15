
using FreightBKShipping.DTOs.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [Column("user_role_id")]    
        public string? UserRoleId { get; set; }

        [Column("user_parent_id")]
        public string? UserParentId { get; set; } 

        [Required]
        [Column("user_first_name")]
        public string? UserFirstName { get; set; }

        [Column("user_last_name")]
        public string? UserLastName { get; set; }

        [Required]
        [Column("user_email")]
        public string UserEmail { get; set; }


        [Column("user_password")]
        public string UserPassword { get; set; }

        [Column("user_zipcode")]
        public string? UserZipcode { get; set; }

        [Column("user_phone")]
        public string? UserPhone { get; set; }

        [Column("user_country_code")]
        public string? UserCountryCode { get; set; }

        [Required]
        [Column("user_mobile")]
        public string UserMobile { get; set; }

        [Column("user_image")]
        public string? UserImage { get; set; }

        [Column("user_address")]
        public string? UserAddress { get; set; }

        [Column("user_contact_detail")]
        public string? UserContactDetail { get; set; }

        [Column("user_contact_person")]
        public string? UserContactPerson { get; set; }

        [Column("user_remarks")]
        public string? UserRemarks { get; set; }


        [Column("user_activation_key")]
        public string? UserActivationKey { get; set; }

        [Column("user_signup_type")]
        public string? UserSignupType { get; set; }

        [Column("user_private_id_address")]
        public string? UserPrivateIdAddress { get; set; }

        [Column("user_public_ip_address")]
        public string? UserPublicIpAddress { get; set; }

        [Column("user_last_login")]
        public DateTime? UserLastLogin { get; set; }

        [Column("user_otp")]
        public int? UserOtp { get; set; }

        [Column("user_otp_send_time")]
        public DateTime? UserOtpSendTime { get; set; }

        [Column("user_login_type")]
        public string? UserLoginType { get; set; }

        [Column("user_device_token")]
        public int? UserDeviceToken { get; set; }

        [Column("user_broswer")]
        public int? UserBroswer { get; set; }

        [Column("user_browser_version")]
        public int? UserBrowserVersion { get; set; }

        [Column("user_api_key")]
        public int? UserApiKey { get; set; }

        [Column("user_status")]
        public byte? UserStatus { get; set; }

        [Column("user_created")]
        public DateTime? UserCreated { get; set; } = DateTime.UtcNow;

        [Column("user_updated")]
        public DateTime? UserUpdated { get; set; } = DateTime.UtcNow;

        [Column("user_create_uid")]
        public string? UserAddbyUserId { get; set; }

        [Column("user_edited_uid")]
        public string? UserUpdatebyUserId { get; set; }

        [Column("user_company_id")]
        public int UserCompanyId { get; set; }

        [Column("user_name")]
        public string? UserName { get; set; }

        [Column("user_branch_id")]
        public int? UserBranchId { get; set; }

        [Column("user_password_reset")]
        public string? PasswordResetToken { get; set; }

        [Column("user_reset_token_Expire")]
        public DateTime? ResetTokenExpires { get; set; }
        [ForeignKey("UserRoleId")]
        [InverseProperty("Users")]
        public virtual UserRole Role { get; set; }
        //[ForeignKey(nameof(UserRoleId))]
        //public virtual UserRole Role { get; set; }
        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }

    public class PagedUserResponse
    {
        //public string? Message { get; set; }
        public PaginationInfo Pagination { get; set; }
        public List<UserReadDTo> Data { get; set; } = new();
    }

    public class PaginationInfo
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }

   
}
