using System.Collections;
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
            PlantProfile profile = await _plantProfileService.RemovePlantProfile(pId);
            return Ok(profile);
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
            PlantProfile profile = await _plantProfileService.UpdatePlantProfile(plantP);
            return Ok(profile);
        }
        catch (Exception e) {
            return StatusCode(500,e.Message);
        }
    }

    [HttpGet]
    [Route("plantProfilesForUser/{uId:long}")]
    public async Task<ActionResult<ICollection<PlantProfile>>> GetUserPlantProfile([FromRoute] long uId)
    {
        List<PlantProfile> allProfiles = new List<PlantProfile>();
        try {
            ICollection<PlantProfile> plantProfiles = await _plantProfileService.GetUserPlantProfiles(uId);
            ICollection<PlantProfile> preMadePlantProfiles = await _plantProfileService.GetPreMadePlantProfiles();
            allProfiles.AddRange(preMadePlantProfiles);
            allProfiles.AddRange(plantProfiles);
            return Ok(allProfiles);
        }
        catch (Exception e) {
            return StatusCode(505,e.Message);
        }
    }

    [HttpGet]
    [Route("preMadeProfiles")]
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
            return StatusCode(404,e.Message);
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
    [HttpPatch]
    [Route("deactivate/{gId:long}")]
    public async Task<ActionResult> DeActivatePlantProfile([FromRoute] long gId)
    {
        try {
            await _plantProfileService.DeActivatePlantProfile(gId);
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
            return StatusCode(404,e.Message);
        }
    }


}