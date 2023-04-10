namespace WebApplication1.Modul_Proizvod.ViewModels
{
    public class ProizvodUpdateVM
    {
        public int id { get; set; }
        public string naziv { get; set; }
        public string opis { get; set; }
        public float cijena { get; set; }
        public int kategorijaId { get; set; }
        public string slika { get; set; }
    }
}
