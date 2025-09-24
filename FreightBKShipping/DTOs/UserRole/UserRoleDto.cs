using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreightBKShipping.DTOs.UserRole
{
    public class UserRoleDto
    {
        public string RoleUuid { get; set; } = default!;

        public string RoleName { get; set; } = default!;
        public int RoleStatus { get; set; }
        public DateTime RoleCreated { get; set; }
        public DateTime RoleUpdated { get; set; }
        public int RoleCompanyId { get; set; }
    }

    public class UserRoleCreateDto
    {
        [Required]
        public int RoleCompanyId { get; set; } = default!;

        [Required]
        [StringLength(60)]
        public string RoleAddedByUserId { get; set; } = default!;

        [Required(ErrorMessage = "Enter Role Name")]
        [StringLength(220)]
        public string RoleName { get; set; } = default!;
    

    }

    public class UserRoleUpdateDto
    {
        [Required]
        public string RoleUuid { get; set; } = default!;

        [StringLength(60)]
        public string? RoleUpdatedByUserId { get; set; }

        [Required(ErrorMessage = "Enter Role Name")]
        [StringLength(220)]
        public string RoleName { get; set; } = default!;

        public int RoleStatus { get; set; } = 1;
    }
}
