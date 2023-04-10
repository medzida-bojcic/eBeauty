using System.ComponentModel.DataAnnotations;

namespace WebApplication1.EntityModels
{
    public class Kategorija
    {
        [Key]
        public int Id { get; set; }
        public string NazivKategorije { get; set; }
        public string? Slika { get; set; }
    }
}
