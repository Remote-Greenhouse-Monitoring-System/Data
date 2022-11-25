using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseDataAPI.Controllers;

[ApiController]
[Route("/PlantProfiles/")]

public class PlantProfileController : ControllerBase
{
    private readonly IPlantProfileService _plantProfileService;

    public PlantProfileController(IPlantProfileService plantProfileService)
    {
        _plantProfileService = plantProfileService;
    }

    [HttpPost]
    [Route("add/{uId:long}")]
    public async Task<ActionResult<PlantProfile>> CreatePlantProfile([FromRoute] long uId, [FromBody] PlantProfile plantP)
    {
        try {
            PlantProfile newPlantProfile = await _plantProfileService.CreatePlantProfile(plantP);
            return Ok(newPlantProfile);
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }

    [HttpDelete]
    [Route("remove/{pId:long}")]
    public async Task<ActionResult<PlantProfile>> RemovePlantProfile([FromRoute] long pId)
    {
        try { 
            await _plantProfileService.RemovePlantProfile(pId);
            return Ok();
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }

    [HttpPatch]
    [Route("update/")]
    public async Task<ActionResult<PlantProfile>> UpdatePlantProfile([FromBody] PlantProfile plantP)
    {
        try { 
            await _plantProfileService.UpdatePlantProfile(plantP);
            return Ok();
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }
/*
//User not implemented yet
    [HttpGet]
    [Route("UserPlantP/{uId:long}")]
    public async Task<ActionResult<ICollection<PlantProfile>>> GetUserPlantProfile([FromRoute] long uId)
    {
        try {
            ICollection<PlantProfile> plantProfiles = await _plantProfileService.GetUserPlantProfile(uId);
            return Ok(plantProfiles);
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }
*/
    [HttpGet]
    public async Task<ActionResult<ICollection<PlantProfile>>> GetPremadePlantProfiles()
    {
        try {
            ICollection<PlantProfile> pPlantProfiles = await _plantProfileService.GetPremadePlantProfiles();
            return Ok(pPlantProfiles);
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }

    [HttpGet]
    [Route("PlantP/{pId:long}")]
    public async Task<ActionResult<PlantProfile>> GetPlantProfileById([FromRoute] long pId)
    {
        try {
            PlantProfile plantProfile = await _plantProfileService.GetPlantProfileById(pId);
            return Ok(plantProfile);
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }

    [HttpPatch]
    [Route("ActPP/{pId:long}")]
    public async Task<ActionResult> ActivatePlantProfile([FromRoute] long pId)
    {
        try {
            await _plantProfileService.ActivatePlantProfile(pId);
            return Ok();
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }

}