namespace OpcDomain;

public static class ValueFormat
{
    public const int ArrayLimith = 1;

    public static (string Display, string? DataType) ToDisplay(OpcTag tag)
    {
        throw new NotImplementedException();
    }

    public static TimeSpan? AgeUtc(OpcTag tag, DateTime nowUtc)
    {
        if (tag.ServerTimestamp is null)
        {
            return null;
        }

        return nowUtc - tag.ServerTimestamp.Value;
    }
}
