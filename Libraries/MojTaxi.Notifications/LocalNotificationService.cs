
using MojTaxi.Core.Abstractions;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Notifications
{

    public class LocalNotificationService : ILocalNotificationService
    {
        public void Show(string title, string message)
        {
            var request = new NotificationRequest
            {
                NotificationId = new Random().Next(1000, 9999),
                Title = title,
                Description = message
            };

            LocalNotificationCenter.Current.Show(request);
        }
    }
}
