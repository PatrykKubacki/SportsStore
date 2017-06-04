using System.Net;
using System.Net.Mail;
using SportsStore.Domain.Abstract;

namespace SportsStore.Domain.Concrete
{
    public class EmailSender : IEmailSender
    {
        EmailSettings _settings;

        public EmailSender(EmailSettings settings)
        {
            _settings = settings;
        }

        public void SendMessage(string email,string subject, string body)
        {
            using (var smtpClinet = new SmtpClient())
            {
                smtpClinet.EnableSsl = _settings.UseSsl;
                smtpClinet.Port = _settings.ServerPort;
                smtpClinet.Host = _settings.ServerName;
                smtpClinet.UseDefaultCredentials = false;
                smtpClinet.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

                var mailMessage = new MailMessage(_settings.MailFromAddress, email, subject, body);
                smtpClinet.Send(mailMessage);
            } 
        }

    }

    public class EmailSettings
    {
        public string MailToAddress = "zamowienie@o2.pl";
        public string MailFromAddress = "TestowyAdresEmail@o2.pl";
        public bool UseSsl = true;
        public string Username = "TestowyAdresEmail@o2.pl";
        public string Password = "Zaq12wsx";
        public string ServerName = "poczta.o2.pl";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:/SportsStore";
    }

}