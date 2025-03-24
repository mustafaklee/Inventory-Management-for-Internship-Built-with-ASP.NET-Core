using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LoginEkrani.Models.Admin
{
    public class DurumModel
    {
        [Key]
        public int id_kpsft { get; set; }

        [StringLength(45)]
        [Required]
        public string kpsft_durum { get; set; }
    }
}
