using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginEkrani.Models.Admin
{
    public class GrupModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_kpsft { get; set; }


        [Required]
        [StringLength(45)]
        public string kpsft_grup_kodu { get; set; }

        [Required]
        [StringLength(45)]
        public string kpsft_grup_adi { get; set; }

        [StringLength(100)]
        public string kpsft_aciklama { get; set; }
    }
}
