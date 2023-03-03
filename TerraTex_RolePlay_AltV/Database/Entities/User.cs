using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TerraTex_RolePlay_AltV_Server.Database.Entities;


#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

[Index(nameof(Nickname), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(UUID), IsUnique = true)]
public class User : BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Column(Order = 0)]
    public int Id { get; set; }
    
    public string UUID { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string Nickname { get; set; }

    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Forename { get; set; }
    [Required]
    public string Lastname { get; set; }
    [Required]
    public string Salt { get; set; }
    [Required]
    public DateOnly BirthDay { get; set; }
    [Required]
    public string Gender { get; set; }

    public int AdminLevel { get; set; } = 0;
    public int DevLevel { get; set; } = 0;

    public ulong? LastHardwareIdExHash { get; set; }
    public ulong? LastHardwareIdHash { get; set; }
    public ulong? LastSocialClubId { get; set; }
    public string? LastIp { get; set; }

}
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
