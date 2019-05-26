using GameStore.Domain.Entities;
using GameStore.Web.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace GameStore.Web.Services.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "gamestore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = "game_store_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private readonly EmailSettings _emailSettings;
        private readonly ICartProvider _cartProvider;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EmailOrderProcessor(EmailSettings settings, ICartProvider cartProvider, IHostingEnvironment hostingEnvironment)
        {
            _emailSettings = settings;
            _cartProvider = cartProvider;
            _hostingEnvironment = hostingEnvironment;
        }

        public void ProcessOrder(ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = _emailSettings.UseSsl;
                smtpClient.Host = _emailSettings.ServerName;
                smtpClient.Port = _emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                if (_emailSettings.WriteAsFile)
                {
                    var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, _emailSettings.FileLocation);
                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }
                    smtpClient.DeliveryMethod
                        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = fullPath;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("Новый заказ обработан")
                    .AppendLine("---")
                    .AppendLine("Товары:");

                foreach (var line in _cartProvider.Lines)
                {
                    var subtotal = line.Game.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (итого: {2:c}",
                        line.Quantity, line.Game.Name, subtotal);
                }

                body.AppendFormat("Общая стоимость: {0:c}", _cartProvider.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Доставка:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.Line3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.Country)
                    .AppendLine("---")
                    .AppendFormat("Подарочная упаковка: {0}",
                        shippingInfo.GiftWrap ? "Да" : "Нет");

                MailMessage mailMessage = new MailMessage(
                                       _emailSettings.MailFromAddress,	// От кого
                                       _emailSettings.MailToAddress,		// Кому
                                       "Новый заказ отправлен!",		// Тема
                                       body.ToString()); 				// Тело письма

                if (_emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }
}