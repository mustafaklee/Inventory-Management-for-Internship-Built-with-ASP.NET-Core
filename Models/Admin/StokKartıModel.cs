using LoginEkrani.Models.Admin;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class StokKartıModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_kpsft { get; set; }

    [Key]
    [StringLength(45)]
    public string kpsft_stokKodu { get; set; } 

    [StringLength(80)]
    [Required]
    public string kpsft_stokAdi { get; set; }

    [StringLength(45)]
    public string kpsft_uretim_turu { get; set; }

    [Required]
    public int kpsft_birim { get; set; }

    [Required]
    public int kpsft_durum { get; set; }

    public int? kpsft_grupKodu { get; set; }
    public int? kpsft_stokKartTipi { get; set; }
    public int? kpsft_alisKdvOrani { get; set; }
    public int? kpsft_satisKdvOrani { get; set; }
    public int? kpsft_kalite { get; set; }

    public int? kpsft_kritikMiktar { get; set; }
    public int? kpsft_lotBuyuklugu { get; set; }

    [Required]
    public int kpsft_gtipKodu { get; set; }

    public int? kpsft_boyut1 { get; set; }
    public int? kpsft_boyut2 { get; set; }
    public int? kpsft_boyut3 { get; set; }
    public int? kpsft_boyut4 { get; set; }
    public int? kpsft_boyut5 { get; set; }
    public int? kpsft_uretim_parti_buyuklugu { get; set; }

    [StringLength(100)]
    public string? kpsft_aciklama { get; set; }

    [ForeignKey("kpsft_durum")]
    public DurumModel durum { get; set; }

    [ForeignKey("kpsft_birim")]
    public BirimModel birimModel { get; set; }

    [ForeignKey("kpsft_grupKodu")]
    public GrupModel grupModel { get; set; }

    [ForeignKey("kpsft_alisKdvOrani")]
    public AlısKdvOraniModel alisKdvOraniModel { get; set; }

    [ForeignKey("kpsft_satisKdvOrani")]
    public SatisKdvOrani satisKdvOrani { get; set; }

    [ForeignKey("kpsft_kalite")]
    public KaliteModel kaliteModel { get; set; }

    [ForeignKey("kpsft_stokKartTipi")]
    public StokTipi stokTipi { get; set; }

    [ForeignKey("kpsft_gtipKodu")]
    public GumrukKoduModel gumrukKoduModel { get; set; }
}
