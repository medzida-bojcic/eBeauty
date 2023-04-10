namespace WebApplication1.Modul_Narudzba.ViewModels
{
    public class NarudzbaGetNarudzbaVM
    {
        public int id { get; set; }
        public float cijena { get; set; }

        public class Stavka
        {
            public int id { get; set; }
            public string naziv { get; set; }
            public string opis { get; set; }
            public float cijena { get; set; }
            public string slika { get; set; }
            public int kolicina { get; set; }
            public int kategorijaId { get; set; }
        }

        public List<Stavka> stavke { get; set; }
    }
}
