using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LoginEkrani.Models.Admin
{
    public class TurModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_kpsft { get; set; }
        [StringLength(45)]
        [Required]
        public string kpsft_turKodu { get; set; }

        [StringLength(100)]
        public string? kpsft_aciklama { get; set; }
    }
}
