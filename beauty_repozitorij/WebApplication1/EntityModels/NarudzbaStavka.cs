using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    public class NarudzbaStavka
    {
        [Key]
        public int Id { get; set; }
        public int Kolicina { get; set; }
        public float Cijena { get; set; }

        [ForeignKey("NarudzbaId")]
        public int NarudzbaId { get; set; }
        public Narudzba Narudzba { get; set; }

        [ForeignKey("ProizvodId")]
        public int ProizvodId { get; set; }
        public Proizvod Proizvod { get; set; }
    }
}
