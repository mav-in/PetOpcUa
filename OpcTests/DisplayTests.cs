using System.Text.Json;
using FluentAssertions;
using OpcDomain;

namespace OpcTests;

public class DisplayTests
{
    [Fact]
    public void Mode_Enum_Parsing()
    {
        var tag = new OpcTag
        {
            DisplayName = "Mixer1.Mode",
            DataType = "Int32",
            Attributes = new Attributes {
                EnumStrings = ["Off", "Manual", "Automatic", "Maintenance"]
            },
            Value = JsonDocument.Parse("2").RootElement,
            StatusCode = "Good"
        };

        var (display, _) = ValueFormat.ToDisplay(tag);

        display.Should().Be("Automatic");
    }

    [Theory]
    [InlineData("GoodClamp", Constants.QualityGroup.Good)]
    [InlineData("BadOutOfService", Constants.QualityGroup.Bad)]
    [InlineData("UncertainLastUsableValue", Constants.QualityGroup.Uncertain)]
    public void Status_Grouping(string status, Constants.QualityGroup expected)
    {
        Quality.GroupOf(status).Should().Be(expected);
    } 

    [Fact]
    public void NoSuchTag_NullValue()
    {
        var tag = new OpcTag { DisplayName = "NoSuchTag", StatusCode = "BadNodeIdUnknown" };
        
        var (display, dt) = ValueFormat.ToDisplay(tag);
        
        display.Should().Be("—");

        dt.Should().BeNull();
    }

    [Fact]
    public void Age_From_ServerTimestamp()
    {
        var now = DateTime.Parse("2025-09-07T12:00:10Z");
        
        var tag = new OpcTag {
            ServerTimestamp = DateTime.Parse("2025-09-07T12:00:00Z")
        };
        var age = ValueFormat.AgeUtc(tag, now);

        age.Should().NotBeNull();

        age!.Value.TotalSeconds.Should().Be(10);
    }

    [Fact]
    public void TrendLevel_Array_Length()
    {
        var json = "[1,2,3,4,5]";

        var tag = new OpcTag
        {
            DisplayName = "Tank1.TrendLevel",
            DataType = "Int32",
            ArrayType = "Array",
            Value = JsonDocument.Parse(json).RootElement
        };

        var (display, _) = ValueFormat.ToDisplay(tag);

        display.Should().Contain("len=5");
    }
}
