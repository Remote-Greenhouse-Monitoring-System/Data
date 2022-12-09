using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Contracts;
using Entities;
using GreenhouseDataAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assert = NUnit.Framework.Assert;

namespace Tests;

[TestClass]
public class MeasurementControllerUnitTests
{
    [TestMethod]
    public void GetLastMeasurementReturnsLastMeasurement()
    {
        // Arrange
        var mockService = new Mock<IMeasurementService>();
        mockService.Setup(x => x.GetLastMeasurement(1))
            .ReturnsAsync(new Measurement());

        var controller = new MeasurementController(mockService.Object);

        // Act
        ActionResult<Measurement> actionResult = controller.GetLastMeasurement(1).Result;

        // Assert
        Assert.IsNotNull(actionResult);
        Assert.IsNotNull(actionResult.Result);
        // Assert.IsTrue(actionResult.Result?.GetType() == typeof(Measurement));
    }
    
}