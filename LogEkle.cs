using LoginEkrani.Models.Login;
using System;
using System.Threading.Tasks;

namespace LoginEkrani.Services
{
    public class LogService
    {
        private readonly Database _db;

        public LogService(Database db)
        {
            _db = db;
        }

        public async Task LogEkle(string kullaniciAdi, string sifre,string islemTipi)
        {
            var logEntry = new LogKayıtlarıModelDis
            {
                kpsft_kullaniciAdi = kullaniciAdi,
                kpsft_sifre = sifre,
                kpsft_zaman = DateTime.Now,
                kpsft_islemAdi = islemTipi
            };

            _db.kpsft_log_kayitlari_dis.Add(logEntry);
            await _db.SaveChangesAsync();
        }
    }
}
