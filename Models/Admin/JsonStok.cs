using Org.BouncyCastle.Asn1.Mozilla;

namespace LoginEkrani.Models.Admin
{
    public class JsonStok
    {
        public List<Datum> Data { get; set; }
        public string FisNo { get; set; }
        
        public DateOnly Tarih { get; set; }
        public string cari {  get; set; }
        public string depo {  get; set; }
    }

    public class Datum
    {
        public string kpsft_StokKod { get; set; }
        public int kpsft_Miktar { get; set; }
    }

}
