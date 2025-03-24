using System.ComponentModel.DataAnnotations;

namespace LoginEkrani.Models.Admin
{
    public class CariModel
    {
        [Key]
        [Required]
        [StringLength(45)]
        public string kpsft_cariKod { get; set; }

        [Required]
        [StringLength(45)]
        public string kpsft_cariAd { get; set; }

        [Required]
        public int kpsft_cariTipi { get; set; }

        [StringLength(100)]
        public string? kpsft_vergiNo { get; set; }

        [StringLength(45)]
        public string? kpsft_vergiDairesi { get; set; }

        [StringLength(45)]
        public string? kpsft_adres1 { get; set; }

        [StringLength(45)]
        public string? kpsft_adres2 { get; set; }

        [StringLength(45)]
        public string? kpsft_cariAd2 { get; set; }

        [StringLength(45)]
        public string? kpsft_adres3 { get; set; }

        [StringLength(45)]
        public string? kpsft_telefon1 { get; set; }

        [StringLength(45)]
        public string? kpsft_telefon2 { get; set; }

        [StringLength(45)]
        public string? kpsft_email { get; set; }

        [StringLength(100)]
        public string? kpsft_ıban { get; set; }

        [StringLength(45)]
        public string? kpsft_aciklama { get; set; }

        // Yeni eklenen alanlar
        public int? kpsft_yurticiDisi { get; set; } // Yurtiçi/Dışı bilgisi

        [StringLength(80)]
        public string? kpsft_web_sitesi { get; set; } // Web sitesi bilgisi

        public int? kpsft_durum { get; set; } // Cari durumu

        public int? kpsft_fason_uretim { get; set; } // Fason üretim bilgisi
    }
}
