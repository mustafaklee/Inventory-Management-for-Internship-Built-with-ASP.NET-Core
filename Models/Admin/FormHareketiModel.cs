using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace LoginEkrani.Models.Admin
{
    public class FormHareketiModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int kpsft_FisHaraketiID { get; set; }

        [Required]
        [StringLength(45)]
        public string kpsft_stokKod { get; set; }

        [Required]
        [StringLength(45)]
        public string kpsft_evrakNo { get; set; }

        [Required]
        public int kpsft_Miktar { get; set; }

        [Required]
        public int kpsft_birimFiyat { get; set; }

        [Required]
        public int kpsft_kdv { get; set; }


        [Required]
        [StringLength(45)]
        public string kpsft_gemi { get; set; }


        [Required]
        public double kpsft_tutar { get; set; }



        [ForeignKey("kpsft_evrakNo")]
        public FormTabloModel formTabloModel { get; set; }

        [ForeignKey("kpsft_stokKod")]
        public StokKartıModel stokKartıModel { get; set; }

    }
}
