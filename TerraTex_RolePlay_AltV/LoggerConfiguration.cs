using NLog;
using NLog.Extensions.Logging;
using NLog.Layouts;
using NLog.Targets;

namespace TerraTex_RolePlay_AltV_Server;

static class LoggerConfiguration
{
    public static void ConfigureLogger()
    {
        var config = new NLog.Config.LoggingConfiguration();
        
        // Rules for mapping loggers to targets            
        config.AddRule(LogLevel.Info, LogLevel.Fatal, GetConsoleTarget());
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, GetFileLogger());
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, GetJsonLogger());

        // Apply config           
        NLog.LogManager.Configuration = config;
    }

    static ColoredConsoleTarget GetConsoleTarget()
    {
        return new ColoredConsoleTarget
        {
            Layout = "${longdate}|${sequenceid}|${level:uppercase=true}|${logger}|${message}",
            UseDefaultRowHighlightingRules = false,
            RowHighlightingRules =
                {
                    new ConsoleRowHighlightingRule
                    {
                        Condition = "level == LogLevel.Trace",
                        ForegroundColor = ConsoleOutputColor.DarkCyan
                    },
                    new ConsoleRowHighlightingRule
                    {
                        Condition = "level == LogLevel.Debug",
                        ForegroundColor = ConsoleOutputColor.DarkGray
                    },
                    new ConsoleRowHighlightingRule
                    {
                        Condition = "level == LogLevel.Info",
                        ForegroundColor = ConsoleOutputColor.White
                    },
                    new ConsoleRowHighlightingRule
                    {
                        Condition = "level == LogLevel.Warn",
                        ForegroundColor = ConsoleOutputColor.Yellow
                    },
                    new ConsoleRowHighlightingRule
                    {
                        Condition = "level == LogLevel.Error",
                        ForegroundColor = ConsoleOutputColor.DarkRed
                    },
                    new ConsoleRowHighlightingRule
                    {
                        Condition = "level == LogLevel.Fatal",
                        ForegroundColor = ConsoleOutputColor.Red
                    }
                },
            WordHighlightingRules =
                {
                    new ConsoleWordHighlightingRule
                    {
                        Text = "TerraTex_RolePlay_AltV_Server",
                        IgnoreCase = true,
                        ForegroundColor = ConsoleOutputColor.Green
                    },
                    new ConsoleWordHighlightingRule
                    {
                        Text = "TerraTexRolePlayResource",
                        IgnoreCase = true,
                        ForegroundColor = ConsoleOutputColor.Green
                    },
                    new ConsoleWordHighlightingRule
                    {
                        Text = "TerraTex",
                        IgnoreCase = true,
                        ForegroundColor = ConsoleOutputColor.Green
                    }
                }
        };
    }

    static FileTarget GetJsonLogger()
    {
        return new FileTarget("logfile")
        {
            FileName = "logs/log.json",
            ArchiveNumbering = ArchiveNumberingMode.DateAndSequence,
            ArchiveOldFileOnStartup = true,
            CreateDirs = true,
            Layout = new JsonLayout
            {
                Attributes =
                    {
                        new JsonAttribute
                        {
                            Name = "time",
                            Layout = "${longdate}"
                        },
                        new JsonAttribute
                        {
                            Name = "sequenceid",
                            Layout = "${sequenceid}"
                        },
                        new JsonAttribute
                        {
                            Name = "level",
                            Layout = "${level:upperCase=true}"
                        },
                        new JsonAttribute
                        {
                            Name = "message",
                            Layout = "${message}"
                        },
                        new JsonAttribute
                        {
                            Name = "callsite",
                            Layout = "${callsite}"
                        },
                        new JsonAttribute
                        {
                            Name = "logger",
                            Layout = "${logger}"
                        },
                        new JsonAttribute
                        {
                            Name = "stacktrace",
                            Layout = "${stacktrace}"
                        },
                        new JsonAttribute
                        {
                            Name = "exception",
                            Layout = "${exception:format=@}",
                            Encode = false,
                        },
                        new JsonAttribute
                        {
                            Name = "gdc",
                            Layout = "${gdc:format=@}",
                            Encode = false,
                        },
                        new JsonAttribute
                        {
                            Name = "props",
                            Layout = "${all-event-properties}"
                        },
                        new JsonAttribute
                        {
                            Name = "eventData",
                            Layout = new JsonLayout
                            {
                                IncludeEventProperties = true,
                                IncludeGdc = true,
                                IncludeScopeProperties = true,
                                MaxRecursionLimit = 2
                            },
                            Encode = false,
                        }
                    },
                ExcludeEmptyProperties = false,
                RenderEmptyObject = true
            }
        };
    }

    static FileTarget GetFileLogger()
    {
        return new FileTarget("logfile")
        {
            FileName = "logs/log.txt",
            ArchiveNumbering = ArchiveNumberingMode.DateAndSequence,
            ArchiveOldFileOnStartup = true,
            CreateDirs = true,
            Layout = "${longdate}|${sequenceid}|${level:uppercase=true}|${logger}|${message:withexception=true}"
        };
    }
}