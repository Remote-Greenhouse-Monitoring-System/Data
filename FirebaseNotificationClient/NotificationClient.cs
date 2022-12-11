using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using FirebaseAdmin.Messaging;

namespace FirebaseNotificationClient;

public class NotificationClient : INotificationClient
{
    public async Task SendNotificationToUser(string token, string title, string body)
    {
        var message = new Message()
        {
            Notification = new Notification
            {
                Title = title,
                Body = body
            },

            Token = token,
        };
        var messaging = FirebaseMessaging.DefaultInstance;
        var result = await messaging.SendAsync(message);
        Console.WriteLine(result);
    }
}