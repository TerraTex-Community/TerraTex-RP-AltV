using System.ComponentModel;
using System.Reflection;
using System.Text;
using System;
using System.Diagnostics;
using System.IO;
using AltV.Net;

namespace TerraTex_RolePlay_AltV_Server.Lib.System.ConsoleInput;

public class ConsoleReader : IScript
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    ConsoleReader()
    {
        Alt.OnConsoleCommand += AltOnOnConsoleCommand;
    }

    private void AltOnOnConsoleCommand(string name, string[] args)
    {
        if (name.StartsWith("/"))
        {
            HandleConsoleCommand(name.Substring(1), args);
        }
        else
        {
            String txt = $"{name} {String.Join(" ", args)}";
            Logger.Info("Sending Message: [SERVER - INFORMATION]: " + txt);

            // @todo: add sending as broadcast to all users
            Chat.Chat.BroadCast("[SERVER - INFORMATION]: " + txt);
            // TTRPG.Api.triggerClientEventForAll("addHtmlMessage", "<span style='font-weight: bold'>[SERVER-INFORMATION]: " + s + "</span>");

        }
    }

    private void HandleConsoleCommand(string cmd, string[] parts)
    {
        Assembly ass = Assembly.Load(Assembly.GetExecutingAssembly().GetName());
    
        MethodInfo[] methods = ass.GetTypes()
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetCustomAttributes(typeof(ConsoleCommandAttribute), false).Length > 0)
            .ToArray();
    
        foreach (MethodInfo info in methods)
        {
            if (!CheckMethodForAttribute(cmd, info)) continue;
    
            ParameterInfo[] pInfos = info.GetParameters();
    
            try
            {
                object?[] parameter = new object?[pInfos.Length];
    
                for (int i = 0; i < pInfos.Length; i++)
                {
                    ParameterInfo pInfo = pInfos[i];
                    TypeConverter converter = TypeDescriptor.GetConverter(pInfo.ParameterType);
    
                    if (parts.Length <= i)
                    {
                        if (pInfo is { HasDefaultValue: false, IsOptional: false })
                        {
                            throw new ArgumentException("Not all parameters given!");
                        }
                        continue;
                    }
    
                    parameter[i] = converter.ConvertFrom(parts[i]);
                }
    
                if (pInfos.Length < parts.Length && pInfos[^1].ParameterType == typeof(string))
                {
                    List<string> strings = new List<string> { (string)parameter[^1]! };
    
                    for (int i = parameter.Length; i < parts.Length; i++)
                    {
                        strings.Add(parts[i]);
                    }
    
                    parameter[^1] = string.Join(" ", strings.ToArray());
                }
                
                object obj = Activator.CreateInstance(info.ReflectedType!)!;
                info.Invoke(obj, parameter);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                StringBuilder sb = new StringBuilder();
    
                foreach (ParameterInfo pInfo in pInfos)
                {
                    sb.Append($" [{pInfo.ParameterType.Name} {pInfo.Name}]");
                }
    
                Logger.Error("Usage: /" + cmd + sb);
            }
        }
    }
    
    private bool CheckMethodForAttribute(string cmd, MethodInfo info)
    {
        IEnumerable<Attribute> attributes = info.GetCustomAttributes(typeof(ConsoleCommandAttribute));
    
        foreach (Attribute attribute in attributes)
        {
            ConsoleCommandAttribute attr = (ConsoleCommandAttribute)attribute;
            if (attr.Cmd.Equals(cmd))
            {
                return true;
            }
        }
    
        return false;
     }
}