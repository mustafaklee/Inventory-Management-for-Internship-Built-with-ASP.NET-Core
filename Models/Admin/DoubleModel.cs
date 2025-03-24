namespace LoginEkrani.Models.Admin
{
    public class DoubleModel
    {
        public List<StokKartıModel>? StokKartıModel { get; set; }
        public List<FisHaraketleriCariModel>? FisHaraketleriModelCari { get; set; }
        public List<FisHaraketleriDepoModel>? FisHaraketleriModelDepo { get; set; }

        // Fiş hareketlerinde gösterebilmek için
        public List<FisTabloCariModel>? FisTabloCariModelList { get; set; }
        public List<FisTabloDepoModel>? FisTabloDepoModelList { get; set; }

        public FisTabloCariModel? fistablocariModel { get; set; }
        public FisTabloDepoModel? FistabloDepoModel { get; set; }
        public List<CariModel>? cariModel { get; set; }
        public List<DepoModel>? depoModel { get; set; }

        public List<FormTabloModel>? formTabloModelList { get; set; }
        public List<FormHareketiModel>? formHareketiModel { get; set; }

        public FormTabloModel? formTabloModel { get; set; }
        public FormHareketiModel? formharaketimodel { get; set; }
        public CariModel? carimodel { get; set; }
    }

}