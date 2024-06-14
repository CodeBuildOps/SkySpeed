using System.Net;
using System.Net.Mail;

namespace SkySpeedService.Email
{
    internal class SendEmail
    {
        private const string fromMail = "abhiksingh1999@gmail.com"; /*"skyspeed.cu@gmail.com";*/
        private const string fromPassword = "uqha jdiw knbo bawu";

        public bool EmailSend(string toMail, string htmlContent)
        {
            bool isMailSent = false;
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(fromMail);
                message.Subject = "E-Ticket for your flight PNR: ABCD1234";
                message.To.Add(new MailAddress(toMail));
                message.Body = htmlContent;
                message.IsBodyHtml = true;

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                    smtpClient.EnableSsl = true;

                    smtpClient.Send(message);

                    isMailSent = true;
                }
            }
            return isMailSent;
        }
    }
}