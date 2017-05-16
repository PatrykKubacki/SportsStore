using System.Net;
using System.Net.Mail;
using System.Text;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;

namespace SportsStore.Domain.Concrete
{

	public class EmailOrderProcesssor : IOrderProcessor
	{
		EmailSettings _emailSettings;

		public EmailOrderProcesssor(EmailSettings emailSettings)
		{
			_emailSettings = emailSettings;
		}

		public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
		{
			using (var smtpClient = new SmtpClient())
			{
				smtpClient.EnableSsl = _emailSettings.UseSsl;
				smtpClient.Port = _emailSettings.ServerPort;
				smtpClient.Host = _emailSettings.ServerName;
				smtpClient.UseDefaultCredentials = false;
				smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

				if (_emailSettings.WriteAsFile)
				{
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
					smtpClient.PickupDirectoryLocation = _emailSettings.FileLocation;
					smtpClient.EnableSsl = false;
				}

				var body = new StringBuilder();
				body.Append("Nowe zamówienie")
					.Append("---------")
					.Append("Produkty:");

				foreach (var line in cart.Lines)
				{
					var subTotal = line.Quantity * line.Product.Price;
					var product = $"Nazwa: {line.Product.Name}, Ilość:{line.Quantity} razem: {subTotal}";
					body.Append(product);
				}

				body.AppendFormat("Wartość całkowita: {0:c}", cart.ComputeTotalValue())
						.AppendLine("---")
						.AppendLine("Wysyłka dla:")
						.AppendLine(shippingDetails.Name)
						.AppendLine(shippingDetails.Line1)
						.AppendLine(shippingDetails.Line2 ?? "")
						.AppendLine(shippingDetails.Line3 ?? "")
						.AppendLine(shippingDetails.City)
						.AppendLine(shippingDetails.State ?? "")
						.AppendLine(shippingDetails.Country)
						.AppendLine(shippingDetails.Zip)
						.AppendLine("---")
						.AppendFormat($"Pakowanie prezentu {(shippingDetails.GiftWrap ? "Tak" : "Nie")}");

				var mailMessage = new MailMessage(_emailSettings.MailFromAddress, _emailSettings.MailToAddress, "Otrzymano nowe zamówienie", body.ToString());

				if (_emailSettings.WriteAsFile)
					mailMessage.BodyEncoding = Encoding.ASCII;

				smtpClient.Send(mailMessage);
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
}
