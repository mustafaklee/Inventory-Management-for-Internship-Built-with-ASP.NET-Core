using System.ComponentModel.DataAnnotations;

namespace LoginEkrani.Models.Admin
{
    public class VergiDairesiModel
    {
        [Key]
        [StringLength(45)]
        public string kpsft_vergiDKodu { get; set; }

        [Required]
        [StringLength(100)]
        public string kpsft_vergiDAdi { get; set; }

        [StringLength(100)]
        public string kpsft_aciklama { get; set; }
    }
}
