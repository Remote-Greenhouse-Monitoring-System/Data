using Contracts;
using Entities;
using GreenhouseDataAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseDataAPI.Controllers;

[ApiController]
[Route("/PlantProfiles/")]
[ApiKey]
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
        try
        {
            plantP.Threshold = new Threshold();
            PlantProfile newPlantProfile = await _plantProfileService.AddPlantProfile(plantP,uId);
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

    [HttpGet]
    [Route("plantProfilesForUser/{uId:long}")]
    public async Task<ActionResult<ICollection<PlantProfile>>> GetUserPlantProfile([FromRoute] long uId)
    {
        try {
            ICollection<PlantProfile> plantProfiles = await _plantProfileService.GetUserPlantProfiles(uId);
            return Ok(plantProfiles);
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<PlantProfile>>> GetPremadePlantProfiles()
    {
        try {
            ICollection<PlantProfile> pPlantProfiles = await _plantProfileService.GetPreMadePlantProfiles();
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
    [Route("activate/{pId:long}/{gId:long}")]
    public async Task<ActionResult> ActivatePlantProfile([FromRoute] long pId, [FromRoute] long gId)
    {
        try {
            await _plantProfileService.ActivatePlantProfile(pId,gId);
            return Ok();
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }
    
    [HttpGet]
    [Route("activated/{gId:long}")]
    public async Task<ActionResult<PlantProfile>> GetActivePlantProfileOnGreenhouse([FromRoute] long gId)
    {
        try {
            PlantProfile plantProfile = await _plantProfileService.GetActivePlantProfileOnGreenhouse(gId);
            return Ok(plantProfile);
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }


}