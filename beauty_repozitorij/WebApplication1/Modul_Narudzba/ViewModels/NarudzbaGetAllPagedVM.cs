namespace WebApplication1.Modul_Narudzba.ViewModels
{
    public class NarudzbaGetAllPagedVM
    {
        public int id { get; set; }
        public float cijena { get; set; }
        public string datumNarudzbe { get; set; }
        public string statusNarudzbe { get; set; }
        public class Stavka
        {
            public string naziv { get; set; }
            public int kolicina { get; set; }
        }
        public List<Stavka> stavke { get; set; }
    }
}
