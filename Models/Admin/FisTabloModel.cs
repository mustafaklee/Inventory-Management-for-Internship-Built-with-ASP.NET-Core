using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LoginEkrani.Models.Admin
{
    public class FisTabloModel
    {
        public List<FisHaraketleriCariModel> FisHaraketleriCari { get; set; }
        public List<FisHaraketleriDepoModel> FisHaraketleriDepo { get; set; }
    }

}
