using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LoginEkrani.Models.Admin
{
    public class SatisKdvOrani
    {

        [Key] // Birincil anahtar
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_kpsft { get; set; }


        [StringLength(45)] // Maksimum uzunluk
        public string kpsft_kdv_orani { get; set; }

        [StringLength(100)] // Maksimum uzunluk
        public string kpsft_aciklama { get; set; }
    }
}
