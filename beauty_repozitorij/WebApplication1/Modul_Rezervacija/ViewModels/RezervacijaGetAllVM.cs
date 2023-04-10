namespace WebApplication1.Modul_Rezervacija.ViewModels
{
    public class RezervacijaGetAllVM
    {
        public int id { get; set; }
        public DateTime datumRezervacije { get; set; }
        public int uslugaId { get; set; }
        public string nazivUsluge { get; set; }
    }
}
