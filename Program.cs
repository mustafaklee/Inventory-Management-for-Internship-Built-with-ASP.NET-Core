using Microsoft.EntityFrameworkCore;
using LoginEkrani;
using LoginEkrani.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using LoginEkrani.Models.Login.Roles;

var builder = WebApplication.CreateBuilder(args);

// LogService'i DI container'a ekleyin
builder.Services.AddScoped<LogService>();

// Controller ve View desteðini ekleyin
builder.Services.AddControllersWithViews();

// Session desteðini ekleyin
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Oturum süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuration nesnesine eriþim
var configuration = builder.Configuration;

// DbContext'i ekleyin ve MySQL baðlantý dizesini kullanýn
builder.Services.AddDbContext<Database>(options =>
    options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// Kimlik doðrulama için Cookie Authentication ekleyin
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Giriþ yapýlmadýðýnda yönlendirilecek sayfa
        options.LogoutPath = "/Logout"; // Çýkýþ yapýldýðýnda yönlendirilecek sayfa
        options.AccessDeniedPath = "/AccessDenied"; // Yetki yoksa yönlendirilecek sayfa
    });

// Authorization yapýlandýrmasý
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Veritabaný seed iþlemi
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        // Hata yönetimi, örneðin logging
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}





// Hata ve güvenlik yapýlandýrmasý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Kimlik doðrulama ve yetkilendirme middleware'lerini ekleyin
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
