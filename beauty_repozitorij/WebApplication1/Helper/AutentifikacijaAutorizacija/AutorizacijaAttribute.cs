using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Helper.AutentifikacijaAutorizacija
{
    public class AutorizacijaAttribute : TypeFilterAttribute
    {
        public AutorizacijaAttribute(bool admin, bool korisnik, bool uposlenik) : base(typeof(MyAuthorizeImpl))
        {
            Arguments = new object[] { };
        }
    }

    public class MyAuthorizeImpl : IActionFilter
    {
        private readonly bool _admin;
        private readonly bool _korisnik;
        private readonly bool _uposlenik;
        public void OnActionExecuted(ActionExecutedContext context)
        {
            KretanjePoSistemu.Save(context.HttpContext);
        }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            if (filterContext.HttpContext.GetLoginInfo().isLogiran)
            {
                filterContext.Result = new UnauthorizedResult();
                return;
            }

            if (filterContext.HttpContext.GetLoginInfo().isPermisijaAdmin)
            {
                return;//ok - ima pravo pristupa
            }
            if (filterContext.HttpContext.GetLoginInfo().isPermisijaKorisnik)
            {
                if (filterContext.HttpContext.GetLoginInfo().autentifikacijaToken == null || filterContext.HttpContext.GetLoginInfo().autentifikacijaToken.twoFJelOtkljucano == false)
                {
                    filterContext.Result = new UnauthorizedObjectResult("potrebno je otkljucati login sa codom poslat na email " + filterContext.HttpContext.GetLoginInfo().korisnickiNalog.Korisnik.Email);
                    return;
                }
                //return;//ok - ima pravo pristupa
            }
            if (filterContext.HttpContext.GetLoginInfo().isPermisijaUposlenik && _uposlenik)
            {
                return;//ok - ima pravo pristupa
            }
          

            //else nema pravo pristupa
            filterContext.Result = new UnauthorizedResult();
        }
    }
}
