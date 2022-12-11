namespace FirebaseNotificationClient;

public interface INotificationClient
{
    public Task SendNotificationToUser(string token,string title, string body);
}

