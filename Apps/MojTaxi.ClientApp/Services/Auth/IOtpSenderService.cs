using MojTaxi.ClientApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.ClientApp.Services.Auth
{

    public interface IOtpSenderService
    {
        Task SendOtpAsync(LoginMethod method, string destination);
    }
}
