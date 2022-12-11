using System.Threading.Tasks;
using Contracts;
using Entities;
using GreenhouseDataAPI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests;

[TestClass]
public class MeasurementControllerUnitTests
{
    [TestMethod]
    public async Task GetLastMeasurementReturnsLastMeasurement()
    {
        // Arrange
        var mockService = new Mock<IMeasurementService>();
        mockService.Setup(x => x.GetLastMeasurement(1))
            .ReturnsAsync(new Measurement());

        var controller = new MeasurementController(mockService.Object);

        // Act
        var actionResult = await controller.GetLastMeasurement(1);

        // Assert
        Assert.IsNotNull(actionResult);
        Assert.IsNotNull(actionResult.Result);
        Assert.AreEqual(typeof(Measurement), actionResult.Value.GetType());
    }
    
}