using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    public class AutentifikacijaToken
    {
        [Key]
        public int Id { get; set; }
        public string Vrijednost { get; set; }
        public DateTime VrijemeEvidentiranja { get; set; }
        public string IpAdresa { get; set; }
        [ForeignKey(nameof(korisnickiNalog))]
        public int KorisnickiNalogId { get; set; }
        public KorisnickiNalog korisnickiNalog { get; set; }
        public string? twoFCode { get; set; }
        public bool? twoFJelOtkljucano { get; set; }
    }
}
