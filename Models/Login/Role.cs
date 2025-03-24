using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginEkrani.Models.Login
{ 
    public class Role
    {
        [Key]
        public int id_kpsft { get; set; }
        public string kpsft_rol { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } // Navigation property
    }
}
