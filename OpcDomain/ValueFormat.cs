using System.Text.Json;

namespace OpcDomain;

public static class ValueFormat
{
    //TODO cfg
    public const int ArrayLimith = 1;

    public static (string Display, string? DataType) ToDisplay(OpcTag tag)
    {
        if (tag.StatusCode.ToLower() == "badnodeidunknown"
            || tag.Value.ValueKind == JsonValueKind.Null)
        {
            return (Constants.NoSuchTag, tag.DataType);
        }

        var dt = tag.DataType ?? DisplayDataType(tag);

        if (dt?.ToLower() == "int32"
            && tag.Attributes != null &&
            tag.Attributes.EnumStrings is { Length: > 0 }
            && tag.Value.TryGetInt32(out var iv))
        {
            var str = (iv >= 0 && iv < tag.Attributes.EnumStrings.Length) ?
                tag.Attributes.EnumStrings[iv] :
                iv.ToString();

            return (str, dt);
        }

        if (tag.ArrayType?.ToLower() == "array")
        {
            var arr = tag.Value;

            if (arr.ValueKind == JsonValueKind.Array)
            {
                var len = arr.GetArrayLength();

                string data = len > 0 ?
                    string.Join(", ", arr.EnumerateArray()
                        .Take(ArrayLimith)
                        .Select(e => e.ToString())) :
                    "";
                var preview = len > ArrayLimith ? $"{data}, ..." : data;

                return ($"[{preview}] (len={len})", dt);
            }
        }

        return (tag.Value.ToString(), dt);
    }

    public static TimeSpan? AgeUtc(OpcTag tag, DateTime nowUtc)
    {
        if (tag.ServerTimestamp is null)
        {
            return null;
        }

        return nowUtc - tag.ServerTimestamp.Value;
    }

    private static string? DisplayDataType(OpcTag tag)
    {
        if (tag.DisplayName.ToLower() == "nosuchtag")
        {
            return Constants.NoSuchTag;
        }

        switch (tag.Value.ValueKind)
        {
            case JsonValueKind.String:
                return "String";
            case JsonValueKind.Number:
                return "Double";
            case JsonValueKind.True:
            case JsonValueKind.False:
                return "Boolean";
            default:
                return tag.DataType;
        }
    }
}
