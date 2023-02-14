using System.Text.Json.Serialization;
using TerraTex_RolePlay_AltV_Server.Utils.JsonConverter;

namespace TerraTex_RolePlay_AltV_Server.Lib.User.Authentication;

public class RegistrationData
{
    public string Password { get; set; }
    public string Email { get; set; }
    public string Forename { get; set; }
    public string Lastname { get; set; }
    [JsonConverter(typeof(DateOnlyConverter))]
    public DateOnly Birthday { get; set; }
    public string Gender { get; set; }
}