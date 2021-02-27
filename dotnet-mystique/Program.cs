using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace dotnet_mystique
{
    class Program
    {
        private const string CONFIG_DIRECTORY_NAME = ".dotnet-mystique";
        private static readonly string globalDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), CONFIG_DIRECTORY_NAME);
        private static readonly string localDirectoryPath = Path.Combine(Environment.CurrentDirectory, CONFIG_DIRECTORY_NAME);
        private static readonly string commandsGlobalDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), CONFIG_DIRECTORY_NAME, "commands");
        private static readonly string commandsLocalDirectoryPath = Path.Combine(Environment.CurrentDirectory, CONFIG_DIRECTORY_NAME, "commands");

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                AnsiConsole.MarkupLine($"[silver]Usage:[/] [grey]dotnet[/] [#6699CC]mystique[/] [grey][[options]][/]");
                Console.WriteLine();
                AnsiConsole.MarkupLine($"[silver]options:[/]");
                AnsiConsole.MarkupLine($"  [#6699CC]--version[/]\t\t[silver]Display the tool version[/]");
                AnsiConsole.MarkupLine($"  [#6699CC]--help[/]\t\t[silver]Display the command line help[/]");
            }
            else
            {
                if (args[0].StartsWith("--"))
                {
                    switch (args[0].ToLower())
                    {
                        case "--version":
                            AnsiConsole.MarkupLine($"[silver]{Assembly.GetExecutingAssembly().GetName().Version}[/]");

                            break;
                        case "--help":
                            AnsiConsole.MarkupLine($"[silver]dotnet mystique tool ({Assembly.GetExecutingAssembly().GetName().Version})[/]");
                            Console.WriteLine();
                            AnsiConsole.MarkupLine($"[silver]Usage:[/] [grey]dotnet[/] [#6699CC]mystique[/] [grey][[options]][/]");
                            AnsiConsole.MarkupLine($"[silver]Usage:[/] [grey]dotnet[/] [#6699CC]mystique[/] [grey][[command]] [[command-options]][/]");
                            Console.WriteLine();
                            AnsiConsole.MarkupLine($"[silver]options:[/]");
                            AnsiConsole.MarkupLine($"  [#6699CC]--version[/]\t\t[silver]Display the tool version[/]");
                            AnsiConsole.MarkupLine($"  [#6699CC]--help[/]\t\t[silver]Display the command line help[/]");
                            Console.WriteLine();
                            AnsiConsole.MarkupLine($"[silver]commands:[/]");
                            AnsiConsole.MarkupLine($"  [#6699CC]init[/] [silver](internal)[/]\t[silver]Initialize the configuration directory[/]");

                            if (Directory.Exists(commandsGlobalDirectoryPath))
                            {
                                DirectoryInfo di = new DirectoryInfo(commandsGlobalDirectoryPath);

                                FileInfo[] fileInfoList = di.GetFiles("*.dll");

                                foreach (FileInfo fileInfo in fileInfoList)
                                {
                                    AnsiConsole.MarkupLine($"  [#6699CC]{fileInfo.Name.Replace(".dll", "").ToLower()}[/] [silver](global)[/]");
                                }

                                DirectoryInfo[] directoryInfoList = di.GetDirectories();

                                foreach (DirectoryInfo directoryInfo in directoryInfoList)
                                {
                                    fileInfoList = directoryInfo.GetFiles($"{directoryInfo.Name}.dll");

                                    foreach (FileInfo fileInfo in fileInfoList)
                                    {
                                        AnsiConsole.MarkupLine($"  [#6699CC]{fileInfo.Name.Replace(".dll", "").ToLower()}[/] [silver](global)[/]");
                                    }
                                }
                            }

                            if (Directory.Exists(commandsLocalDirectoryPath))
                            {
                                DirectoryInfo di = new DirectoryInfo(commandsLocalDirectoryPath);

                                FileInfo[] fileInfoList = di.GetFiles("*.dll");

                                foreach (FileInfo fileInfo in fileInfoList)
                                {
                                    AnsiConsole.MarkupLine($"  [#6699CC]{fileInfo.Name.Replace(".dll", "").ToLower()}[/] [silver](local)[/]");
                                }

                                DirectoryInfo[] directoryInfoList = di.GetDirectories();

                                foreach (DirectoryInfo directoryInfo in directoryInfoList)
                                {
                                    fileInfoList = directoryInfo.GetFiles($"{directoryInfo.Name}.dll");

                                    foreach (FileInfo fileInfo in fileInfoList)
                                    {
                                        AnsiConsole.MarkupLine($"  [#6699CC]{fileInfo.Name.Replace(".dll", "").ToLower()}[/] [silver](local)[/]");
                                    }
                                }
                            }

                            Console.WriteLine();
                            AnsiConsole.MarkupLine($"[silver]Run[/] [grey]'dotnet[/] [#6699CC]mystique[/] [grey][[command]] --help'[/][silver] for more information on a command.[/]");

                            break;
                        default:
                            // Error message option unknow
                            break;
                    }
                }
                else if (args[0].ToLower() == "init")
                {
                    bool initGlobal = false;

                    if (args.Length > 1 && args[1].StartsWith("--"))
                    {
                        switch (args[1].ToLower())
                        {
                            case "--help":
                                AnsiConsole.MarkupLine($"[silver]Initialize the configuration directory[/]");
                                Console.WriteLine();
                                AnsiConsole.MarkupLine($"[silver]Usage:[/] [grey]dotnet[/] [#6699CC]mystique[/] [silver]init[/] [grey][[options]][/]");
                                Console.WriteLine();
                                AnsiConsole.MarkupLine($"[silver]options:[/]");
                                AnsiConsole.MarkupLine($"  [#6699CC]--help[/]\t\t[silver]Display the command line help[/]");
                                AnsiConsole.MarkupLine($"  [#6699CC]--global[/]\t\t[silver]Create the configuration directory in the user profile directory instead of the current directory[/]");

                                return;
                            case "--global":
                                initGlobal = true;
                                break;
                            default:
                                // Error message option unknow
                                break;
                        }
                    }

                    // Execute init command
                    if (initGlobal)
                    {
                        if (!Directory.Exists(globalDirectoryPath))
                        {
                            Directory.CreateDirectory(globalDirectoryPath);
                        }

                        if (!Directory.Exists(commandsGlobalDirectoryPath))
                        {
                            Directory.CreateDirectory(commandsGlobalDirectoryPath);
                        }
                    }
                    else
                    {
                        if (!Directory.Exists(localDirectoryPath))
                        {
                            Directory.CreateDirectory(localDirectoryPath);
                        }

                        if (!Directory.Exists(commandsLocalDirectoryPath))
                        {
                            Directory.CreateDirectory(commandsLocalDirectoryPath);
                        }
                    }

                    Console.WriteLine($"{(initGlobal ? "Global" : "Local")} directory created in: {(initGlobal ? globalDirectoryPath : localDirectoryPath)}");
                }
                else
                {
                    Dictionary<string, string> dllDictionary = new Dictionary<string, string>();

                    if (Directory.Exists(commandsGlobalDirectoryPath))
                    {
                        DirectoryInfo di = new DirectoryInfo(commandsGlobalDirectoryPath);

                        FileInfo[] fileInfoList = di.GetFiles("*.dll");

                        foreach (FileInfo fileInfo in fileInfoList)
                        {
                            string key = fileInfo.Name.Replace(".dll", "").ToLower();

                            if (!dllDictionary.ContainsKey(key))
                            {
                                dllDictionary.Add(key, fileInfo.FullName);
                            }
                        }

                        DirectoryInfo[] directoryInfoList = di.GetDirectories();

                        foreach (DirectoryInfo directoryInfo in directoryInfoList)
                        {
                            fileInfoList = directoryInfo.GetFiles($"{directoryInfo.Name}.dll");

                            foreach (FileInfo fileInfo in fileInfoList)
                            {
                                string key = fileInfo.Name.Replace(".dll", "").ToLower();

                                if (!dllDictionary.ContainsKey(key))
                                {
                                    dllDictionary.Add(key, fileInfo.FullName);
                                }
                            }
                        }
                    }

                    if (Directory.Exists(commandsLocalDirectoryPath))
                    {
                        DirectoryInfo di = new DirectoryInfo(commandsLocalDirectoryPath);

                        FileInfo[] fileInfoList = di.GetFiles("*.dll");

                        foreach (FileInfo fileInfo in fileInfoList)
                        {
                            string key = fileInfo.Name.Replace(".dll", "").ToLower();

                            if (!dllDictionary.ContainsKey(key))
                            {
                                dllDictionary.Add(key, fileInfo.FullName);
                            }
                        }

                        DirectoryInfo[] directoryInfoList = di.GetDirectories();

                        foreach (DirectoryInfo directoryInfo in directoryInfoList)
                        {
                            fileInfoList = directoryInfo.GetFiles($"{directoryInfo.Name}.dll");

                            foreach (FileInfo fileInfo in fileInfoList)
                            {
                                string key = fileInfo.Name.Replace(".dll", "").ToLower();

                                if (!dllDictionary.ContainsKey(key))
                                {
                                    dllDictionary.Add(key, fileInfo.FullName);
                                }
                            }
                        }
                    }

                    if (!dllDictionary.ContainsKey(args[0].ToLower()))
                    {
                        AnsiConsole.MarkupLine($"[red]Command not found[/]");

                        return;
                    }

                    AssemblyLoadContext loader = new AssemblyLoadContext(null);
                    string assemblyFullPath = dllDictionary[args[0].ToLower()];

                    CommandLoadContext loadContext = new CommandLoadContext(assemblyFullPath);
                    //AssemblyDependencyResolver resolver = new AssemblyDependencyResolver(assemblyFullPath);
                    //string assemblyPath = resolver.ResolveAssemblyToPath(new AssemblyName(Path.GetFileNameWithoutExtension(assemblyFullPath)));
                    //Assembly assembly = loader.LoadFromAssemblyPath(assemblyPath);
                    Assembly assembly =  loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(assemblyFullPath)));

                    if (args.Length > 1 && args[1].StartsWith("--"))
                    {
                        switch (args[1].ToLower())
                        {
                            case "--version":
                                AnsiConsole.MarkupLine($"[silver]{assembly.GetName().Version}[/]");

                                break;
                            case "--help":
                                AnsiConsole.MarkupLine($"[silver]Usage:[/] [grey]dotnet[/] [#6699CC]mystique[/] [silver]{args[0].ToLower()}[/] [grey][[options]][/]");
                                AnsiConsole.MarkupLine($"[silver]Usage:[/] [grey]dotnet[/] [#6699CC]mystique[/] [silver]{args[0].ToLower()}[/] [grey][[command]] [[command-options]][/]");
                                Console.WriteLine();
                                AnsiConsole.MarkupLine($"[silver]options:[/]");
                                AnsiConsole.MarkupLine($"  [#6699CC]--version[/]\t\t[silver]Display the tool version[/]");
                                AnsiConsole.MarkupLine($"  [#6699CC]--help[/]\t\t[silver]Display the command line help[/]");
                                Console.WriteLine();
                                AnsiConsole.MarkupLine($"[silver]commands:[/]");

                                Dictionary<string, string> classDictionary = new Dictionary<string, string>();

                                foreach (Type type in assembly.GetTypes())
                                {
                                    if (type.IsClass)
                                    {
                                        string key = type.Name.ToLower();

                                        if (!classDictionary.ContainsKey(key))
                                        {
                                            classDictionary.Add(key, type.FullName);
                                        }
                                        else
                                        {
                                            string updatedKey = classDictionary[key].Replace(".", "-").ToLower();
                                            string value = classDictionary[key];

                                            classDictionary.Remove(key);

                                            classDictionary.Add(updatedKey, value);

                                            key = type.FullName.Replace(".", "-").ToLower();

                                            if (!classDictionary.ContainsKey(key))
                                            {
                                                classDictionary.Add(key, type.FullName);
                                            }
                                        }
                                    }
                                }

                                foreach (KeyValuePair<string, string> item in classDictionary)
                                {
                                    AnsiConsole.MarkupLine($"  [#6699CC]{item.Key}[/]");
                                }

                                Console.WriteLine();
                                AnsiConsole.MarkupLine($"[silver]Run[/] [grey]'dotnet[/] [#6699CC]mystique[/] [silver]{args[0].ToLower()}[/] [grey][[command]] --help'[/][silver] for more information on a command.[/]");

                                break;
                            default:
                                // Error message option unknow
                                break;
                        }
                    }
                    else if (args.Length > 2 && args[2].StartsWith("--"))
                    {
                        Dictionary<string, string> classDictionary = new Dictionary<string, string>();

                        foreach (Type type in assembly.GetTypes())
                        {
                            if (type.IsClass)
                            {
                                string key = type.Name.ToLower();

                                if (!classDictionary.ContainsKey(key))
                                {
                                    classDictionary.Add(key, type.FullName);
                                }
                                else
                                {
                                    string updatedKey = classDictionary[key].Replace(".", "-").ToLower();
                                    string value = classDictionary[key];

                                    classDictionary.Remove(key);

                                    classDictionary.Add(updatedKey, value);

                                    key = type.FullName.Replace(".", "-").ToLower();

                                    if (!classDictionary.ContainsKey(key))
                                    {
                                        classDictionary.Add(key, type.FullName);
                                    }
                                }
                            }
                        }

                        if (!classDictionary.ContainsKey(args[1].ToLower()))
                        {
                            AnsiConsole.MarkupLine($"[red]Command not found[/]");

                            return;
                        }

                        switch (args[2].ToLower())
                        {
                            case "--help":
                                AnsiConsole.MarkupLine($"[silver]Usage:[/] [grey]dotnet[/] [#6699CC]mystique[/] [grey]{args[0].ToLower()}[/] [silver]{args[1].ToLower()}[/] [grey][[options]][/]");
                                AnsiConsole.MarkupLine($"[silver]Usage:[/] [grey]dotnet[/] [#6699CC]mystique[/] [grey]{args[0].ToLower()}[/] [silver]{args[1].ToLower()}[/] [grey][[command]] [[command-options]][/]");
                                Console.WriteLine();
                                AnsiConsole.MarkupLine($"[silver]options:[/]");
                                AnsiConsole.MarkupLine($"  [#6699CC]--help[/]\t\t[silver]Display the command line help[/]");
                                Console.WriteLine();
                                AnsiConsole.MarkupLine($"[silver]commands:[/]");

                                Type type = assembly.GetType(classDictionary[args[1].ToLower()]);

                                foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                                {
                                    AnsiConsole.MarkupLine($"  [#6699CC]{method.Name.ToLower()}[/]");
                                }

                                Console.WriteLine();
                                AnsiConsole.MarkupLine($"[silver]Run[/] [grey]'dotnet[/] [#6699CC]mystique[/] [grey]{args[0].ToLower()}[/] [silver]{args[1].ToLower()}[/] [grey][[command]] --help'[/][silver] for more information on a command.[/]");

                                break;
                            default:
                                // Error message option unknow
                                break;
                        }
                    }
                    //else if (args.Length > 3 && args[3].StartsWith("--"))
                    else if (args.Length > 2)
                    {
                        Dictionary<string, string> classDictionary = new Dictionary<string, string>();

                        foreach (Type classType in assembly.GetTypes())
                        {
                            if (classType.IsClass)
                            {
                                string key = classType.Name.ToLower();

                                if (!classDictionary.ContainsKey(key))
                                {
                                    classDictionary.Add(key, classType.FullName);
                                }
                                else
                                {
                                    string updatedKey = classDictionary[key].Replace(".", "-").ToLower();
                                    string value = classDictionary[key];

                                    classDictionary.Remove(key);

                                    classDictionary.Add(updatedKey, value);

                                    key = classType.FullName.Replace(".", "-").ToLower();

                                    if (!classDictionary.ContainsKey(key))
                                    {
                                        classDictionary.Add(key, classType.FullName);
                                    }
                                }
                            }
                        }

                        if (!classDictionary.ContainsKey(args[1].ToLower()))
                        {
                            AnsiConsole.MarkupLine($"[red]Command not found[/]");

                            return;
                        }

                        Dictionary<string, MethodInfo> methodDictionary = new Dictionary<string, MethodInfo>();

                        Type type = assembly.GetType(classDictionary[args[1].ToLower()]);

                        foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        {
                            if (!methodDictionary.ContainsKey(methodInfo.Name.ToLower()))
                            {
                                methodDictionary.Add(methodInfo.Name.ToLower(), methodInfo);
                            }
                        }

                        if (!methodDictionary.ContainsKey(args[2].ToLower()))
                        {
                            AnsiConsole.MarkupLine($"[red]Command not found[/]");

                            return;
                        }

                        MethodInfo method = methodDictionary[args[2].ToLower()];

                        if (args.Length > 3)
                        {
                            switch (args[3].ToLower())
                            {
                                case "--help":
                                    AnsiConsole.MarkupLine($"[silver]Usage:[/] [grey]dotnet[/] [#6699CC]mystique[/] [grey]{args[0].ToLower()}[/] [grey]{args[1].ToLower()}[/] [silver]{args[2].ToLower()}[/] [grey][[options]][/]");
                                    Console.WriteLine();
                                    AnsiConsole.MarkupLine($"[silver]options:[/]");

                                    foreach (ParameterInfo parameter in method.GetParameters())
                                    {
                                        AnsiConsole.MarkupLine($"  [#6699CC]--{parameter.Name.ToLower()}[/] [silver][[{parameter.ParameterType.Name}]] ({(!parameter.IsOptional ? "Required" : $"Default: {parameter.DefaultValue}")})[/]");
                                    }

                                    return;
                                default:
                                    // Error message option unknow
                                    break;
                            }
                        }

                        Dictionary<string, string> parameterDictionary = new Dictionary<string, string>();

                        for (int i = 3; i < args.Length; i += 2)
                        {
                            if (!parameterDictionary.ContainsKey(args[i]))
                            {
                                parameterDictionary.Add(args[i].ToLower(), args[i + 1]);
                            }
                        }

                        object classInstance = Activator.CreateInstance(type, null);
                        List<object> parameters = new List<object>();

                        foreach (ParameterInfo parameter in method.GetParameters())
                        {
                            string key = $"--{parameter.Name.ToLower()}";

                            if (parameterDictionary.ContainsKey(key))
                            {
                                object value = parameterDictionary[key];

                                if (parameter.ParameterType.Name == "Int32")
                                {
                                    parameters.Add(Convert.ToInt32(value));
                                }
                                else
                                {
                                    parameters.Add(value);
                                }

                            }
                            else
                            {
                                parameters.Add(Type.Missing);
                            }
                        }

                        method.Invoke(classInstance, parameters.ToArray());
                    }
                }
            }
        }
    }
}
