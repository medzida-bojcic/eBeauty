using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    public class Skladiste
    {
        [Key]
        public int Id { get; set; }
        public string Adresa { get; set; }
        public string Naziv { get; set; }

        [ForeignKey(nameof(Opstina))]
        public int NarudzbaId { get; set; }
        public Opstina Opstina { get; set; }
    }
}
