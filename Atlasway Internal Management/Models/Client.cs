using System.Text.Json.Serialization;

namespace Atlasway_Internal_Management.Models;


public readonly struct Client
{
    [JsonPropertyName("clientNo")]      public uint     ClientNo        { get; init; }
    [JsonPropertyName("clientName")]    public string   ClientName      { get; init; }
    [JsonPropertyName("contactNo")]     public string?  ContactNo       { get; init; }
    [JsonPropertyName("emailAddress")]  public string?  EmailAddress    { get; init; }

    public Client(uint clientNo, string clientName, string? contactNo, string? emailAddress)
    {
        ClientNo        = clientNo;
        ClientName      = clientName;
        ContactNo       = contactNo;
        EmailAddress    = emailAddress;
    }
}

public readonly struct NewClient
{
    [JsonPropertyName("clientName")]    public string   ClientName      { get; init; }
    [JsonPropertyName("contactNo")]     public string?  ContactNo       { get; init; }
    [JsonPropertyName("emailAddress")]  public string?  EmailAddress    { get; init; }

    public NewClient(string clientName, string? contactNo, string? emailAddress)
    {
        ClientName      = clientName;
        ContactNo       = contactNo;
        EmailAddress    = emailAddress;
    }
}
