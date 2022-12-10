using Contracts;
using Entities;
using GreenhouseDataAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
namespace GreenhouseDataAPI.Controllers;

[ApiController]
[Route("/Greenhouses/")]
[ApiKey]
public class GreenHouseController : ControllerBase {

    private readonly IGreenHouseService _greenHouseService;

    public GreenHouseController(IGreenHouseService greenHouseService) {
        _greenHouseService = greenHouseService;
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
            return StatusCode(404,ex.Message);
        }

    }
    
    
    //-	Adds a greenhouse object for the user with the queried username
    //-	Argument: a greenhouse object
    
    [HttpPost]
    [Route("{uid:long}")]
    public async Task<ActionResult<GreenHouse>> CreateGreenHouse([FromRoute] long uid, [FromBody] GreenHouse greenHouse) {
        try {
            GreenHouse g= await _greenHouseService.AddGreenHouse(uid, greenHouse);
            return Ok(g);
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
            GreenHouse g = await _greenHouseService.UpdateGreenHouse(greenHouse);
            return Ok(g);
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
            GreenHouse g=await _greenHouseService.RemoveGreenHouse(gid);
            return Ok(g);
        }
        catch (Exception ex) {
            return StatusCode(500,ex.Message);
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
    [Route("greenhousesWithLastMeasurements/{uId:long}")]
    public async Task<ActionResult<ICollection<GreenhouseLastMeasurement>>> GetGreenhousesWithLastMeasurements(long uId)
    {
        try
        {
            ICollection<GreenhouseLastMeasurement> greenhousesWithMeasurement = await _greenHouseService.GetGreenhousesWithLastMeasurement(uId);
            
            return Ok(greenhousesWithMeasurement);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(404, e.Message);
        }
    }
}
    
