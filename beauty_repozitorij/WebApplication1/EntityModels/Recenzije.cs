using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    public class Recenzije
    {
        [Key]
        public int Id { get; set; }
        public string Opis { get; set; }

        [ForeignKey(nameof(Usluga))]
        public int UslugaId { get; set; }
        public Usluga Usluga { get; set; }

        [ForeignKey(nameof(Korisnik))]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
    }
}
