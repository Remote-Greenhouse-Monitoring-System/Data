namespace FirebaseNotificationClient;

public interface INotificationClient
{
    public void SendNotificationToUser(string key);
}