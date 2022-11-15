using Entities;

namespace WebSocketClients.Interfaces;

public interface IMeasurementClient
{
    void SetPlantProfile(PlantProfile plantProfile);
    void UpdatePlantProfile(PlantProfile plantProfile);
    void DeletePlantProfile(PlantProfile plantProfile);
    Task<PlantProfile> ClientTestPlantProfile();
    Task<Measurement> ClientTestMeasurements();
}