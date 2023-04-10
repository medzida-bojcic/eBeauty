using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    public class Salon
    {
        [Key]
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Telefon { get; set; }
        public string RadnoVrijeme { get; set; }

        [ForeignKey(nameof(Opstina))]
        public int OpstinaId { get; set; }
        public Opstina Opstina { get; set; }
    }
}
