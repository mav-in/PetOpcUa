namespace OpcApi.Extension;

static class Extensions
{
    public static (DateTime? min, DateTime? max) MinMaxSinglePass(this IEnumerable<DateTime> seq)
    {
        if (seq == null) return (null, null);
        using var e = seq.GetEnumerator();
        if (!e.MoveNext()) return (null, null);
        DateTime min = e.Current, max = e.Current;
        while (e.MoveNext())
        {
            var v = e.Current;
            if (v < min) min = v;
            if (v > max) max = v;
        }
        return (min, max);
    }
}
