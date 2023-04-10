using System.ComponentModel.DataAnnotations;

namespace WebApplication1.EntityModels
{
    public class Opstina
    {
        [Key]
        public int Id { get; set; }
        public string Naziv { get; set; }
    }
}
