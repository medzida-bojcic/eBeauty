using System.Globalization;

namespace WebApplication1.Helper.ErrorHandler
{
    public class ErrorResponse : Exception
    {
        public int statusCode { get; set; }
        public string message { get; set; }

        public ErrorResponse() : base() { }

        public ErrorResponse(string message) : base(message) { }

        public ErrorResponse(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
