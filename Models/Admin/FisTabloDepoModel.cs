using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LoginEkrani.Models.Admin
{
    public class FisTabloDepoModel
    {
        [Key]
        [StringLength(45)]
        public string kpsft_FisNo { get; set; }

        [Required]
        public DateOnly kpsft_tarih { get; set; } = DateOnly.FromDateTime(DateTime.Now);


        [Required]
        public string kpsft_FisTuru { get; set; }

        [Required]
        public string kpsft_islemKaynagiDepo { get; set; }

        public string kpsft_islemTuru { get; set; }

        [Required]
        public string kpsft_depo { get; set; }


        [ForeignKey("kpsft_islemKaynagiDepo")]
        public DepoModel Depoislem { get; set; }


        [ForeignKey("kpsft_depo")]
        public DepoModel Depo { get; set; }


        // Fişin stok hareketlerini içeren koleksiyon
        public ICollection<FisHaraketleriDepoModel> FisHaraketleri { get; set; }

    }
}
