
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    public class Proizvod
    {
        [Key]
        public int Id { get; set; }
        public string Naziv { get; set; }
        public float Cijena { get; set; }
        public string Opis { get; set; }
        public int Kolicina { get; set; }
        public string Slika { get; set; }

        [ForeignKey("KategorijaId")]
        public int KategorijaId { get; set; }
        public Kategorija Kategorija { get; set; }
    }
}
