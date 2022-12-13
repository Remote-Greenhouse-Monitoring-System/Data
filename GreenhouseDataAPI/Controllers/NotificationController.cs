using Contracts;
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
    private IUserService _userService;
    public NotificationController(INotificationClient notificationClient, IUserService userService)
    {
        _notificationClient = notificationClient;
        _userService = userService;
    }

    [HttpPost]
    [Route("register/{token}/{uId:long}")]
    public async Task<ActionResult> RegisterForNotifications([FromRoute] string token, [FromRoute] long uId)
    {
        try
        {   
            //uncomment line below to test notifications directly
            // await _notificationClient.SendNotificationToUser(token,"Hello","android team!");
            await _userService.SetTokenForUser(uId,token);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    [HttpPatch]
    [Route("unregister/{uId:long}")]
    public async Task<ActionResult> UnregisterUser([FromRoute] long uId)
    {
        try
        {
            await _userService.RemoveTokenFromUser(uId);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }
}