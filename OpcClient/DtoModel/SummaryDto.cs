namespace OpcClient.DtoModel;

public sealed class SummaryDto
{
    public Rank Counts { get; set; } = new();
    public IEnumerable<string> DataTypes { get; set; } = [];
    public RangeTime SourceTimestamp { get; set; } = new();
    public RangeTime ServerTimestamp { get; set; } = new();

    public sealed class Rank {
        public int Good { get; set; }
        public int Uncertain { get; set; }
        public int Bad { get; set; }
    }

    public sealed class RangeTime {
        public DateTime? Min { get; set; }
        public DateTime? Max { get; set; }
    }
}
