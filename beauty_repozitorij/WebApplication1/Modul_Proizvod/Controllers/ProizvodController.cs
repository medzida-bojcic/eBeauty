using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.EntityModels;
using WebApplication1.Helper;
using WebApplication1.Helper.AutentifikacijaAutorizacija;
using WebApplication1.Modul_Proizvod.ViewModels;

namespace WebApplication1.Modul_Proizvod.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProizvodController :  ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment WebHostEnvironment;
        public ProizvodController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            WebHostEnvironment= webHostEnvironment;
        }


        [HttpGet]
        public List<Proizvod> GetAll()
        {
            return _dbContext.Proizvod.ToList();
        }

        [HttpGet]
        public List<ProizvodGetAllPagedVM> GetAllPaged(string nazivKategorije)
        {
            List<ProizvodGetAllPagedVM> pagedStavke = _dbContext.Proizvod
                                            .Where(ps=>ps.Kategorija.NazivKategorije==nazivKategorije)
                                            .Select(ps => new ProizvodGetAllPagedVM()
                                            {
                                                id = ps.Id,
                                                naziv = ps.Naziv,
                                                opis = ps.Opis,
                                                cijena = ps.Cijena,
                                                slika = ps.Slika,
                                                nazivKategorije = ps.Kategorija.NazivKategorije
                                            }).ToList();
            return pagedStavke;
        }

        [HttpPost]
        public IActionResult GetAllPagedLog([FromBody] ProizvodGAPLogInfoVM proizvodGAPLogInfoVM)
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");

            Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;
            if (korisnik == null)
                return null;
            List<ProizvodGetAllPagedVM> pagedProizvodi = _dbContext.Proizvod
                                            .Where(pp => pp.Kategorija.NazivKategorije == proizvodGAPLogInfoVM.nazivKategorije)
                                            .Select(pp => new ProizvodGetAllPagedVM()
                                            {
                                                id = pp.Id,
                                                naziv = pp.Naziv,
                                                opis = pp.Opis,
                                                cijena = pp.Cijena,
                                                slika = pp.Slika,
                                                nazivKategorije = pp.Kategorija.NazivKategorije
                                            }).ToList();
            return Ok(pagedProizvodi);
        }

        
        [HttpGet("{id}")]
        public ProizvodUpdateVM GetById(int id)
        {
            Proizvod proizvod = _dbContext.Proizvod.Find(id);
            if (proizvod != null)
            {
                ProizvodUpdateVM odabraniProizvod = new ProizvodUpdateVM()
                {
                    id = proizvod.Id,
                    naziv = proizvod.Naziv,
                    opis = proizvod.Opis,
                    cijena = proizvod.Cijena,
                    kategorijaId = proizvod.KategorijaId,
                    slika = proizvod.Slika
                };
                return odabraniProizvod;
            }
            return null;
        }
       
    }
}
