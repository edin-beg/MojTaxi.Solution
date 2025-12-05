using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Core.Abstractions
{
    public interface IErrorLogger
    {
        Task LogAsync(Exception ex);
    }
}
