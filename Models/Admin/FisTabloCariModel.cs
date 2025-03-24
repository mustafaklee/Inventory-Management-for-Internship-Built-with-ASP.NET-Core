using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LoginEkrani.Models.Admin
{
    public class FisTabloCariModel
    {
        [Key]
        [StringLength(45)]
        public string kpsft_FisNo { get; set; }

        [Required]
        public DateOnly kpsft_tarih { get; set; } = DateOnly.FromDateTime(DateTime.Now);


        [Required]
        public string kpsft_FisTuru { get; set; }

        [Required]
        public string kpsft_islemKaynagiCari { get; set; }

        [Required]
        public string kpsft_depo { get; set; }

        [ForeignKey("kpsft_islemKaynagiCari")]
        public CariModel Cariislem { get; set; }

        [ForeignKey("kpsft_depo")]
        public DepoModel Depo { get; set; }


        // Fişin stok hareketlerini içeren koleksiyon
        public ICollection<FisHaraketleriCariModel> FisHaraketleri { get; set; }
    }
}
