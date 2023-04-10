﻿using System.Net;
using System.Net.Mail;

namespace WebApplication1.Helper
{
    public class EmailSender
    {
        //2way autentifikacija
        public static void Send(string to, string messageSubject, string messageBody, bool isBodyHtml = false)
        {
            if (to == "")
                return;
            String SendMailFrom = "rsseminarski@gmail.com";

            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage email = new MailMessage();
            // START
            email.From = new MailAddress(SendMailFrom);
            email.To.Add(to);
            email.CC.Add(SendMailFrom);
            email.Subject = messageSubject;
            email.Body = messageBody;
            email.IsBodyHtml = isBodyHtml;
            //END
            SmtpServer.Timeout = 5000;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(SendMailFrom, "cwgrxtjommdslocs");
            SmtpServer.Send(email);
        }
    }
}
