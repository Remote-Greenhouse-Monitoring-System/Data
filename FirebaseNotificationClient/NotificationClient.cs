using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using FirebaseAdmin.Messaging;

namespace FirebaseNotificationClient;

public class NotificationClient : INotificationClient
{
    public async Task SendNotificationToUser(string token)
    {
        var message = new Message()
        {
            Notification = new Notification
            {
                Title = "Hello android!",
                Body = "Some notification body text, idk."
            },

            Token = token,
        };
        var messaging = FirebaseMessaging.DefaultInstance;
        var result = await messaging.SendAsync(message);
        Console.WriteLine(result);
    }
}