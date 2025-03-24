using LoginEkrani.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient.Memcached;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace LoginEkrani.Controllers
{
       public class AdminController : Controller
        {
        private readonly ILogger<AdminController> _logger;
        private readonly Database _db;
        private string? enStr;

        public AdminController(ILogger<AdminController> logger, Database db)
        {
            _logger = logger;
            _db = db;
        }

        [Authorize(Roles="Admin")]
        [HttpGet]
         
        public async Task<IActionResult> YeniCariEkle()
        {
            CariGrupModel model = new CariGrupModel();
            model.cariModel = new CariModel();
            model.vergidairesiModel = await _db.kpsft_vergidairesi.ToListAsync();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> YeniCariEkle(CariGrupModel model)
        {

            _db.kpsft_cariler.Add(model.cariModel);
            await _db.SaveChangesAsync();
            return RedirectToAction("CariTanım");
        }

        //[Authorize(Roles ="Moderator,Admin")]
        [HttpGet]
        public async Task<IActionResult> YeniDepoEkle()
        {
            ViewBag.DepoTurler = _db.kpsft_depo_turu.ToList();
            ViewBag.DepoSorumlusu = _db.kpsft_boskay.ToList();
            DepoModel model = new DepoModel();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> YeniDepoEkle(DepoModel model, int kpsft_depoTuru)
        {
            model.kpsft_depoTuru = kpsft_depoTuru;

            _db.kpsft_depobilgisi.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("DepoTanım"); 
        }


        [HttpGet]
        public async Task<IActionResult> StokHareketleriMain(StokKartıModel model)
        {

            var stokKartlar = await _db.kpsft_stok_karti
            .Include(s => s.durum)               // DurumModel ilişkisini dahil et
            .Include(s => s.birimModel)          // BirimModel ilişkisini dahil et
            .Include(s => s.grupModel)           // GrupModel ilişkisini dahil et
            .Include(s => s.gumrukKoduModel)     // GumrukKoduModel ilişkisini dahil et
            .Include(s => s.kaliteModel)         // KaliteModel ilişkisini dahil et
            .Include(s => s.satisKdvOrani)       // SatisKdvOrani ilişkisini dahil et
            .Include(s => s.alisKdvOraniModel)   // AlisKdvOraniModel ilişkisini dahil et
            .Include(s => s.stokTipi)            // StokTipi ilişkisini dahil et
            .ToListAsync();


            return View(stokKartlar);
        }

        [HttpGet]
        public async Task<IActionResult> StokHareketleri(string stokKodu)
        {
            ViewBag.depolar = await _db.kpsft_depobilgisi.ToListAsync();
            ViewBag.cariler = await _db.kpsft_cariler.ToListAsync();

            var hareketler = await _db.kpsft_fisharaketi_cari
                .Include(f => f.StokKartı)
                    .ThenInclude(s => s.birimModel)
                .Include(f => f.fistablocariModel)
                    .ThenInclude(f => f.Cariislem)
                .Include(f => f.fistablocariModel)
                    .ThenInclude(f => f.Depo)
                .Where(a => a.kpsft_StokKod == stokKodu)
                .OrderBy(a => a.fistablocariModel.kpsft_tarih) 
                .ToListAsync();

            var stokGuncelMiktarlar = new List<int>();


            foreach (var hareket in hareketler)
            {
                var oncekiHareketler = hareketler
                    .Where(h =>
                        h.fistablocariModel.kpsft_depo == hareket.fistablocariModel.kpsft_depo && 
                        (h.fistablocariModel.kpsft_tarih < hareket.fistablocariModel.kpsft_tarih || 
                         (h.fistablocariModel.kpsft_tarih == hareket.fistablocariModel.kpsft_tarih && h.kpsft_FisHaraketiID < hareket.kpsft_FisHaraketiID))) 
                    .ToList();

                hareket.kpsft_Miktar = hareket.fistablocariModel.kpsft_FisTuru == "Giriş Fişi" ? hareket.kpsft_Miktar : - hareket.kpsft_Miktar;
               
                    int depoGuncelMiktar = oncekiHareketler.Any() ? oncekiHareketler.Sum(h => h.kpsft_Miktar) : 0 ;

                depoGuncelMiktar += hareket.kpsft_Miktar;


                stokGuncelMiktarlar.Add(depoGuncelMiktar);
            }

            ViewBag.StokGuncelMiktarlar = stokGuncelMiktarlar;

            var model = new FisTabloModel
            {
                FisHaraketleriCari = hareketler,
            };

            return View(model);
        }








        [HttpGet]
        public IActionResult AdminIndex()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> StokKartiDuzenle (string stokKodu)
        {

            MultipleModel model = new MultipleModel();

                // Eğer bu başarılı oluyorsa, ardından tek tek Include ekleyerek sorunun kaynağını bulabiliriz.

                // Stok koduna ait veriyi bulup modelin StokKartıModel kısmına atama yapıyoruz
                model.StokKartıModel = await _db.kpsft_stok_karti
                   .Include(s => s.alisKdvOraniModel)
                   .Include(sh => sh.birimModel)
                   .Include(sc => sc.grupModel)
                   .Include(sm => sm.gumrukKoduModel)
                   .Include(sk => sk.kaliteModel)
                   .Include(sl => sl.satisKdvOrani)
                   .Include(so => so.stokTipi)
                   .Include(sn => sn.durum)
                   .FirstOrDefaultAsync(sa => sa.kpsft_stokKodu == stokKodu);

                model.BirimModel = await _db.kpsft_birimler.ToListAsync();
                model.AlisKdvOraniModel = await _db.kpsft_alis_kdv_orani.ToListAsync();
                model.SatisKdvOraniModel = await _db.kpsft_satis_kdv_orani.ToListAsync();
                model.GrupModel = await _db.kpsft_grup_kodu.ToListAsync();
                model.GumrukKoduModel = await _db.kpsft_gumruk_kodu.ToListAsync();
                model.KaliteModel = await _db.kpsft_kalite.ToListAsync();
                model.StokTipiModel = await _db.kpsft_stoktipi.ToListAsync();


                return View(model);
            

        }

        [HttpPost]
        public async Task<IActionResult> StokKartiDuzenle(StokKartıModel model)
        {
           
                // Veritabanında mevcut olan kaydı buluyoruz
                var mevcutStokKarti = await _db.kpsft_stok_karti
                    .FirstOrDefaultAsync(s => s.kpsft_stokKodu == model.kpsft_stokKodu);

            if (mevcutStokKarti != null)
            {
                mevcutStokKarti.kpsft_stokKodu = model.kpsft_stokKodu;
                mevcutStokKarti.kpsft_stokAdi = model.kpsft_stokAdi;
                mevcutStokKarti.kpsft_birim = model.kpsft_birim;
                mevcutStokKarti.kpsft_grupKodu = model.kpsft_grupKodu;
                mevcutStokKarti.kpsft_aciklama = model.kpsft_aciklama;
                mevcutStokKarti.kpsft_durum = model.kpsft_durum;
                mevcutStokKarti.kpsft_stokKartTipi = model.kpsft_stokKartTipi;
                mevcutStokKarti.kpsft_alisKdvOrani = model.kpsft_alisKdvOrani;
                mevcutStokKarti.kpsft_satisKdvOrani = model.kpsft_satisKdvOrani;
                mevcutStokKarti.kpsft_kalite = model.kpsft_kalite;
                mevcutStokKarti.kpsft_kritikMiktar = model.kpsft_kritikMiktar;
                mevcutStokKarti.kpsft_lotBuyuklugu = model.kpsft_lotBuyuklugu;
                mevcutStokKarti.kpsft_gtipKodu = model.kpsft_gtipKodu;
                mevcutStokKarti.kpsft_boyut1 = model.kpsft_boyut1;
                mevcutStokKarti.kpsft_boyut2 = model.kpsft_boyut2;
                mevcutStokKarti.kpsft_boyut3 = model.kpsft_boyut3;
                mevcutStokKarti.kpsft_boyut4 = model.kpsft_boyut4;
                mevcutStokKarti.kpsft_boyut5 = model.kpsft_boyut5;
                mevcutStokKarti.kpsft_uretim_parti_buyuklugu = model.kpsft_uretim_parti_buyuklugu;


                _db.kpsft_stok_karti.Update(mevcutStokKarti);
                await _db.SaveChangesAsync();
            }else
            {
                _db.kpsft_stok_karti.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("StokKartıTanım");
            }

            return RedirectToAction("StokKartıTanım");
        }



        [HttpGet]
        public async Task<IActionResult> DepoKartiDuzenle(string depoKodu)
        {
            ViewBag.DepoTurler = ViewBag.DepoTurler = _db.kpsft_depo_turu.ToList();
            DepoModel model = new DepoModel();
            model = await _db.kpsft_depobilgisi
               .FirstOrDefaultAsync(sa => sa.kpsft_depoKod == depoKodu);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DepoKartiDuzenle(DepoModel model)
        {

            // Veritabanında mevcut olan kaydı buluyoruz
            var mevcutDepoKarti = await _db.kpsft_depobilgisi
                .FirstOrDefaultAsync(s => s.kpsft_depoKod == model.kpsft_depoKod);

            if (mevcutDepoKarti != null)
            {
                mevcutDepoKarti.kpsft_depoKod = model.kpsft_depoKod;
                mevcutDepoKarti.kpsft_depoAd = model.kpsft_depoAd;
                mevcutDepoKarti.kpsft_depoTuru = model.kpsft_depoTuru;
                mevcutDepoKarti.kpsft_durum = model.kpsft_durum;
                mevcutDepoKarti.kpsft_genislik = model.kpsft_genislik;
                mevcutDepoKarti.kpsft_yukseklik = model.kpsft_yukseklik;
                mevcutDepoKarti.kpsft_uzunluk = model.kpsft_uzunluk;
                mevcutDepoKarti.kpsft_alan = model.kpsft_alan;
                mevcutDepoKarti.kpsft_depoSorumlusu = model.kpsft_depoSorumlusu;
                mevcutDepoKarti.kpsft_aciklama = model.kpsft_aciklama;

                _db.kpsft_depobilgisi.Update(mevcutDepoKarti);
                await _db.SaveChangesAsync();
            }
            else
            {
                _db.kpsft_depobilgisi.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("DepoTanım");
            }

            return RedirectToAction("DepoTanım");
        }


        [HttpGet]
        public async Task<IActionResult> CariDuzenle(string cariKodu)
        {
            CariGrupModel model = new CariGrupModel();
            model.cariModel = await _db.kpsft_cariler
               .FirstOrDefaultAsync(sa => sa.kpsft_cariKod == cariKodu);

            model.vergidairesiModel = await _db.kpsft_vergidairesi.ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CariDuzenle(CariGrupModel model1)
        {

            var mevcutCariKarti = await _db.kpsft_cariler
                .FirstOrDefaultAsync(s => s.kpsft_cariKod == model1.cariModel.kpsft_cariKod);
            var model = model1.cariModel;
            if (mevcutCariKarti != null)
            {
                mevcutCariKarti.kpsft_cariKod = model.kpsft_cariKod;
                mevcutCariKarti.kpsft_cariAd = model.kpsft_cariAd;
                mevcutCariKarti.kpsft_cariTipi = model.kpsft_cariTipi;
                mevcutCariKarti.kpsft_vergiNo = model.kpsft_vergiNo;
                mevcutCariKarti.kpsft_vergiDairesi = model.kpsft_vergiDairesi;
                mevcutCariKarti.kpsft_adres1 = model.kpsft_adres1;
                mevcutCariKarti.kpsft_adres2 = model.kpsft_adres2;
                mevcutCariKarti.kpsft_cariAd2 = model.kpsft_cariAd2;
                mevcutCariKarti.kpsft_adres3 = model.kpsft_adres3;
                mevcutCariKarti.kpsft_telefon1 = model.kpsft_telefon1;
                mevcutCariKarti.kpsft_telefon2 = model.kpsft_telefon2;
                mevcutCariKarti.kpsft_email = model.kpsft_email;
                mevcutCariKarti.kpsft_ıban = model.kpsft_ıban;
                mevcutCariKarti.kpsft_aciklama = model.kpsft_aciklama;
                mevcutCariKarti.kpsft_yurticiDisi = model.kpsft_yurticiDisi;
                mevcutCariKarti.kpsft_web_sitesi = model.kpsft_web_sitesi;
                mevcutCariKarti.kpsft_durum = model.kpsft_durum;
                mevcutCariKarti.kpsft_fason_uretim = model.kpsft_fason_uretim;



                _db.kpsft_cariler.Update(mevcutCariKarti);
                await _db.SaveChangesAsync();
            }
            else
            {
                _db.kpsft_cariler.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("CariTanım");
            }

            return RedirectToAction("CariTanım");
        }



        [HttpGet]
        public async Task<IActionResult> StokKartiEkle()
        {

            MultipleModel model = new MultipleModel();

                model.BirimModel = await _db.kpsft_birimler.ToListAsync();
                model.AlisKdvOraniModel = await _db.kpsft_alis_kdv_orani.ToListAsync();
                model.SatisKdvOraniModel = await _db.kpsft_satis_kdv_orani.ToListAsync();
                model.GrupModel = await _db.kpsft_grup_kodu.ToListAsync();
                model.GumrukKoduModel = await _db.kpsft_gumruk_kodu.ToListAsync();
                model.KaliteModel = await _db.kpsft_kalite.ToListAsync();
                model.StokTipiModel = await _db.kpsft_stoktipi.ToListAsync();
                model.StokKartıModel = new StokKartıModel();

            
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> StokKartiEkle(StokKartıModel model)
        {
            _db.kpsft_stok_karti.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("StokKartıTanım");
        }

        public async Task<IActionResult> depoKartıSil(string depoKodu)
        {
            var relatedStokItems = _db.kpsft_depobilgisi.Where(s => s.kpsft_depoKod == depoKodu);
            _db.kpsft_depobilgisi.RemoveRange(relatedStokItems);
            await _db.SaveChangesAsync();
            return RedirectToAction("DepoTanım");
        }


        public async Task<IActionResult> cariKartiSil(string cariKodu)
        {
            var relatedCariItems = _db.kpsft_cariler.Where(s => s.kpsft_cariKod == cariKodu);
            _db.kpsft_cariler.RemoveRange(relatedCariItems);
            await _db.SaveChangesAsync();
            return RedirectToAction("CariTanım");
        }


        public async Task<IActionResult> StokKartıSil(string StokKodu)
        {
            var relatedStokItems = _db.kpsft_stok_karti.Where(s => s.kpsft_stokKodu == StokKodu);
            _db.kpsft_stok_karti.RemoveRange(relatedStokItems);
            //var StokK = _db.kpsft_stok_karti.FirstOrDefault(e => e.kpsft_stokKodu == StokKodu);
            //StokK.kpsft_durum = 0;
            await _db.SaveChangesAsync();
            return RedirectToAction("StokKartıTanım");
        }
        public async Task<IActionResult> StokKartiIncele (string StokKodu)
        {
            MultipleModel model = new MultipleModel();
            model.StokKartıModel = await _db.kpsft_stok_karti
               .Include(s => s.alisKdvOraniModel)
               .Include(sh => sh.birimModel)
               .Include(sc => sc.grupModel)
               .Include(sm => sm.gumrukKoduModel)
               .Include(sk => sk.kaliteModel)
               .Include(sl => sl.satisKdvOrani)
               .Include(so => so.stokTipi)
               .Include(sn => sn.durum)
               .FirstOrDefaultAsync(sa => sa.kpsft_stokKodu == StokKodu);

            model.BirimModel = await _db.kpsft_birimler.ToListAsync();
            model.AlisKdvOraniModel = await _db.kpsft_alis_kdv_orani.ToListAsync();
            model.SatisKdvOraniModel = await _db.kpsft_satis_kdv_orani.ToListAsync();
            model.GrupModel = await _db.kpsft_grup_kodu.ToListAsync();
            model.GumrukKoduModel = await _db.kpsft_gumruk_kodu.ToListAsync();
            model.KaliteModel = await _db.kpsft_kalite.ToListAsync();
            model.StokTipiModel = await _db.kpsft_stoktipi.ToListAsync();


            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> StokKartıTanım(StokKartıModel model)
        {

            var stokKartlar = await _db.kpsft_stok_karti
            .Include(s => s.durum)               // DurumModel ilişkisini dahil et
            .Include(s => s.birimModel)          // BirimModel ilişkisini dahil et
            .Include(s => s.grupModel)           // GrupModel ilişkisini dahil et
            .Include(s => s.gumrukKoduModel)     // GumrukKoduModel ilişkisini dahil et
            .Include(s => s.kaliteModel)         // KaliteModel ilişkisini dahil et
            .Include(s => s.satisKdvOrani)       // SatisKdvOrani ilişkisini dahil et
            .Include(s => s.alisKdvOraniModel)   // AlisKdvOraniModel ilişkisini dahil et
            .Include(s => s.stokTipi)            // StokTipi ilişkisini dahil et
            .ToListAsync();


            return View(stokKartlar);
        }

        [HttpGet]
        public async Task<IActionResult> DepoTanım(DepoModel model)
        {

            var depoBilgisi = await _db.kpsft_depobilgisi.
                Include(s => s.depoTuru)
                .ToListAsync();

            return View(depoBilgisi);
        }

        [HttpGet]
        public async Task<IActionResult> CariTanım(DepoModel model)
        {

            var cariBilgisi = await _db.kpsft_cariler
                .ToListAsync();

            return View(cariBilgisi);
        }



        [HttpGet]
        public async Task<IActionResult> GirisFisiTanım(DoubleModel model)
        {
            model.FisTabloCariModelList = await _db.kpsft_fistablo_cari.
                Where(f=> f.kpsft_FisTuru=="Giriş Fişi")
                .Include(f => f.FisHaraketleri)
                    .ThenInclude(fh => fh.StokKartı)
                        .ThenInclude(fs => fs.birimModel)
                .Include(f => f.Cariislem)
                .Include(f => f.Depo)
                .ThenInclude(fm => fm.depoTuru).
            ToListAsync();  


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CikisFisiTanım()
        {
            DoubleModel model = new DoubleModel();
            model.FisTabloCariModelList = await _db.kpsft_fistablo_cari.
                Where(f => f.kpsft_FisTuru == "Çıkış Fişi")
                .Include(f => f.FisHaraketleri)
                    .ThenInclude(fh => fh.StokKartı)
                        .ThenInclude(fs => fs.birimModel)
                .Include(f => f.Cariislem)
                .Include(f => f.Depo)
                .ThenInclude(fm => fm.depoTuru).
            ToListAsync();


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> MalKabulList(string fisno)
        {
            DoubleModel model = new DoubleModel();

            model.formHareketiModel = await _db.kpsft_form_haraketi
            .Include(fh => fh.stokKartıModel)
                .ThenInclude(f => f.birimModel) 
            .Include(fh => fh.stokKartıModel)
                .ThenInclude(f => f.kaliteModel) 
            .ToListAsync();



            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> KutukSiparisListesi()
        {
            DoubleModel model = new DoubleModel();
            model.formTabloModelList = await _db.kpsft_formtablo.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FORM()
        {
            var stokKartlar = await _db.kpsft_stok_karti
            .Include(s => s.durum)               // DurumModel ilişkisini dahil et
            .Include(s => s.birimModel)          // BirimModel ilişkisini dahil et
            .Include(s => s.grupModel)           // GrupModel ilişkisini dahil et
            .Include(s => s.gumrukKoduModel)     // GumrukKoduModel ilişkisini dahil et
            .Include(s => s.kaliteModel)         // KaliteModel ilişkisini dahil et
            .Include(s => s.satisKdvOrani)       // SatisKdvOrani ilişkisini dahil et
            .Include(s => s.alisKdvOraniModel)   // AlisKdvOraniModel ilişkisini dahil et
            .Include(s => s.stokTipi)            // StokTipi ilişkisini dahil et
            .ToListAsync();


            var model = new DoubleModel
            {
                StokKartıModel = stokKartlar,
                formTabloModel = new FormTabloModel(),
                formHareketiModel = new List<FormHareketiModel>(),
                cariModel = await _db.kpsft_cariler.
                ToListAsync(),
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> FORMM([FromBody] JsonForm json)
        {
            if (ModelState.IsValid)
            {
                FormTabloModel model = new FormTabloModel
                {
                    kpsft_evrakNo = json.kpsft_evrakNo,
                    kpsft_evrak_tarih = json.kpsft_evrak_tarih,
                    kpsft_teslim_tarih = json.kpsft_teslim_tarih,
                    kpsft_cari = json.kpsft_cari,
                    kpsft_aciklama = json.kpsft_aciklama,
                    kpsft_satici_notu = json.kpsft_satici_notu,
                    kpsft_paraBirim = json.kpsft_paraBirim
                };


                _db.kpsft_formtablo.Add(model);

                List<FormHareketiModel> formHareketleri = new List<FormHareketiModel>();
                foreach (var data in json.datas)
                {
                    FormHareketiModel formH = new FormHareketiModel
                    {
                        kpsft_stokKod = data.kpsft_stokKod,
                        kpsft_Miktar = data.kpsft_Miktar,
                        kpsft_birimFiyat = data.kpsft_birimFiyat,
                        kpsft_kdv = data.kpsft_kdv,
                        kpsft_tutar = data.kpsft_tutar,
                        kpsft_gemi = data.kpsft_gemi,
                        kpsft_evrakNo = json.kpsft_evrakNo
                    };

                    formHareketleri.Add(formH);
                }

                _db.kpsft_form_haraketi.AddRange(formHareketleri);

                try
                {
                    await _db.SaveChangesAsync();

                    return Ok(new { success = true, message = "İşlem başarılı." });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = "Bir hata oluştu.", error = ex.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }




        [HttpGet]
        public async Task<IActionResult> CariGirisFisiEkle(string evrakNo,string stokKod)
        {
            var lastFisNo = _db.kpsft_fistablo_cari
                   .Where(f => f.kpsft_FisTuru == "Giriş Fişi")
                   .OrderByDescending(f => f.kpsft_FisNo)
                   .Select(f => f.kpsft_FisNo)
                   .FirstOrDefault();

            string newFisNo = "FG001"; // Varsayılan başlangıç numarası

            if (lastFisNo != null)
            {
                // Fi̇ş numarasını ayırın ve sayısal kısmı artırın
                var prefix = lastFisNo.Substring(0, 2); // 'FG' gibi prefix
                var numberPart = lastFisNo.Substring(2); // '001' gibi sayı kısmı

                if (int.TryParse(numberPart, out int lastNumber))
                {
                    newFisNo = $"{prefix}{(lastNumber + 1).ToString("D3")}";
                }
                else
                {
                    // Sayısal kısım uygun formatta değilse, uygun hata işleme yapın
                }
            }

            // ViewBag'e yeni fiş numarasını atama
            ViewBag.cariFis2 = newFisNo;



            DoubleModel model;

            if (!string.IsNullOrEmpty(evrakNo) && !string.IsNullOrEmpty(stokKod))
            {
                model = new DoubleModel
                {
                    fistablocariModel = new FisTabloCariModel(),
                    FisHaraketleriModelCari = new List<FisHaraketleriCariModel>(),
                    formharaketimodel = await _db.kpsft_form_haraketi
                        .Include(fh => fh.stokKartıModel)
                            .ThenInclude(f => f.birimModel)
                        .Include(fh => fh.stokKartıModel)
                            .ThenInclude(f => f.kaliteModel)
                        .Where(f => f.kpsft_evrakNo == evrakNo && f.kpsft_stokKod == stokKod)
                        .FirstOrDefaultAsync(), // Asenkron sorgu
                    formTabloModel = await _db.kpsft_formtablo
                        .Where(f => f.kpsft_evrakNo == evrakNo)
                        .Include(f => f.cari)
                        .FirstOrDefaultAsync(), // Asenkron sorgu
                    cariModel = await _db.kpsft_cariler.ToListAsync(), // Asenkron sorgu
                    depoModel = await _db.kpsft_depobilgisi
                        .Include(m => m.depoTuru)
                        .ToListAsync() // Asenkron sorgu
                };
            }
            else
            {
                var stokKartlar = await _db.kpsft_stok_karti
                    .Include(s => s.durum)               // DurumModel ilişkisini dahil et
                    .Include(s => s.birimModel)          // BirimModel ilişkisini dahil et
                    .Include(s => s.grupModel)           // GrupModel ilişkisini dahil et
                    .Include(s => s.gumrukKoduModel)     // GumrukKoduModel ilişkisini dahil et
                    .Include(s => s.kaliteModel)         // KaliteModel ilişkisini dahil et
                    .Include(s => s.satisKdvOrani)       // SatisKdvOrani ilişkisini dahil et
                    .Include(s => s.alisKdvOraniModel)   // AlisKdvOraniModel ilişkisini dahil et
                    .Include(s => s.stokTipi)            // StokTipi ilişkisini dahil et
                    .ToListAsync(); // Asenkron sorgu

                model = new DoubleModel
                {
                    StokKartıModel = stokKartlar,
                    fistablocariModel = new FisTabloCariModel(),
                    FisHaraketleriModelCari = new List<FisHaraketleriCariModel>(),
                    cariModel = await _db.kpsft_cariler.ToListAsync(), // Asenkron sorgu
                    depoModel = await _db.kpsft_depobilgisi
                        .Include(m => m.depoTuru)
                        .ToListAsync() // Asenkron sorgu
                };
            }

            // Model'i View'a döndür
            return View(model);


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CariGirisFisiEkle([FromBody] JsonStok json)
        {
            if (ModelState.IsValid)
            {
                FisTabloCariModel model = new FisTabloCariModel
                {
                    kpsft_FisTuru = "Giriş Fişi",
                    kpsft_FisNo = json.FisNo,
                    kpsft_tarih = json.Tarih,
                    kpsft_islemKaynagiCari = json.cari,
                    kpsft_depo = json.depo
                };

                _db.kpsft_fistablo_cari.Add(model);

                List<FisHaraketleriCariModel> fisHareketleri = new List<FisHaraketleriCariModel>();
                foreach (var datum in json.Data)
                {
                    FisHaraketleriCariModel fisH = new FisHaraketleriCariModel
                    {
                        kpsft_Miktar = datum.kpsft_Miktar,
                        kpsft_StokKod = datum.kpsft_StokKod,
                        kpsft_FisNo = json.FisNo
                    };
                    fisHareketleri.Add(fisH);
                }

                _db.kpsft_fisharaketi_cari.AddRange(fisHareketleri);

                try
                {
                    await _db.SaveChangesAsync();

                    return Ok(new { success = true, message = "İşlem başarılı." });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = "Bir hata oluştu.", error = ex.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }






        [HttpGet]
        public async Task<IActionResult> CariGirisFisiDuzenle(string fisno)
        {
            var stokKartlar = await _db.kpsft_stok_karti
            .Include(s => s.durum)               // DurumModel ilişkisini dahil et
            .Include(s => s.birimModel)          // BirimModel ilişkisini dahil et
            .Include(s => s.grupModel)           // GrupModel ilişkisini dahil et
            .Include(s => s.gumrukKoduModel)     // GumrukKoduModel ilişkisini dahil et
            .Include(s => s.kaliteModel)         // KaliteModel ilişkisini dahil et
            .Include(s => s.satisKdvOrani)       // SatisKdvOrani ilişkisini dahil et
            .Include(s => s.alisKdvOraniModel)   // AlisKdvOraniModel ilişkisini dahil et
            .Include(s => s.stokTipi)            // StokTipi ilişkisini dahil et
            .ToListAsync();


            var model = new DoubleModel
            {
                StokKartıModel = stokKartlar,
                fistablocariModel = await _db.kpsft_fistablo_cari.Where(m=> m.kpsft_FisNo == fisno).FirstOrDefaultAsync(),
                FisHaraketleriModelCari = await _db.kpsft_fisharaketi_cari.Where(m => m.kpsft_FisNo == fisno).ToListAsync(), 
                cariModel = await _db.kpsft_cariler.ToListAsync(),
                depoModel = await _db.kpsft_depobilgisi.Include(m => m.depoTuru)
                .ToListAsync()
            };

            return View(model);
        }





        [HttpPost]
        public async Task<IActionResult> CariGirisFisiDuzenle([FromBody] JsonStok json)
        {
            if (ModelState.IsValid)
            {
                var currentFis = await _db.kpsft_fistablo_cari
                    .Where(s => s.kpsft_FisNo == json.FisNo)
                    .FirstOrDefaultAsync();

                if (currentFis == null)
                {
                    return NotFound(new { success = false, message = "Mevcut fiş bulunamadı." });
                }
                else
                {
                    currentFis.kpsft_tarih = json.Tarih;
                    currentFis.kpsft_islemKaynagiCari = json.cari;
                    currentFis.kpsft_depo = json.depo;

                    var mevcutHareketler = await _db.kpsft_fisharaketi_cari
                        .Where(h => h.kpsft_FisNo == json.FisNo)
                        .ToListAsync();

                    List<FisHaraketleriCariModel> yeniHareketler = new List<FisHaraketleriCariModel>();

                    foreach (var datum in json.Data)
                    {
                        // Aynı StokKod ve FisNo'ya sahip bir hareket varsa
                        var mevcutHareket = mevcutHareketler
                            .FirstOrDefault(h => h.kpsft_StokKod == datum.kpsft_StokKod);

                        if (mevcutHareket != null)
                        {
                            // Miktarı güncelle
                            mevcutHareket.kpsft_Miktar = datum.kpsft_Miktar;

                            // Bu hareket işlendiği için listeden çıkarıyoruz (geriye kalanlar silinecekler)
                            mevcutHareketler.Remove(mevcutHareket);
                        }
                        else
                        {
                            // Eğer aynı stok yoksa yeni hareket olarak ekliyoruz
                            yeniHareketler.Add(new FisHaraketleriCariModel
                            {
                                kpsft_FisNo = json.FisNo,
                                kpsft_StokKod = datum.kpsft_StokKod,
                                kpsft_Miktar = datum.kpsft_Miktar
                            });
                        }
                    }

                    // Veritabanına yeni eklenen hareketleri ekliyoruz
                    _db.kpsft_fisharaketi_cari.AddRange(yeniHareketler);

                    // Geride kalan hareketleri, JSON verisinde bulunmadıkları için silinmesi gerekenler olarak düşünüyoruz
                    _db.kpsft_fisharaketi_cari.RemoveRange(mevcutHareketler);

                    try
                    {
                        await _db.SaveChangesAsync();
                        return Ok(new { success = true, message = "İşlem başarılı." });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new { success = false, message = "Bir hata oluştu.", error = ex.Message });
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }





        [HttpGet]
        public async Task<IActionResult> CariCikisFisiDuzenle(string fisno)
        {
            var stokKartlar = await _db.kpsft_stok_karti
            .Include(s => s.durum)               // DurumModel ilişkisini dahil et
            .Include(s => s.birimModel)          // BirimModel ilişkisini dahil et
            .Include(s => s.grupModel)           // GrupModel ilişkisini dahil et
            .Include(s => s.gumrukKoduModel)     // GumrukKoduModel ilişkisini dahil et
            .Include(s => s.kaliteModel)         // KaliteModel ilişkisini dahil et
            .Include(s => s.satisKdvOrani)       // SatisKdvOrani ilişkisini dahil et
            .Include(s => s.alisKdvOraniModel)   // AlisKdvOraniModel ilişkisini dahil et
            .Include(s => s.stokTipi)            // StokTipi ilişkisini dahil et
            .ToListAsync();


            var model = new DoubleModel
            {
                StokKartıModel = stokKartlar,
                fistablocariModel = await _db.kpsft_fistablo_cari.Where(m => m.kpsft_FisNo == fisno).FirstOrDefaultAsync(),
                FisHaraketleriModelCari = await _db.kpsft_fisharaketi_cari.Where(m => m.kpsft_FisNo == fisno).ToListAsync(),
                cariModel = await _db.kpsft_cariler.ToListAsync(),
                depoModel = await _db.kpsft_depobilgisi
                .Include(m => m.depoTuru)
                .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CariCikisFisiDuzenle([FromBody] JsonStok json)
        {
            if (ModelState.IsValid)
            {
                var currentFis = await _db.kpsft_fistablo_cari
                    .Where(s => s.kpsft_FisNo == json.FisNo)
                    .FirstOrDefaultAsync();

                if (currentFis == null)
                {
                    return NotFound(new { success = false, message = "Mevcut fiş bulunamadı." });
                }
                else
                {
                    currentFis.kpsft_tarih = json.Tarih;
                    currentFis.kpsft_islemKaynagiCari = json.cari;
                    currentFis.kpsft_depo = json.depo;

                    var mevcutHareketler = await _db.kpsft_fisharaketi_cari
                        .Where(h => h.kpsft_FisNo == json.FisNo)
                        .ToListAsync();

                    List<FisHaraketleriCariModel> yeniHareketler = new List<FisHaraketleriCariModel>();

                    foreach (var datum in json.Data)
                    {
                        // Aynı StokKod ve FisNo'ya sahip bir hareket varsa
                        var mevcutHareket = mevcutHareketler
                            .FirstOrDefault(h => h.kpsft_StokKod == datum.kpsft_StokKod);

                        if (mevcutHareket != null)
                        {
                            // Miktarı güncelle
                            mevcutHareket.kpsft_Miktar = datum.kpsft_Miktar;

                            // Bu hareket işlendiği için listeden çıkarıyoruz (geriye kalanlar silinecekler)
                            mevcutHareketler.Remove(mevcutHareket);
                        }
                        else
                        {
                            // Eğer aynı stok yoksa yeni hareket olarak ekliyoruz
                            yeniHareketler.Add(new FisHaraketleriCariModel
                            {
                                kpsft_FisNo = json.FisNo,
                                kpsft_StokKod = datum.kpsft_StokKod,
                                kpsft_Miktar = datum.kpsft_Miktar
                            });
                        }
                    }

                    // Veritabanına yeni eklenen hareketleri ekliyoruz
                    _db.kpsft_fisharaketi_cari.AddRange(yeniHareketler);

                    // Geride kalan hareketleri, JSON verisinde bulunmadıkları için silinmesi gerekenler olarak düşünüyoruz
                    _db.kpsft_fisharaketi_cari.RemoveRange(mevcutHareketler);

                    try
                    {
                        await _db.SaveChangesAsync();
                        return Ok(new { success = true, message = "İşlem başarılı." });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new { success = false, message = "Bir hata oluştu.", error = ex.Message });
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        [HttpGet]
        public async Task<IActionResult> DepolarArasıTransfer()
        {
            var stokKartlar = await _db.kpsft_stok_karti
            .Include(s => s.durum)               // DurumModel ilişkisini dahil et
            .Include(s => s.birimModel)          // BirimModel ilişkisini dahil et
            .Include(s => s.grupModel)           // GrupModel ilişkisini dahil et
            .Include(s => s.gumrukKoduModel)     // GumrukKoduModel ilişkisini dahil et
            .Include(s => s.kaliteModel)         // KaliteModel ilişkisini dahil et
            .Include(s => s.satisKdvOrani)       // SatisKdvOrani ilişkisini dahil et
            .Include(s => s.alisKdvOraniModel)   // AlisKdvOraniModel ilişkisini dahil et
            .Include(s => s.stokTipi)            // StokTipi ilişkisini dahil et
            .ToListAsync();


            var model = new DoubleModel
            {
                StokKartıModel = stokKartlar,
                FistabloDepoModel = new FisTabloDepoModel(),
                FisHaraketleriModelDepo = new List<FisHaraketleriDepoModel>(),
                depoModel = await _db.kpsft_depobilgisi
                .Include(m => m.depoTuru)
                .ToListAsync()
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> DepolarArasıTransfer(DoubleModel model)
        {
            var fisno = model.FistabloDepoModel.kpsft_FisNo;
            model.FistabloDepoModel.kpsft_FisTuru = "Depolar Arası Transfer";
            model.FistabloDepoModel.kpsft_islemTuru = "Depolar Arası Transfer";

            // Giriş fişini veritabanına ekleyin
            _db.kpsft_fistablo_depo.Add(model.FistabloDepoModel);
            await _db.SaveChangesAsync();

            foreach (var fisHareketi in model.FisHaraketleriModelDepo)
            {
                // StokKartı ile ilgili olan referansı null yapma
                fisHareketi.StokKartı = null;
                fisHareketi.kpsft_FisNo = fisno;

                // Çıkış deposu için mevcut stok miktarını al
                var mevcutFisHaraketiCikisDepo = await _db.kpsft_fisharaketi_depo
                    .Where(f => f.kpsft_StokKod == fisHareketi.kpsft_StokKod
                                && f.fistablodepoModel.kpsft_depo == model.FistabloDepoModel.kpsft_islemKaynagiDepo) // Çıkış deposu
                    .OrderByDescending(f => f.kpsft_FisHaraketiID)
                    .FirstOrDefaultAsync();

                // Giriş deposu için mevcut stok miktarını al
                var mevcutFisHaraketiGirisDepo = await _db.kpsft_fisharaketi_depo
                    .Where(f => f.kpsft_StokKod == fisHareketi.kpsft_StokKod
                                && f.fistablodepoModel.kpsft_depo == model.FistabloDepoModel.kpsft_depo) // Giriş deposu
                    .OrderByDescending(f => f.kpsft_FisHaraketiID)
                    .FirstOrDefaultAsync();

                // Çıkış deposundan stok miktarını azalt
                if (mevcutFisHaraketiCikisDepo != null && mevcutFisHaraketiCikisDepo.kpsft_Mevcut_Miktar >= fisHareketi.kpsft_Miktar)
                {
                    mevcutFisHaraketiCikisDepo.kpsft_Mevcut_Miktar -= fisHareketi.kpsft_Miktar;
                    _db.kpsft_fisharaketi_depo.Update(mevcutFisHaraketiCikisDepo);
                }
                else
                {
                    // Eğer çıkış deposunda yeterli miktar yoksa hata döndürebilirsiniz
                    return BadRequest("Çıkış deposunda yeterli stok yok.");
                }

                // Giriş deposuna stok miktarını ekle
                if (mevcutFisHaraketiGirisDepo == null)
                {
                    // Eğer giriş deposunda bu stok kodu daha önce yoksa yeni bir kayıt oluştur
                    fisHareketi.kpsft_Mevcut_Miktar = fisHareketi.kpsft_Miktar;
                    _db.kpsft_fisharaketi_depo.Add(fisHareketi);
                }
                else
                {
                    // Eğer giriş deposunda stok varsa mevcut miktarı güncelle
                    mevcutFisHaraketiGirisDepo.kpsft_Mevcut_Miktar += fisHareketi.kpsft_Miktar;
                    _db.kpsft_fisharaketi_depo.Update(mevcutFisHaraketiGirisDepo);
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("GirisFisiTanım");
        }






        [HttpGet]
        public async Task<IActionResult> CariCikisFisiEkle()
        {
            var lastFisNo = _db.kpsft_fistablo_cari
                                .Where(f => f.kpsft_FisTuru == "Çıkış Fişi")
                                .OrderByDescending(f => f.kpsft_FisNo)
                                .Select(f => f.kpsft_FisNo)
                                .FirstOrDefault();

            string newFisNo = "FC001"; 

            if (lastFisNo != null)
            {
                var prefix = lastFisNo.Substring(0, 2); // 'FC' gibi prefix
                var numberPart = lastFisNo.Substring(2); // '001' gibi sayı kısmı

                if (int.TryParse(numberPart, out int lastNumber))
                {
                    newFisNo = $"{prefix}{(lastNumber + 1).ToString("D3")}";
                }
                else
                {
                    
                }
            }

            ViewBag.cariFis = newFisNo;


            var stokKartlar = await _db.kpsft_stok_karti
             .Include(s => s.durum)               // DurumModel ilişkisini dahil et
             .Include(s => s.birimModel)          // BirimModel ilişkisini dahil et
             .Include(s => s.grupModel)           // GrupModel ilişkisini dahil et
             .Include(s => s.gumrukKoduModel)     // GumrukKoduModel ilişkisini dahil et
             .Include(s => s.kaliteModel)         // KaliteModel ilişkisini dahil et
             .Include(s => s.satisKdvOrani)       // SatisKdvOrani ilişkisini dahil et
             .Include(s => s.alisKdvOraniModel)   // AlisKdvOraniModel ilişkisini dahil et
             .Include(s => s.stokTipi)            // StokTipi ilişkisini dahil et
             .ToListAsync();


            var model = new DoubleModel
            {
                StokKartıModel = stokKartlar,
                fistablocariModel = new FisTabloCariModel(), // FisTabloModel nesnesini başlatın
                FisHaraketleriModelCari = new List<FisHaraketleriCariModel>(), // Boş liste başlatın veya gerekli verilerle doldurun
                cariModel = await _db.kpsft_cariler.ToListAsync(),
                depoModel = await _db.kpsft_depobilgisi
                .Include(m => m.depoTuru)
                .ToListAsync()
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> CariCikisFisiEkle([FromBody] JsonStok json)
        {
            if (ModelState.IsValid)
            {
                FisTabloCariModel model = new FisTabloCariModel
                {
                    kpsft_FisTuru = "Çıkış Fişi",
                    kpsft_FisNo = json.FisNo,
                    kpsft_tarih = json.Tarih,
                    kpsft_islemKaynagiCari = json.cari,
                    kpsft_depo = json.depo
                };

                _db.kpsft_fistablo_cari.Add(model);

                List<FisHaraketleriCariModel> fisHareketleri = new List<FisHaraketleriCariModel>();
                foreach (var datum in json.Data)
                {
                    FisHaraketleriCariModel fisH = new FisHaraketleriCariModel
                    {
                        kpsft_Miktar = datum.kpsft_Miktar,
                        kpsft_StokKod = datum.kpsft_StokKod,
                        kpsft_FisNo = json.FisNo
                    };
                    fisHareketleri.Add(fisH);
                }

                _db.kpsft_fisharaketi_cari.AddRange(fisHareketleri);

                try
                {
                    await _db.SaveChangesAsync();

                    return Ok(new { success = true, message = "İşlem başarılı." });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = "Bir hata oluştu.", error = ex.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        [HttpGet]
        public IActionResult YeniBirimEkle(BirimModel model)
        {

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YeniBirimEklee(BirimModel model)
        {
            _db.kpsft_birimler.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("StokKartiEkle");
        }

        public IActionResult YeniStokTipiEkle(StokTipi model)
        {

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YeniStokTipiEklee(StokTipi model)
        {
                _db.kpsft_stoktipi.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("StokKartiEkle"); // İşlem tamamlandığında yönlendirilecek sayfa
         
        }

        [HttpGet]
        public IActionResult YeniKaliteEkle(KaliteModel model)
        {

             var lastKaliteNo = _db.kpsft_kalite
            .OrderByDescending(f => f.kpsft_kalite_no)
            .Select(f => f.kpsft_kalite_no)
            .FirstOrDefault();

            string newKaliteNo = "001"; // Varsayılan yeni kalite numarası

            if (lastKaliteNo != null)
            {
                // Kalite numarasını ayırın ve sayısal kısmı artırın
                var numberPart = lastKaliteNo; // Kalite numarasının tamamı sayısal olduğu için direkt kullanılıyor

                if (int.TryParse(numberPart, out int lastNumber))
                {
                    // Yeni kalite numarası oluşturulurken 3 haneli olacak şekilde ayarlandı
                    newKaliteNo = $"{(lastNumber + 1).ToString("D3")}";
                }
                else
                {
                    // Sayısal kısım uygun formatta değilse, uygun hata işleme yapın
                    // Bu durumda newKaliteNo varsayılan "001" kalmaya devam edecek
                }
            }

            ViewBag.newKaliteNo = newKaliteNo;



            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YeniKaliteEklee(KaliteModel model)
        {
            
                _db.kpsft_kalite.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("StokKartiEkle");
            
        }


        [HttpGet]
        public IActionResult YeniGrupKoduEkle(GrupModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YeniGrupKoduEklee(GrupModel model)
        {
                _db.kpsft_grup_kodu.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("StokKartiEkle");
        }


        [HttpGet]
        public IActionResult YeniAlisKdvOraniEkle(AlısKdvOraniModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YeniAlisKdvOraniEklee(AlısKdvOraniModel model)
        {
                _db.kpsft_alis_kdv_orani.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("StokKartiEkle");
        }


        [HttpGet]
        public IActionResult YeniSatisKdvOraniEkle(SatisKdvOrani model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YeniSatisKdvOraniEklee(SatisKdvOrani model)
        {
                _db.kpsft_satis_kdv_orani.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("StokKartiEkle");
        }

        [HttpGet]
        public IActionResult YeniGumrukKoduEkle(GumrukKoduModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YeniGumrukKoduEklee(GumrukKoduModel model)
        {
                _db.kpsft_gumruk_kodu.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("StokKartiEkle"); // Yönlendirme yapılacak sayfa
        }

        [HttpGet]
        public IActionResult YeniVergiDairesi()
        {
            VergiDairesiModel model = new VergiDairesiModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YeniVergiDairesi(VergiDairesiModel model)
        {
            _db.kpsft_vergidairesi.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("YeniCariEkle"); // Yönlendirme yapılacak sayfa
        }


        [HttpGet]
        public IActionResult Kapat()
        {
            return RedirectToAction("index", "Login");
        }

    }
}
