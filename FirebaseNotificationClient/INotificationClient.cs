namespace FirebaseNotificationClient;

public interface INotificationClient
{
    public Task SendNotificationToUser(string token);
}

