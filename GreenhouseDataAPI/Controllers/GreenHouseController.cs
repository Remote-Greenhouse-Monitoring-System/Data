using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using WebSocketClients.Interfaces;

namespace GreenhouseDataAPI.Controllers;

[ApiController]
[Route("/greenhouses/")]

public class GreenHouseController : ControllerBase {

    private readonly IGreenHouseService _greenHouseService;
    private IGreenhouseClient _greenhouseClient;

    public GreenHouseController(IGreenHouseService greenHouseService, IGreenhouseClient greenhouseClient) {
        _greenHouseService = greenHouseService;
        _greenhouseClient = greenhouseClient;
    }
    
    
    //-	Returns a list of greenhouse objects, corresponding to the user with the queried username.
    [HttpGet]
    [Route("{uid:long}")]
    public async Task<ActionResult<List<GreenHouse>>> GetGreenHouses([FromRoute] long uid) {
        try {
            ICollection<GreenHouse> greenHouses = await _greenHouseService.GetGreenHouses(uid);
            // Change the service to return only the greenhouses of the user ?
            return Ok(greenHouses);
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }

    }
    
    //-	Adds a greenhouse object for the user with the queried username
    //-	Argument: a greenhouse object
    
    [HttpPost]
    [Route("{uid:long}")]
    public async Task<ActionResult<GreenHouse>> CreateGreenHouse([FromRoute] long uid, [FromBody] GreenHouse greenHouse) {
        try {
            GreenHouse newGreenHouse = await _greenHouseService.CreateGreenHouse(uid, greenHouse);
            return Ok(newGreenHouse);
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
    
    //-	Updates an existing greenhouse
    //-	Argument: a greenhouse object

    [HttpPatch]
    public async Task<ActionResult<GreenHouse>> UpdateGreenHouse([FromBody] GreenHouse greenHouse) {
        try {
            GreenHouse updatedGreenHouse = await _greenHouseService.UpdateGreenHouse(greenHouse);
            return Ok(updatedGreenHouse);
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
    
    //-	Deletes the greenhouse with the queried ID
    //-	Argument: a greenhouse object
    [HttpDelete]
    [Route("{gid:long}")]
    public async Task<ActionResult> RemoveGreenHouse([FromRoute] long gid) {
        try {
            await _greenHouseService.RemoveGreenHouse(gid);
            return Ok();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
    
    
    //ws-client test
    [HttpGet]
    [Route("clientTest/")]
    public async Task<ActionResult> ClientTest()
    {
        try
        {
            await _greenhouseClient.WsClientTest();
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }

}
    
