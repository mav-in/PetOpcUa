using CommunityToolkit.Mvvm.ComponentModel;
using OpcDomain;
using System.Text.Json;

namespace OpcClient.ViewModels;

public partial class TagItemViewModel : ObservableObject
{
    private string _DisplayValue;
    private string _DataType;
    private JsonElement _Value;

    public OpcTag Model { get; }

    public TagItemViewModel(OpcTag model)
    {
        var (display, dataType) = ValueFormat.ToDisplay(model);
        _DisplayValue = display ?? string.Empty;
        _DataType = dataType ?? string.Empty;
        
        _Value = model.Value;

        Model = model;
    }

    public string DisplayName => Model.DisplayName;
    public string NodeId => Model.NodeId ?? "—";
    public string Value => _DisplayValue;
    public string DataType => _DataType ?? (Model.DataType ?? "Unknown");
    public string StatusCode => Model.StatusCode ?? string.Empty;
    public DateTime? SourceTimestamp => Model.SourceTimestamp;
    public DateTime? ServerTimestamp => Model.ServerTimestamp;

    public string Age
    {
        get
        {
            var srv = ServerTimestamp;

            if (!srv.HasValue)
                return "—";

            var age = ValueFormat.AgeUtc(Model, DateTime.UtcNow);

            return age!.Value.TotalSeconds < 1 ? "0s" : $"{(int)age!.Value.TotalSeconds}s";
        }
    }

    public string Note
    {
        get
        {
            return (Model.Attributes?.EnumStrings is { Length: > 0 }) ?
                "Enum" :
                (
                    string.Equals(Model.ArrayType?.ToLower(), "array") ?
                    $"Array len={_Value.GetArrayLength()}" :
                    (string.IsNullOrWhiteSpace(Model.DataType) && Model.NodeId == null) ?
                        "NoSuchTag" :
                        ""
                );
        }
    }
}
