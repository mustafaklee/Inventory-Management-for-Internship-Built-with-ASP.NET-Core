using LoginEkrani.Controllers.LoginController;
using Microsoft.EntityFrameworkCore;



namespace LoginEkrani.Models.Login.Roles
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Database(
                serviceProvider.GetRequiredService<DbContextOptions<Database>>()))
            {
                // Roller daha önce eklenmiş mi kontrol et
                if (!context.kpsft_roller.Any())
                {
                    // Roller yoksa ekle
                    context.kpsft_roller.AddRange(
                        new Role { kpsft_rol = "Admin" },
                        new Role { kpsft_rol = "User" }
                    );
                    context.SaveChanges();
                }

                // Benzer şekilde diğer veriler eklenebilir
            }
        }
    }
}
