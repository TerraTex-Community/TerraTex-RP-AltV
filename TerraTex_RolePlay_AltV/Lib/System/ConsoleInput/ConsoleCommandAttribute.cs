namespace TerraTex_RolePlay_AltV_Server.Lib.System.ConsoleInput;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ConsoleCommandAttribute : Attribute
{
    public string Cmd { get; }
    public ConsoleCommandAttribute(string cmd)
    {
        Cmd = cmd;
    }
}