using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    public class Narudzba
    {
        [Key]
        public int Id { get; set; }
        public DateTime DatumNarudzbe { get; set; }
        public bool Zakljucena { get; set; }
        public float Cijena { get; set; }

        public int BrojStavki { get; set; }

        [ForeignKey("KorisnikId")]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        [ForeignKey("UposlenikId")]
        public int? UposlenikId { get; set; }
        public Uposlenik Uposlenik { get; set; }

        [ForeignKey("StatusNarudzbeId")]
        public int? StatusNarudzbeId { get; set; }
        public StatusNarudzbe StatusNarudzbe { get; set; }
    }

}
