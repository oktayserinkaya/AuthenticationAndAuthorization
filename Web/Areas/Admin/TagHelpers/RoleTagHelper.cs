using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Web.Models.Entities;

namespace Web.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "user-role")]
    public class RoleTagHelper(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) : TagHelper
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        [HtmlAttributeName("user-role")]
        public required string RoleName { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> userNames = [];

            var role = await _roleManager.FindByNameAsync(RoleName);

            if (role != null)
            {
                foreach (var user in await _userManager.Users.ToListAsync())
                {
                    if (user.UserName != null && await _userManager.IsInRoleAsync(user, RoleName))
                    {
                        userNames.Add(user.UserName);
                    }
                }
            }

            var userNamesText = string.Join(", ", userNames);
            output.Content.SetContent
                (
                    userNames.Count == 0 ? "Kullanıcı Yok!..." : userNamesText.Length > 43 ? userNamesText[..40] + "..." : userNamesText
                );
        }
    }
}
