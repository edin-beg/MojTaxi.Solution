using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MojTaxi.ClientApp.Services.Auth
{

    public sealed class EmailOtpSender : IOtpSender
    {
        // TODO: prebaci u settings
        private const string SmtpHost = "mail.it-craft.ba";
        private const int SmtpPort = 465;
        private const string Username = "otp@it-craft.ba";
        private const string Password = "H33m3m426!*";

        public async Task SendAsync(string email, string otp)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Username, "MojTaxi"),
                Subject = "MojTaxi – kod za prijavu",
                Body = $"Vaš MojTaxi kod za prijavu je: {otp}",
                IsBodyHtml = false
            };

            message.To.Add(email);

            using var client = new SmtpClient(SmtpHost, SmtpPort)
            {
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}
