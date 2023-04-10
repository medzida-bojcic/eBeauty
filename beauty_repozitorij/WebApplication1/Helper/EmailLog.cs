using WebApplication1.EntityModels;

namespace WebApplication1.Helper
{
    public class EmailLog
    {
        public static void uspjesnoLogiranKorisnik(AutentifikacijaToken token, HttpContext httpContext)
        {
            var logiraniKorisnik = token.korisnickiNalog;
            if (logiraniKorisnik.isUser)
            {
                var poruka = $"Postovani {logiraniKorisnik.KorisnickoIme} <br>, "  +
                    $"kod za 2F autorizaciju je <br> "+
                    $"{token.twoFCode} <br>" +
                    $"Login info {DateTime.Now}";
                EmailSender.Send(logiraniKorisnik.Email, "Kod za 2F autorizaciju ", poruka, true);
            }
        }
    }
}
