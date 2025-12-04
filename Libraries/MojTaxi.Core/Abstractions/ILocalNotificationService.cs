using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Core.Abstractions
{
    public interface ILocalNotificationService
    {
        void Show(string title, string message);
    }
}
