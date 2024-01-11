using System.Text.Json.Serialization;

namespace Atlasway_Internal_Management.Models;


public readonly struct ProjectStatusType
{
    [JsonPropertyName("typeNo")]    public uint     TypeNo      { get; init; }
    [JsonPropertyName("typeName")]  public string   TypeName    { get; init; }

    internal ProjectStatusType(ProjectStatusType projectStatusType)
    {
        TypeNo      = projectStatusType.TypeNo;
        TypeName    = projectStatusType.TypeName;
    }
}
