namespace TerraTex_RolePlay_AltV_Client.utils;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class TerraTexClientInit : Attribute
{
    public static void RunInitFunctions()
    {
        AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad!;

        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly =>
        {
            try
            {
                return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(TerraTexClientInit), false).Any());
            }
            catch
            {
                //Do Nothing Do not Stop program from running if assembly cannot be loaded
            }
            return new List<Type>();
        });

        RunInitFunctions(types);
    }

    protected static void RunInitFunctions(IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            Type fullType = type;
            Activator.CreateInstance(fullType);
        }
    }

    static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
    {
        RunInitFunctions(args.LoadedAssembly.GetTypes());
    }
}