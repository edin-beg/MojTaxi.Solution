using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.ClientApp.Services.Auth
{
    public interface IOtpSender
    {
        Task SendAsync(string destination, string otp);
    }
}
