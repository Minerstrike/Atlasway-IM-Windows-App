using System.Text.Json.Serialization;

namespace Atlasway_Internal_Management.Models;


public readonly struct Staff
{
    [JsonPropertyName("staffNo")]       public uint     StaffNo         { get; init; }
    [JsonPropertyName("surname")]       public string   Surname         { get; init; }
    [JsonPropertyName("firstname")]     public string   Firstname       { get; init; }
    [JsonPropertyName("contactno")]     public string   ContactNo       { get; init; }
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

public readonly struct NewStaff
{
    [JsonPropertyName("surname")]       public string   Surname         { get; init; }
    [JsonPropertyName("firstname")]     public string   Firstname       { get; init; }
    [JsonPropertyName("contactno")]     public string   ContactNo       { get; init; }
    [JsonPropertyName("emailaddress")]  public string   EmailAddress    { get; init; }

    internal NewStaff(string surname, string firstname, string contactNo, string emailAddress)
    {
        Surname         = surname;
        Firstname       = firstname;
        ContactNo       = contactNo;
        EmailAddress    = emailAddress;
    }
}