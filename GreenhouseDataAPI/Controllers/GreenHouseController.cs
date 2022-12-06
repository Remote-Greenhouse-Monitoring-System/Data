using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using WebSocketClients.Interfaces;

namespace GreenhouseDataAPI.Controllers;

[ApiController]
[Route("/Greenhouses/")]

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
            return Ok(greenHouses);
        }
        catch (Exception ex) {
            return StatusCode(500,ex.Message);
        }

    }
    
    
    //-	Adds a greenhouse object for the user with the queried username
    //-	Argument: a greenhouse object
    
    [HttpPost]
    [Route("{uid:long}")]
    public async Task<ActionResult> CreateGreenHouse([FromRoute] long uid, [FromBody] GreenHouse greenHouse) {
        try {
            await _greenHouseService.AddGreenHouse(uid, greenHouse);
            return Ok();
        }
        catch (Exception ex) {
            return StatusCode(500,ex.Message);
        }
    }
    
    //-	Updates an existing greenhouse
    //-	Argument: a greenhouse object

    [HttpPatch]
    public async Task<ActionResult> UpdateGreenHouse([FromBody] GreenHouse greenHouse) {
        try {
            await _greenHouseService.UpdateGreenHouse(greenHouse);
            return Ok();
        }
        catch (Exception ex) {
            return StatusCode(500,ex.Message);
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
            return StatusCode(500,ex.Message);
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
    
    [HttpGet]
    [Route("lastMeasurementGreenhouse")]
    public async Task<ActionResult<GreenHouse>> GetLastMeasurementGreenhouse()
    {
        try
        {
            GreenHouse greenHouse = await _greenHouseService.GetLastMeasurementGreenhouse();
            
            return Ok(greenHouse);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    [Route("greenhousesWithLastMeasurements")]
    public async Task<ActionResult<ICollection<GreenhouseLastMeasurement>>> GetGreenhouesWithLastMeasurements()
    {
        try
        {
            ICollection<GreenhouseLastMeasurement> greenhousesWithMeasurement = await _greenHouseService.GetGreenhousesWithMeasurement();
            
            return Ok(greenhousesWithMeasurement);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, e.Message);
        }
    }
}
    
