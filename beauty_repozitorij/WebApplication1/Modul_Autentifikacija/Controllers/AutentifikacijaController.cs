using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.EntityModels;
using WebApplication1.Helper;
using WebApplication1.Helper.ErrorHandler;
using WebApplication1.Modul_Autentifikacija.ViewModels;
using static WebApplication1.Helper.AutentifikacijaAutorizacija.MyAuthTokenExtension;

namespace WebApplication1.Modul_Autentifikacija.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AutentifikacijaController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AutentifikacijaController(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        [HttpGet("{code}")]
        public ActionResult Otkljucaj(string code)
        {
            var korisnickiNalog = HttpContext.GetLoginInfo().korisnickiNalog;

            if (korisnickiNalog == null)
            {
                return BadRequest("Korisnik nije logiran");
            }

            var token = dbContext.AutentifikacijaToken.FirstOrDefault(s => s.twoFCode == code && s.KorisnickiNalogId == korisnickiNalog.Id);
            if (token != null)
            {
                token.twoFJelOtkljucano = true;
                dbContext.SaveChanges();
                return Ok();
            }

            return BadRequest("pogresan URL");
        }

        [HttpPost]
        public ActionResult<LoginInformacije> Login([FromBody] LoginVM loginVM)
        {
            KorisnickiNalog logiranKorisnik = dbContext.KorisnickiNalog
                .FirstOrDefault(x => x.KorisnickoIme == loginVM.korisnickoIme && x.Lozinka == loginVM.lozinka);

            if (logiranKorisnik == null)
            {
                //pogresan username i password
                throw new ErrorResponse("User not found!");
                //return BadRequest("Korisnik nije logiran");
            }

            string randomString = TokenGenerator.Generate(10);
            string twoFCode = TokenGenerator.Generate(4);

            var noviToken = new AutentifikacijaToken()
            {
                IpAdresa = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                Vrijednost = randomString,
                korisnickiNalog = logiranKorisnik,
                VrijemeEvidentiranja = DateTime.Now,
               twoFCode= twoFCode
            };

            dbContext.Add(noviToken);
            dbContext.SaveChanges();

            EmailLog.uspjesnoLogiranKorisnik(noviToken, Request.HttpContext);

            return new LoginInformacije(noviToken);
        }
        [HttpPost]
        public ActionResult Logout()
        {
            AutentifikacijaToken autentifikacijaToken = HttpContext.GetAuthToken();

            if (autentifikacijaToken == null)
                return Ok();

            dbContext.Remove(autentifikacijaToken);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public ActionResult<AutentifikacijaToken> Get()
        {
            AutentifikacijaToken autentifikacijaToken = HttpContext.GetAuthToken();

            return autentifikacijaToken;
        }

        [HttpGet("{id}")]
        public KorisnickiNalog GetUser(int id)
        {
            return dbContext.KorisnickiNalog.Find(id);
        }
    }
}
