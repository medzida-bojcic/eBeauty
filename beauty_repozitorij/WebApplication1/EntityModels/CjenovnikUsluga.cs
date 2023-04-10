using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    public class CjenovnikUsluga
    {
        [Key]
        public int Id { get; set; }
        public float Cijena { get; set; }
        public DateTime VrijemeTrajanja { get; set; }
        public string Opis { get; set; }
        [ForeignKey(nameof(Usluga))]
        public int UslugaId { get; set; }
        public Usluga Usluga { get; set; }
    }
}
