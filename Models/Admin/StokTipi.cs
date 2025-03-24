using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginEkrani.Models.Admin
{
    public class StokTipi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_kpsft { get; set; }
        [Required]
        [StringLength(45)]
        public string kpsft_kod { get; set; }

        [Required]
        [StringLength(45)]
        public string kpsft_stok_tipi { get; set; }

        [Required]
        [StringLength(100)]
        public string kpsft_aciklama { get; set; }
    }
}
