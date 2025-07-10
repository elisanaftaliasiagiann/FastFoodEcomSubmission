using FastFood.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace fastFood.web.Controllers
{
    // Ini membuat URL jadi /RoleSeeder/...
    [Route("[controller]")]
    public class RoleSeederController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleSeederController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Ini membuat URL jadi /RoleSeeder/AddAdminRole
        [HttpGet("AddAdminRole")]
        public async Task<IActionResult> AddAdminRole()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            return Content("Admin role berhasil dibuat.");
        }
    }
}
