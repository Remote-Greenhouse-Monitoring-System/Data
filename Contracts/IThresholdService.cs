using Entities;

namespace Contracts;

public interface IThresholdService
{
    public Task<Threshold> GetThresholdForPlantProfile(long plantProfileId);
    public Task UpdateThresholdOnPlantProfile(Threshold threshold, long pId);

    public Task<Threshold> GetThresholdOnActivePlantProfile(long gId);
}