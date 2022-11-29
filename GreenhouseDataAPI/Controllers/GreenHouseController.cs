using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using WebSocketClients.Interfaces;

namespace GreenhouseDataAPI.Controllers;

[ApiController]
[Route("/")]

public class GreenHouseController : ControllerBase {

    private readonly IGreenHouseService _greenHouseService;
    private IGreenhouseClient _greenhouseClient;

    public GreenHouseController(IGreenHouseService greenHouseService, IGreenhouseClient greenhouseClient) {
        _greenHouseService = greenHouseService;
        _greenhouseClient = greenhouseClient;
    }

    //TODO: we must handle security, and how users are getting data
    
    
    //-	Returns a list of greenhouse objects, corresponding to the user with the queried username.
    [HttpGet]
    [Route("{uid:long}")]
    public async Task<ActionResult<List<GreenHouse>>> GetGreenHouse(long uid) {
        try {
            ICollection<GreenHouse> greenHouses = await _greenHouseService.GetGreenHouses();
            //TODO I Need to filter the greenhouses by uid, but this should not be done in the controller,
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
    public async Task<ActionResult<GreenHouse>> CreateGreenHouse(long uid, [FromBody] GreenHouse greenHouse) {
        try {
            GreenHouse newGreenHouse = await _greenHouseService.CreateGreenHouse(greenHouse);
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
    public async Task<ActionResult<GreenHouse>> RemoveGreenHouse(long id) {
        try {
            await _greenHouseService.RemoveGreenHouse(id);
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

    [HttpPost]
    [Route("clientTest/{gId:long}")]
    public async Task<ActionResult> AddThresholdToGreenhouse([FromRoute] long gid, [FromBody] Threshold threshold)
    {
        try
        {
            await _greenhouseClient.SetThresholdToGreenhouse(gid, threshold);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }
}
    
