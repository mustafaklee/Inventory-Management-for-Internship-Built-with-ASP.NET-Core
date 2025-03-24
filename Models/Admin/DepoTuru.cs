using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginEkrani.Models.Admin
{
    public class DepoTuru
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id_kpsft { get; set; }

        [Required]
        [StringLength(45)]
        public string kpsft_depoTuru { get; set; }
    }
}
