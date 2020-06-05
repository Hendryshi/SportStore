using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
	public class EmailSettings
	{
		public string MailToAddress = "orders@example.com";
		public string MailFromAddress = "sportsstore@example.com";
		public bool useSsl = true;
		public string Usersame = "MySmtpUsername";
		public string Password = "MySmtpPassword";
		public string Servername = "smtp.example.com";
		public int ServerPort = 587;
		public bool WriteAsFile = false;
		public string FileLocation = @"D:\WorkSpace\ASP.Net MVC\DevTest\SportsStore\Documents";
	}


	public class EmailOrderProcessor : IOrderProcessor
	{
		private EmailSettings emailSettings;

		public EmailOrderProcessor(EmailSettings settings)
		{
			emailSettings = settings;
		}

		public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
		{
			using(var smtpClient = new SmtpClient())
			{
				smtpClient.EnableSsl = emailSettings.useSsl;
				smtpClient.Host = emailSettings.Servername;
				smtpClient.Port = emailSettings.ServerPort;
				smtpClient.UseDefaultCredentials = false;
				smtpClient.Credentials = new NetworkCredential(emailSettings.Usersame, emailSettings.Password);
				
				if(emailSettings.WriteAsFile)
				{
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
					smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
					smtpClient.EnableSsl = false;
				}

				StringBuilder body = new StringBuilder().AppendLine("A new order has been submitted").AppendLine("---").AppendLine("Items:");

				foreach(var line in cart.Lines())
				{
					var subtotal = line.Product.Price * line.Quantity;
					body.AppendFormat("{0} * {1} (subtotal : {2:c}", line.Quantity, line.Product.Name, subtotal);
				}

				body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
					.AppendLine("---")
					.AppendLine("Ship to:")
					.AppendLine(shippingDetails.Line1)
					.AppendLine(shippingDetails.Line2 ?? "")
					.AppendLine(shippingDetails.Line3 ?? "")
					.AppendLine(shippingDetails.City)
					.AppendLine(shippingDetails.State ?? "")
					.AppendLine(shippingDetails.Country)
					.AppendLine(shippingDetails.Zip)
					.AppendLine("---")
					.AppendFormat("Gift wrap: {0}", shippingDetails.GiftWrap ? "Yes" : "No");

				MailMessage mailMessage = new MailMessage(emailSettings.MailFromAddress, emailSettings.MailToAddress, "New order submitted !", body.ToString());

				if(emailSettings.WriteAsFile)
					mailMessage.BodyEncoding = Encoding.ASCII;
				
				smtpClient.Send(mailMessage);
			}
		}
	}
}
