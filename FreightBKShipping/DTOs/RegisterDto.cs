using System.ComponentModel.DataAnnotations;

namespace FreightBKShipping.DTOs
{
    public class RegisterDto
    {
        [Required] public string UserEmail { get; set; }
        [Required] public string UserPassword { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public int UserCompanyId { get; set; }
    }
    public class ServiceResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static ServiceResponse Success(string message) => new ServiceResponse { IsSuccess = true, Message = message };
        public static ServiceResponse Fail(string message) => new ServiceResponse { IsSuccess = false, Message = message };
    }

}
