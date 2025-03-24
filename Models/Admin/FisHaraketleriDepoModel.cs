using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace LoginEkrani.Models.Admin
{
    public class FisHaraketleriDepoModel
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int kpsft_FisHaraketiID { get; set; }

        [Required]
        public string kpsft_FisNo { get; set; }

        [ForeignKey("kpsft_FisNo")]
        public FisTabloDepoModel fistablodepoModel { get; set; }

        [Required]
        public string kpsft_StokKod { get; set; }

        [ForeignKey("kpsft_StokKod")]
        public StokKartıModel StokKartı { get; set; }

        [Required]
        public int kpsft_Miktar { get; set; }
        
        public int kpsft_Mevcut_Miktar { get; set; }
    }

}
