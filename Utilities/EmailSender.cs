using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace Utilities
{
    public class EmailSender:IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("alaa.moh.latif@gmail.com", "eeyl xdbd gige lwms")
            };

            var message = new MailMessage();
            message.From = new MailAddress("alaa.moh.latif@gmail.com");
            message.Subject = subject;
            message.To.Add(email);
            message.Body = $"<html><body>{htmlMessage}</body></html>";
            message.IsBodyHtml = true;

            client.Send(message);
        }


    }


    }
