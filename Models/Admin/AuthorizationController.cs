using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace LoginEkrani.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthorizationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Bu metod, roller ve kullanıcı oluşturma işlemlerini başlatır
        public async Task<IActionResult> SeedRolesAndUsers()
        {
            // "Admin" rolünü oluştur
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                if (!roleResult.Succeeded)
                {
                    return Content("Admin rolü oluşturulamadı.");
                }
            }

            // Admin kullanıcıyı oluştur
            var adminUser = new IdentityUser { UserName = "admin" };
            var user = await _userManager.FindByEmailAsync("admin@example.com");
            if (user == null)
            {
                var userResult = await _userManager.CreateAsync(adminUser, "Admin123!");

                if (!userResult.Succeeded)
                {
                    return Content("Admin kullanıcısı oluşturulamadı.");
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(adminUser, "Admin");
                if (!addToRoleResult.Succeeded)
                {
                    return Content("Admin kullanıcısı role eklenemedi.");
                }
            }

            return Content("Veritabanı başarıyla başlatıldı, Admin rolü ve kullanıcısı oluşturuldu.");
        }
    }
}
