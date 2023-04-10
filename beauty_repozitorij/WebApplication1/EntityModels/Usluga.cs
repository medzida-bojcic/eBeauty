using System.ComponentModel.DataAnnotations;

namespace WebApplication1.EntityModels
{
    public class Usluga
    {
        [Key]
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
    }
}
