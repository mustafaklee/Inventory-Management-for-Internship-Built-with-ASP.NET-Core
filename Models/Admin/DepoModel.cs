using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginEkrani.Models.Admin
{
    public class DepoModel
    {
        [Key]
        [Required]
        public string kpsft_depoKod { get; set; }

        [Required]
        [StringLength(45)]
        public string kpsft_depoAd { get; set; }

        [Required]
        public int kpsft_depoTuru { get; set; }

        [Required]
        public int kpsft_durum { get; set; }

        public int? kpsft_genislik { get; set; }

        public int? kpsft_yukseklik { get; set; }

        public int? kpsft_uzunluk { get; set; }

        public int? kpsft_alan { get; set; }

        [StringLength(45)]
        public string? kpsft_depoSorumlusu { get; set; }

        [StringLength(100)]
        public string? kpsft_aciklama { get; set; }

        [ForeignKey("kpsft_depoTuru")]
        public DepoTuru depoTuru { get; set; }  
    }
}
