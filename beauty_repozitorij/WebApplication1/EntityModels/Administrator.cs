using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    [Table("Administrator")]
    public class Administrator : KorisnickiNalog
    {
        public DateTime DatumKreiranja { get; set; }
    }
}
