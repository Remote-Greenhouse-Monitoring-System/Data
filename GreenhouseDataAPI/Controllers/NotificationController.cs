using FirebaseNotificationClient;
using GreenhouseDataAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseDataAPI.Controllers;
[ApiController]
[ApiKey]
[Route("/Notifications/")]
public class NotificationController : ControllerBase
{
    private INotificationClient _notificationClient;

    public NotificationController(INotificationClient notificationClient)
    {
        _notificationClient = notificationClient;
    }

    [HttpPost]
    [Route("register/{token}/{uId:long}")]
    public async Task<ActionResult> RegisterForNotifications([FromRoute] string token, [FromRoute] long uId)
    {
        try
        {
            await _notificationClient.SendNotificationToUser(token);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}