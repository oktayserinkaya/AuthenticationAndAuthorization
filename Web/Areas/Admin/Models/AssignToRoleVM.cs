using Microsoft.AspNetCore.Identity;
using Web.Models.Entities;

namespace Web.Areas.Admin.Models
{
    public class AssignToRoleVM
    {
        public required string RoleName { get; set; }

        public List<AppUser> HasRole { get; set; } = [];

        public List<AppUser> HasNotRole { get; set; } = [];

        public string[]? AddIds { get; set; }

        public string[]? DeleteIds { get; set; }
    }
}
