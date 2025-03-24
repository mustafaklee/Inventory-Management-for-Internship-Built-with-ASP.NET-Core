namespace LoginEkrani.Models.Admin
{
    public class JsonForm
    {
        public string kpsft_evrakNo { get; set; }
        public DateTime kpsft_evrak_tarih { get; set; }

        public DateTime kpsft_teslim_tarih { get; set; }
        public string kpsft_cari { get; set; }
        public string kpsft_aciklama { get; set; }
        public string kpsft_satici_notu { get; set; }
        public string kpsft_paraBirim { get; set; }
        public List<Datas> datas { get; set; }    
    }

    public class Datas
    {
        public string kpsft_stokKod { get; set; }
        public int kpsft_Miktar { get; set; }
        public int kpsft_birimFiyat { get; set; }
        public int kpsft_kdv { get; set; }
        public double kpsft_tutar { get; set; }
        public string kpsft_gemi { get; set; }
    }

}
