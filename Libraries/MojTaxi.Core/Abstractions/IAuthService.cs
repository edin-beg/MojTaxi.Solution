using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Core.Abstractions
{
    public interface IAuthService
    {
        Task<bool> TryRestoreAsync();
        Task LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
