using System.Text.Json.Serialization;

namespace Atlasway_Internal_Management.Models;


public readonly struct Staff
{
    [JsonPropertyName("staffNo")]       public uint     StaffNo         { get; init; }
    [JsonPropertyName("surname")]       public string   Surname         { get; init; }
    [JsonPropertyName("firstname")]     public string   Firstname       { get; init; }
    [JsonPropertyName("contactNo")]     public string   ContactNo       { get; init; }
    [JsonPropertyName("emailaddress")]  public string   EmailAddress    { get; init; }

    internal Staff(Staff staff)
    {
        StaffNo         = staff.StaffNo;
        Surname         = staff.Surname;
        Firstname       = staff.Firstname;
        ContactNo       = staff.ContactNo;
        EmailAddress    = staff.EmailAddress;
    }
}
