using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    [Table("Korisnik")]
    public class Korisnik : KorisnickiNalog
    {
        public string Adresa { get; set; }
        public string BrojTelefona { get; set; }
        public string Slika { get; set; }

        [ForeignKey("OpstinaId")]
        public int OpstinaId { get; set; }
        public Opstina Opstina { get; set; }
    }
}
