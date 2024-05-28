using SkySpeedService.PNR;
using System;
using System.Net;
using System.Net.Mail;

namespace SkySpeedService.Email
{
    class SendEmail : HTMLBody
    {
        private const string fromMail = "abhiksingh1999@gmail.com"; /*"skyspeed.cu@gmail.com";*/
        private const string fromPassword = "uqha jdiw knbo bawu";
        private readonly string toMail;

        public SendEmail(string toMail)
        {
            this.toMail = toMail;
        }

        public bool IsEmailSent()
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(fromMail);
                    message.Subject = "E-Ticket for your flight PNR: ABCD1234";
                    message.To.Add(new MailAddress(toMail));
                    message.Body = _htmlBody;
                    message.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.Port = 587;
                        smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                        smtpClient.EnableSsl = true;

                        smtpClient.Send(message);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}