using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace LoginEkrani.Models.Login
{
    public class LogKayıtlarıModelDis
    {

        [StringLength(45)]
        public string kpsft_kullaniciAdi { get; set; }

        [StringLength(150)]
        public string kpsft_sifre { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_kpsft { get; set; } // Otomatik artan bir ID

        [StringLength(45)]
        public string kpsft_islemAdi { get; set; }
        public DateTime kpsft_zaman { get; set; }
    }
}
