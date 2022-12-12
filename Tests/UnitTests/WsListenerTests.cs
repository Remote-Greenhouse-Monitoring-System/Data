using System;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WsListenerBackgroundService;

namespace Tests.UnitTests;

[TestClass]
public class WsListenerTests
{

    // [TestMethod]
    //using reflections to test private methods -fail
    // public void GetHexStringFromThresholdReturnsCorrectString()
    // {
    //     var bl = new BackgroundListener(new Mock<IServiceProvider>().Object);
    //
    //     var methodInfo = typeof(BackgroundListener).GetMethod("GetHexStringFromThreshold",
    //         BindingFlags.NonPublic | BindingFlags.Instance);
    //     
    //     var t = new Threshold{TemperatureMin = 1,TemperatureMax = 1, Co2Min = 1,Co2Max = 1, HumidityMax = 1,HumidityMin = 1};
    //
    //     object[] parameters = { t };
    //     var result = (string)methodInfo.Invoke(bl, parameters);
    //     
    //     Assert.AreEqual("000000000000", result);
    // }

    [TestMethod]
    public void GetMeasurementFromReceivedDataReturnsMeasurement()
    {
        var m = BackgroundListener.GetMeasurementFromReceivedData("00ef01c5060d10");
        Assert.AreEqual( 23.9, Math.Round(m.Temperature, 1));
        Assert.AreEqual(45.3, Math.Round(m.Humidity), 1);
        Assert.AreEqual(1549, m.Co2);
    }
    
    [TestMethod]
    public void GetHexStringFromThresholdReturnsCorrectString()
    {
        var t = new Threshold{TemperatureMax = 5.5f, TemperatureMin = 1.5f,HumidityMax = 89.3f,HumidityMin = 24.5f, Co2Max = 123,Co2Min = 111};
        var result = BackgroundListener.GetHexStringFromThreshold(t);
        
        Assert.AreEqual("0037000F037D00F5007B006F", result);
    }

    [TestMethod]
    public void GetStatusFromReceivedDataReturnsNewStatus()
    {
        var newStatusBits = BackgroundListener.GetStatusFromReceivedData("00ef01c5060d10");
        Assert.AreEqual("00010000", newStatusBits);
    }

    [TestMethod]
    public void GetChangedActionsReturnsChangedActions()
    {
        const string lastStatusBits = "00001111";
        const string newStatusBits = "00000000";
        var changedActions = BackgroundListener.GetChangedActions(lastStatusBits, newStatusBits);

        Assert.AreEqual(4, changedActions.Count);
    }
    
    [TestMethod]
    public void LightActionTurnedOn()
    {
        const string lastStatusBits = "00000000";
        const string newStatusBits = "00001000";
        var changedActions = BackgroundListener.GetChangedActions(lastStatusBits, newStatusBits);

        Assert.AreEqual("Light-action turned ON", changedActions[0]);
    }
    [TestMethod]
    public void LightActionTurnedOff()
    {
        const string lastStatusBits = "00001000";
        const string newStatusBits = "00000000";
        var changedActions = BackgroundListener.GetChangedActions(lastStatusBits, newStatusBits);

        Assert.AreEqual("Light-action turned OFF", changedActions[0]);
    }
    
    [TestMethod]
    public void Co2ActionTurnedOn()
    {
        const string lastStatusBits = "00000000";
        const string newStatusBits = "00000100";
        var changedActions = BackgroundListener.GetChangedActions(lastStatusBits, newStatusBits);

        Assert.AreEqual("Co2-action turned ON", changedActions[0]);
    }
    [TestMethod]
    public void Co2ActionTurnedOff()
    {
        const string lastStatusBits = "00000100";
        const string newStatusBits = "00000000";
        var changedActions = BackgroundListener.GetChangedActions(lastStatusBits, newStatusBits);

        Assert.AreEqual("Co2-action turned OFF", changedActions[0]);
    }
    
    [TestMethod]
    public void HumidityActionTurnedOn()
    {
        const string lastStatusBits = "00000000";
        const string newStatusBits = "00000010";
        var changedActions = BackgroundListener.GetChangedActions(lastStatusBits, newStatusBits);

        Assert.AreEqual("Humidity-action turned ON", changedActions[0]);
    }
    [TestMethod]
    public void HumidityActionTurnedOff()
    {
        const string lastStatusBits = "00000010";
        const string newStatusBits = "00000000";
        var changedActions = BackgroundListener.GetChangedActions(lastStatusBits, newStatusBits);

        Assert.AreEqual("Humidity-action turned OFF", changedActions[0]);
    }
    
    [TestMethod]
    public void TemperatureActionTurnedOn()
    {
        const string lastStatusBits = "00000000";
        const string newStatusBits = "00000001";
        var changedActions = BackgroundListener.GetChangedActions(lastStatusBits, newStatusBits);

        Assert.AreEqual("Temperature-action turned ON", changedActions[0]);
    }
    [TestMethod]
    public void TemperatureActionTurnedOff()
    {
        const string lastStatusBits = "00000001";
        const string newStatusBits = "00000000";
        var changedActions = BackgroundListener.GetChangedActions(lastStatusBits, newStatusBits);

        Assert.AreEqual("Temperature-action turned OFF", changedActions[0]);
    }
}