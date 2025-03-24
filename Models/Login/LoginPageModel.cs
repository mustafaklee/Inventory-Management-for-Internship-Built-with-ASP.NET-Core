using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginEkrani.Models.Login
{
    public class LoginPageModel
    {
        [Required]
        [StringLength(45)]
        public string kpsft_kullaniciAdi { get; set; }

        [Required]
        [StringLength(45)]
        public string kpsft_ad { get; set; }

        [Required]
        [StringLength(45)]
        public string kpsft_soyad { get; set; }

        [Required]
        [StringLength(150)]
        public string kpsft_sifre { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_kpsft { get; set; } // Otomatik artan bir ID

        [Required]
        public int kpsft_tcKimlik { get; set; }
        [Required]
        public string kpsft_mailAdrress { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; } // Kullanıcı ve roller arasındaki ilişkiyi temsil eder
    }
}
