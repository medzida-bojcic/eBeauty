using Microsoft.EntityFrameworkCore;
using WebApplication1.EntityModels;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AutentifikacijaToken> AutentifikacijaToken { get; set; }
        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Kategorija> Kategorija { get; set; }
        public DbSet<CjenovnikUsluga> CjenovnikUsluga { get; set; }
        public DbSet<Notifikacije> Notifikacije { get; set; }
        public DbSet<Proizvod> Proizvod { get; set; }
        public DbSet<Korisnik> OnlineGost { get; set; }
        public DbSet<Narudzba> Narudzba { get; set; }
        public DbSet<NarudzbaStavka> NarudzbaStavka { get; set; }
        public DbSet<Salon> Salon { get; set; }
        public DbSet<Skladiste> Skladiste { get; set; }
        public DbSet<StanjeSkladista> StanjeSkladista { get; set; }
        public DbSet<Uposlenik> Uposlenik { get; set; }
        public DbSet<Opstina> Opstina { get; set; }
        public DbSet<Recenzije> Recenzije { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<KorisnickiNalog> KorisnickiNalog { get; set; }
        public DbSet<Rezervacija> Rezervacija { get; set; }
        public DbSet<KorpaStavke> KorpaStavke { get; set; }
        public DbSet<LogKretanjePoSistemu> LogKretanjePoSistemu { get; set; }
        public DbSet<StatusNarudzbe> StatusNarudzbe { get; set; }
        public DbSet<Usluga> Usluga { get; set; }

        public ApplicationDbContext(DbContextOptions options) :  base(options)
        {

        }
    }
}
