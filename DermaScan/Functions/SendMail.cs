using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace DermaScan.Functions
{
    public class SendMail
    {
        public static void Appointment(string appointment, string url, string email)
        {
            var fromAddress = new MailAddress("doctorscraper2000@gmail.com", "Web Scraper");
            var toAddress = new MailAddress(email, "My Name");
            var fromPassword = Password.Pass();
            const string subject = "Appointment was found!";
            var body = "An appointment has become available on: " + appointment + "\n" + "Link: " + url;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            smtp.Send(message);
        }
    }
}