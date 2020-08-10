using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PMANews.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PMANews.Services
{
    public interface IUserProfileLoader
    {
        Task<ApplicationUser> GetCurrentApplicationUser(ClaimsPrincipal user);
    }

    public class UserServices : IUserProfileLoader
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _CurrentUser;
        public UserServices([FromServices] UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApplicationUser> GetCurrentApplicationUser(ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            _CurrentUser = await _userManager.GetUserAsync(user);

            //_CurrentUser = await _userManager.FindByIdAsync(userId);

            return _CurrentUser;
        }
    }
}
