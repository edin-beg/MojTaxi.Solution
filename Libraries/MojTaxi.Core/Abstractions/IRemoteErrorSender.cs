using MojTaxi.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Core.Abstractions
{
    public interface IRemoteErrorSender
    {
        Task<bool> SendAsync(ErrorEntry entry);
    }
}
