using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace LoginEkrani.Models.Admin
{
    public class FisHaraketleriCariModel
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int kpsft_FisHaraketiID { get; set; }

        [Required]
        public string kpsft_FisNo { get; set; }

        [ForeignKey("kpsft_FisNo")]
        [JsonIgnore]
        [IgnoreDataMember]
        public FisTabloCariModel fistablocariModel { get; set; }

        [Required]
        public string kpsft_StokKod { get; set; }

        [ForeignKey("kpsft_StokKod")]
        public StokKartıModel StokKartı { get; set; }

        [Required]
        public int kpsft_Miktar { get; set; }

    }

}
