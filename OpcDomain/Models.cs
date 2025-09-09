using static OpcDomain.Constants;

namespace OpcDomain;

public class OpcRead
{
    public string EndpointUrl { get; set; } = string.Empty;
    public DateTime ReadAt { get; set; }
    public List<OpcTag> Results { get; set; } = [];
}

public class OpcTag
{
    public string? NodeId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? DataType { get; set; }
    public string? DataTypeId { get; set; }
    public string? ArrayType { get; set; }
    public int[]? ArrayDimensions { get; set; }
    public System.Text.Json.JsonElement Value { get; set; }
    public string StatusCode { get; set; } = string.Empty;
    public DateTime? SourceTimestamp { get; set; }
    public DateTime? ServerTimestamp { get; set; }
    public Attributes? Attributes { get; set; }
    public string[]? EnumStrings { get; set; }
}

public class Attributes
{
    public string[]? EnumStrings { get; set; }
    public System.Text.Json.JsonElement[]? EnumValues { get; set; }
}

public static class Quality
{
    public static QualityGroup GroupOf(string statusCode)
    {
        if (statusCode.StartsWith("Good", StringComparison.OrdinalIgnoreCase))
            return QualityGroup.Good;

        if (statusCode.StartsWith("Uncertain", StringComparison.OrdinalIgnoreCase))
            return QualityGroup.Uncertain;

        return QualityGroup.Bad;
    }
}
