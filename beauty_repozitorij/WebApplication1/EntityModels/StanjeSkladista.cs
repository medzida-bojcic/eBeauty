using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    public class StanjeSkladista
    {
        [Key]
        public int Id { get; set; }
        public int Kolicina { get; set; }

        [ForeignKey(nameof(Skladiste))]
        public int SkladisteId { get; set; }
        public Skladiste Skladiste { get; set; }

        [ForeignKey(nameof(Proizvod))]
        public int ProizvodId { get; set; }
        public Proizvod Proizvod { get; set; }
    }
}
