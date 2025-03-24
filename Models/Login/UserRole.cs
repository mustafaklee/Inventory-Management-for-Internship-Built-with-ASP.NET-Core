using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginEkrani.Models.Login
{
    public class UserRole
    {
        [Key]
        public int id_kpsft { get; set; }
        public int kpsft_rol_id { get; set; }

        [ForeignKey("id_kpsft")]
        public LoginPageModel User { get; set; } // Navigation property

        [ForeignKey("kpsft_rol_id")]
        public Role Role { get; set; } // Navigation property
    }
}
