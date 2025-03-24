using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LoginEkrani.Models.Admin
{
    public class KaliteModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_kpsft {  get; set; }

        [StringLength(45)]
        public string kpsft_kalite_no {  get; set; }

        [StringLength(45)]
        public string kpsft_kalite_kod { get; set; }

        [StringLength(45)]
        public string kpsft_ilgiliStandart {  get; set; }

        [StringLength(100)] // Maksimum uzunluk
        public string? kpsft_aciklama { get; set; }

        public double? kpsft_c { get; set; } // C %
        public double? kpsft_mn { get; set; } // Mn %
        public double? kpsft_si { get; set; } // Si %
        public double? kpsft_s_max { get; set; } // S % Max
        public double? kpsft_p_max { get; set; } // P % Max
        public double? kpsft_n { get; set; } // N ppm
        public double? kpsft_ceq { get; set; } // Ceq %
        public double? kpsft_cu { get; set; } // Cu %
        public double? kpsft_al { get; set; } // Al %
        public double? kpsft_mo { get; set; } // Mo %
        public double? kpsft_v { get; set; } // V %
        public double? kpsft_b { get; set; } // B %
        public double? kpsft_ca { get; set; } // Ca %
        public double? kpsft_ti { get; set; } // Ti %
        public double? kpsft_sn { get; set; } // Sn %
        public double? kpsft_as { get; set; } // As %

    }
}
