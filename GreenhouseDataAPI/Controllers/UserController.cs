using Contracts;
using Entities;
using GreenhouseDataAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseDataAPI.Controllers;
[ApiController]
[Route("/Users/")]
[ApiKey]
public class UserController : ControllerBase
{
    private IUserService _service;
    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("byUsername/{username}")]
    public async Task<ActionResult<User>> GetUserByUsername([FromRoute] string username)
    {
        try
        {
            User u = await _service.GetUserByUsername(username);
            return Ok(u);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("byEmail/{email}")]
    public async Task<ActionResult<User>> GetUserByEmail([FromRoute] string email)
    {
        try
        {
            User u = await _service.GetUserByEmail(email);
            return Ok(u);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("byId/{id:long}")]
    public async Task<ActionResult<User>> GetUserById([FromRoute] long id)
    {
        try
        {
            User u = await _service.GetUserById(id);
            return Ok(u);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    [Route("add")]
    public async Task<ActionResult<User>> AddUser([FromBody] User user)
    {
        try
        {
            User u=await _service.AddUser(user);
            return Ok(u);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete]
    [Route("remove/{id:long}")]
    public async Task<ActionResult<User>> RemoveUser([FromRoute] long id)
    {
        try
        {
            User u=await _service.RemoveUser(id);
            return Ok(u);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch]
    [Route("update")]
    public async Task<ActionResult<User>> UpdateUser([FromBody] User user)
    {
        try
        {
            User u = await _service.UpdateUser(user);
            return Ok(u);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }
    

    [HttpGet]
    [Route("login")]
    public async Task<ActionResult<User>> LogUserIn([FromQuery] string email, [FromQuery] string password)
    {
        try
        {
            User u = await _service.LogUserIn(email, password);
            return Ok(u);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }
}