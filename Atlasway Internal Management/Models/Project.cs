using System.Text.Json.Serialization;

namespace Atlasway_Internal_Management.Models;


public readonly struct Project
{
    [JsonPropertyName("projectNo")]     public uint     ProjectNo   { get; init; }
    [JsonPropertyName("projectName")]   public string   ProjectName { get; init; }
    [JsonPropertyName("clientno")]      public uint     ClientNo    { get; init; }
    [JsonPropertyName("statusno")]      public uint     StatusNo    { get; init; }

    internal Project(Project project)
    {
        ProjectNo   = project.ProjectNo;
        ProjectName = project.ProjectName;
        ClientNo    = project.ClientNo;
        StatusNo    = project.StatusNo;
    }
}

public readonly struct NewProject
{
    [JsonPropertyName("projectName")]   public string   ProjectName { get; init; }
    [JsonPropertyName("clientno")]      public uint     ClientNo    { get; init; }
    [JsonPropertyName("statusno")]      public uint     StatusNo    { get; init; }

    internal NewProject(string projectName, uint clientNo, uint statusNo)
    {
        ProjectName = projectName;
        ClientNo    = clientNo;
        StatusNo    = statusNo;
    }
}
