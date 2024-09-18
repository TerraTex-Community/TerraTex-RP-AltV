using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TerraTex_RolePlay_AltV_Server.Lib.Helper;

namespace TerraTex_RolePlay_AltV_Server.Database.Entities;


#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

[Index(nameof(UUID), IsUnique = true)]
public class User : BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Column(Order = 0)]
    public int Id { get; set; }
    
    public string UUID { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string DiscordId { get; set; }
    
    [Required]
    public bool DiscordMFAEnabled { get; set; }
    
    [Required]
    public string DiscordUsername { get; set; }

    //@todo: does this have to be moved to Character?
    public AdminLevel AdminLevel { get; set; } = AdminLevel.None;
    public DeveloperLevel DevLevel { get; set; } = DeveloperLevel.None;

    public ulong? LastHardwareIdExHash { get; set; }
    public ulong? LastHardwareIdHash { get; set; }
    public ulong? LastSocialClubId { get; set; }
    public string? LastIp { get; set; }

}
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
