using Contracts;
using Entities;
using GreenhouseDataAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseDataAPI.Controllers;
[ApiController]
[Route("/Thresholds/")]
[ApiKey]
public class ThresholdController : ControllerBase
{
    private IThresholdService _service;

    public ThresholdController(IThresholdService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("get/{pId:long}")]
    public  async Task<ActionResult<Threshold>> GetThresholdForPlantProfile([FromRoute]long pId)
    {
        try
        {
            Threshold threshold = await _service.GetThresholdForPlantProfile(pId);
            return Ok(threshold);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch]
    [Route("update/{pId:long}")]
    public async Task<ActionResult> UpdateThresholdForPlantProfile([FromBody] Threshold threshold,[FromRoute] long pId)
    {
        try
        {
            await _service.UpdateThresholdOnPlantProfile(threshold, pId);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    [Route("activeThreshold/{gId:long}")]
    public async Task<ActionResult<Threshold>> UpdateThresholdForPlantProfile([FromRoute] long gId)
    {
        try
        {
            Threshold threshold = await _service.GetThresholdOnActivePlantProfile(gId);
            return Ok(threshold);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}