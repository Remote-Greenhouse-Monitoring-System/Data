using System.Net.Sockets;
using Newtonsoft.Json;

namespace WebSocketClients.Clients;

using Contracts;
using Entities;
using WebSocketClients.Interfaces;


public class MeasurementClient : IMeasurementClient
{

    private IMeasurementService? _measurementService;
    private StreamReader _streamReader;
    private StreamWriter _streamWriter;
    
    public MeasurementClient(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
        
        int port = 1234;
        TcpClient client = new TcpClient("localhost", port);      				
        NetworkStream stream = client.GetStream();
        _streamReader = new StreamReader(stream);
        _streamWriter = new StreamWriter(stream) { AutoFlush = true };
    }

    
    
    //managing a plant profile of a device/=greenhouse
    public void SetPlantProfile(PlantProfile plantProfile)
    {
        Console.Out.WriteLine("MOCK-plant profile "+plantProfile.Id+" -SET");
    }
    public void UpdatePlantProfile(PlantProfile plantProfile)
    {
        Console.Out.WriteLine("MOCK-plant profile "+plantProfile.Id+" -UPDATED");
    }
    public void DeletePlantProfile(PlantProfile plantProfile)
    {
        Console.Out.WriteLine("MOCK-plant profile "+plantProfile.Id+" -DELETED");
    }

    public async Task<PlantProfile> ClientTestPlantProfile()
    {
        PlantProfile plantProfile = new PlantProfile("name","dscrpt",1,2,3, 4);
        _streamWriter.WriteLine(JsonConvert.SerializeObject(plantProfile));
        Console.Out.WriteLine("sent: " + JsonConvert.SerializeObject(plantProfile));

        var received = _streamReader.ReadLine();
        Console.Out.WriteLine("received: " + received);
        return JsonConvert.DeserializeObject<PlantProfile>(received);
    }
    
    public async Task<Measurement> ClientTestMeasurements()
    {
        Measurement measurement = new Measurement(1,2,3,4);
        _streamWriter.WriteLine(JsonConvert.SerializeObject(measurement));
        Console.Out.WriteLine("sent: " + JsonConvert.SerializeObject(measurement));

        var received = _streamReader.ReadLine();
        Console.Out.WriteLine("received: " + received);
        return JsonConvert.DeserializeObject<Measurement>(received);
    }
}