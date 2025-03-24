using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LoginEkrani.Models.Admin
{
    public class FormTabloModel
    {
        [Key]
        [StringLength(45)]
        public string kpsft_evrakNo { get; set; }

        [Required]
        public DateTime kpsft_evrak_tarih { get; set; }

        [Required]
        public DateTime kpsft_teslim_tarih { get; set; }

        [Required]
        [StringLength(45)]
        public string kpsft_cari { get; set; }


        [StringLength(100)]
        public string? kpsft_aciklama { get; set; }

        [StringLength(100)]
        public string? kpsft_satici_notu { get; set; }

        public string kpsft_paraBirim {  get; set; }

        [ForeignKey("kpsft_cari")]
        public CariModel cari { get; set; }

        public ICollection<FormHareketiModel> formtHaraketleri { get; set; }
    }
}
