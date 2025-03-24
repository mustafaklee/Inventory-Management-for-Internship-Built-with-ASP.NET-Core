using Microsoft.EntityFrameworkCore;
using LoginEkrani;
using LoginEkrani.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using LoginEkrani.Models.Login.Roles;

var builder = WebApplication.CreateBuilder(args);

// LogService'i DI container'a ekleyin
builder.Services.AddScoped<LogService>();

// Controller ve View deste�ini ekleyin
builder.Services.AddControllersWithViews();

// Session deste�ini ekleyin
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Oturum s�resi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuration nesnesine eri�im
var configuration = builder.Configuration;

// DbContext'i ekleyin ve MySQL ba�lant� dizesini kullan�n
builder.Services.AddDbContext<Database>(options =>
    options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// Kimlik do�rulama i�in Cookie Authentication ekleyin
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Giri� yap�lmad���nda y�nlendirilecek sayfa
        options.LogoutPath = "/Logout"; // ��k�� yap�ld���nda y�nlendirilecek sayfa
        options.AccessDeniedPath = "/AccessDenied"; // Yetki yoksa y�nlendirilecek sayfa
    });

// Authorization yap�land�rmas�
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
        // Veritaban� seed i�lemi
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        // Hata y�netimi, �rne�in logging
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}





// Hata ve g�venlik yap�land�rmas�
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Kimlik do�rulama ve yetkilendirme middleware'lerini ekleyin
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
