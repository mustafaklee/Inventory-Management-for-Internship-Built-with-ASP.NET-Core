using LoginEkrani.Models;
using LoginEkrani.Models.Login;
using LoginEkrani.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Diagnostics;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using LoginEkrani.Models.Admin;
using Microsoft.AspNetCore.Authorization;
namespace LoginEkrani.Controllers.LoginController
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly Database _db;
        private readonly LogService _logService;

        public LoginController(ILogger<LoginController> logger, Database db, LogService logService)
        {
            _logger = logger;
            _db = db;
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> Register(string userName)
        {

            LoginPageModel model = new LoginPageModel();
            if (userName == null)
            {
                ViewBag.Roles = await _db.kpsft_roller.ToListAsync();
                return View(model);
            }
            else
            {
                ViewBag.Roles = await _db.kpsft_roller.ToListAsync();
                var user = await _db.kpsft_boskay.
                    Include(s => s.UserRoles).
                    ThenInclude(r=> r.Role).
                    FirstOrDefaultAsync(s => s.kpsft_kullaniciAdi == userName);
                user.kpsft_sifre = null;
                return View(user);
            }
           
        }


        [HttpPost]
        public async Task<IActionResult> Register(LoginPageModel model , List<int> selectedRoles)
        {
            var user = await _db.kpsft_boskay
                    .FirstOrDefaultAsync(s => s.kpsft_kullaniciAdi == model.kpsft_kullaniciAdi);


            if (user==null)
            {
                Sifrele sifrele = new Sifrele();
                model.kpsft_sifre = sifrele.HashPassword(model.kpsft_sifre);


                _db.kpsft_boskay.Add(model);
                await _db.SaveChangesAsync();

                LoginPageModel model2  = new LoginPageModel();

                model2 = await _db.kpsft_boskay
                    .FirstOrDefaultAsync(s => s.kpsft_kullaniciAdi == model.kpsft_kullaniciAdi);

                if (selectedRoles != null)
                {
                    foreach (var roleId in selectedRoles)
                    {
                        var userRole = new UserRole
                        {
                            id_kpsft = model2.id_kpsft, // Kullan�c� ID'si
                            kpsft_rol_id = roleId // Se�ilen Rol ID'si
                        };
                        _db.kpsft_kullan�c�_rolleri.Add(userRole);
                    }
                    await _db.SaveChangesAsync();
                }


                return RedirectToAction("Userlist");
            }
            else
            {
                if (selectedRoles != null)
                {
                    var EditUserRole = _db.kpsft_kullan�c�_rolleri.Where(e => e.id_kpsft == user.id_kpsft).ToList();

                    // Remove all matching rows
                    _db.kpsft_kullan�c�_rolleri.RemoveRange(EditUserRole);

                    foreach (var roleId in selectedRoles)
                    {
                        var userRole = new UserRole
                        {
                            id_kpsft = user.id_kpsft, // Kullan�c� ID'si
                            kpsft_rol_id = roleId // Se�ilen Rol ID'si
                        };
                        _db.kpsft_kullan�c�_rolleri.Add(userRole);
                    }
                    await _db.SaveChangesAsync();
                }



                user.kpsft_kullaniciAdi = model.kpsft_kullaniciAdi;
                user.kpsft_ad = model.kpsft_ad;
                user.kpsft_soyad = model.kpsft_soyad;
                user.kpsft_mailAdrress = model.kpsft_mailAdrress;
                user.kpsft_tcKimlik = model.kpsft_tcKimlik;

                Sifrele sifrele = new Sifrele();
                user.kpsft_sifre = sifrele.HashPassword(model.kpsft_sifre);

                _db.kpsft_boskay.Update(user);
                await _db.SaveChangesAsync();

                return RedirectToAction("Userlist");
            }
        }



        [HttpGet]
        public async Task <IActionResult> UserList()
        {
            List<LoginPageModel> model = await _db.kpsft_boskay
                .Include(s => s.UserRoles)
                .ThenInclude(r => r.Role)
                .ToListAsync();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UserList(LoginPageModel model)
        {
            Sifrele sifrele = new Sifrele();
            model.kpsft_sifre = sifrele.HashPassword(model.kpsft_sifre);
            _db.kpsft_boskay.Add(model);
            await _db.SaveChangesAsync();

            return View();
        }


        public async Task<IActionResult> Sil(string userName)
        {
            var User = _db.kpsft_boskay.FirstOrDefault(e => e.kpsft_kullaniciAdi == userName);
            _db.kpsft_boskay.Remove(User);
            await _db.SaveChangesAsync();
            return RedirectToAction("UserList");
        }







        [HttpPost]
        public async Task<IActionResult> SendVerificationCode(string kpsft_mailAdrress)
        {

            var user = await _db.kpsft_boskay
                .FirstOrDefaultAsync(u => u.kpsft_mailAdrress == kpsft_mailAdrress);

            if (user == null)
            {
                ViewBag.Hata = "Bu e-posta adresi do�rulanamad�.";
                return View("InputMail", "Login");
            }

            var verificationCode = GenerateVerificationCode();

            HttpContext.Session.SetString("VerificationCode", verificationCode);
            HttpContext.Session.SetString("EmailAddress", kpsft_mailAdrress);

            SendVerificationCode(kpsft_mailAdrress, verificationCode);

            // Do�rulama kodu g�nderildi�inde formu a�ar
            return View("ResetPassword");
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpGet]
        public IActionResult InputMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string girilenKod, string newPassword)
        {
            var expectedCode = HttpContext.Session.GetString("VerificationCode");
            var email = HttpContext.Session.GetString("EmailAddress");

            if (girilenKod == expectedCode)
            {

                Sifrele sifrele = new Sifrele();

                var user = await _db.kpsft_boskay
                    .FirstOrDefaultAsync(u => u.kpsft_mailAdrress == email);

                user.kpsft_sifre = sifrele.HashPassword(newPassword);
                await _db.SaveChangesAsync();

                await _logService.LogEkle(
                user.kpsft_kullaniciAdi,
                user.kpsft_sifre,
                "Ba�ar�l� �ifre De�i�ikli�i"
                );
                return RedirectToAction("Index", "Login", new { email });
            }
            else
            {
                ViewBag.Hata = "Do�rulama kodu hatal�.";
                await _logService.LogEkle(
                    email,
                    newPassword,
                    "Ba�ar�s�z �ifre De�i�ikli�i"
                );
                return RedirectToAction("ResetPassword", "Login");
            }

        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void SendVerificationCode(string kpsft_mailAdrress, string code)
        {
            MimeMessage mimeMessage = new MimeMessage();

            MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "mustafa12kale@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", kpsft_mailAdrress);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = code;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            mimeMessage.Subject = "Do�rulama Kodu";

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("mustafa12kale@gmail.com", "goym fawb fvkg ihfa");
                client.Send(mimeMessage);
                client.Disconnect(true);
            }
        }

        public IActionResult Index()
        {

            var model = new LoginPageModel();
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginPageModel model)
        {
            Sifrele sifrele = new Sifrele();

            var user = await _db.kpsft_boskay
                .Include(u => u.UserRoles) // Kullan�c�n�n rollerini i�eren ili�kileri de dahil et
                .ThenInclude(ur => ur.Role) // UserRole tablosundaki Role ili�kisini dahil et
                .FirstOrDefaultAsync(u => u.kpsft_kullaniciAdi == model.kpsft_kullaniciAdi);

            if (user != null)
            {
                // �ifreleri kar��la�t�r
                if (sifrele.VerifyPassword(model.kpsft_sifre, user.kpsft_sifre))
                {
                    await _logService.LogEkle(
                        model.kpsft_kullaniciAdi,
                        model.kpsft_sifre,
                        "Ba�ar�l� Giri� ��lemi");

                    // Claims ve Authentication
                    var claims = new List<Claim>();

                    // Kullan�c�n�n rollerini de ekle
                    foreach (var userRole in user.UserRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole.Role.kpsft_rol));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20), // S�reyi uzat
                            IsPersistent = true
                        });

                    return RedirectToAction("AdminIndex", "Admin");
                }
            }

            await _logService.LogEkle(
                model.kpsft_kullaniciAdi,
                model.kpsft_sifre,
                "Ba�ar�s�z Giri� ��lemi");

            ViewBag.Hata = "Kullan�c� bilgileri bulunamad�.";
            model.kpsft_kullaniciAdi = "";
            model.kpsft_sifre = "";

            return View("Index", model);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
