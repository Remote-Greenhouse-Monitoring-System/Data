using Contracts;
using Entities;
using GreenhouseDataAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseDataAPI.Controllers;

[ApiController]
[Route("/Measurement/")]
[ApiKey]
public class MeasurementController : ControllerBase
{
    private readonly IMeasurementService? _measurementService;
    public MeasurementController(IMeasurementService? measurementService)
    {
        _measurementService = measurementService;
    }

    [HttpGet]
    [Route("all/{gId:long}/{amount:int}")]
    public async Task<ActionResult<List<Measurement>>> GetMeasurements([FromRoute] long gId, [FromRoute] int amount)
    {
        try
        {
            ICollection<Measurement> measurements = await _measurementService!.GetMeasurements(gId, amount);
            return Ok(measurements);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("last/{gId:long}")]
    public async Task<ActionResult<Measurement>> GetLastMeasurement([FromRoute] long gId)
    {
        try
        {
            Measurement measurement = await _measurementService!.GetLastMeasurement(gId);
            return Ok(measurement);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("allPerHours/{gId:long}/{hours:int}")]
    public async Task<ActionResult<List<Measurement>>> GetAllPerHours([FromRoute] long gId, [FromRoute] int hours)
    {
        try
        {
            ICollection<Measurement> measurements = await _measurementService!.GetAllPerHours(gId,hours);
            return Ok(measurements);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    [Route("allPerDays/{gId:long}/{days:int}")]
    public async Task<ActionResult<List<Measurement>>> GetAllPerDays([FromRoute] long gId, [FromRoute] int days)
    {
        try
        {
            ICollection<Measurement> measurements = await _measurementService!.GetAllPerDays(gId,days);
            return Ok(measurements);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    [Route("allPerMonth/{gId:long}/{month:int}/{year:int}")]
    public async Task<ActionResult<List<Measurement>>> GetAllPerMonth([FromRoute] long gId, [FromRoute] int month,[FromRoute] int year)
    {
        try
        {
            ICollection<Measurement> measurements = await _measurementService!.GetAllPerMonth(gId,month,year);
            return Ok(measurements);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    [Route("allPerYear/{gId:long}/{year:int}")]
    public async Task<ActionResult<List<Measurement>>> GetAllPerYear([FromRoute] long gId, [FromRoute] int year)
    {
        try
        {
            ICollection<Measurement> measurements = await _measurementService!.GetAllPerYear(gId,year);
            return Ok(measurements);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
            return StatusCode(500, e.Message);
        }
    }
    //testing the DAO functionality of adding to every greenhouse with a specific EUI
    [HttpPost]
    [Route("addWithEUITest")]
    public async Task<ActionResult> AddMeasurementWithEUI([FromBody] Measurement measurement,[FromQuery] string EUI)
    {
        try
        {
            await _measurementService.AddMeasurementWithEUI(measurement, EUI);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    } 
}

