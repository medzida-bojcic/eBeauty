using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    [Table("Uposlenik")]
    public class Uposlenik : KorisnickiNalog
    {
        public int ObavljeneNarudzbe { get; set; }
        public int AktivneNarudzbe { get; set; }
    }
}
