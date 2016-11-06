using System;
using System.Configuration;
using System.Diagnostics;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Server.App_Start;
using Microsoft.Owin.Hosting;
using Ninject;

namespace ElvenCurse.Server
{
    class Program
    {
        static void Main()
        {
            Trace.Listeners.RemoveAt(0);
            Trace.Listeners.Add(new ConsoleTraceListener());

            var url = ConfigurationManager.AppSettings["serverPath"];
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);

                var gameengine = NinjectWebCommon.bootstrapper.Kernel.TryGet(typeof(IGameEngine)) as IGameEngine;
                if (gameengine == null)
                {
                    Console.WriteLine("Gameengine not running. Closing down");
                    return;
                }

                Console.CursorVisible = true;

                var serverRunning = true;
                do
                {
                    string commandtext;
                    var cmd = "";
                    var parameters = "";
                    commandtext = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(commandtext))
                    {
                        cmd = commandtext;
                        var position = cmd.IndexOf(" ", StringComparison.Ordinal);
                        if (position > -1)
                        {
                            parameters = cmd.Substring(position).Trim();
                            cmd = cmd.Substring(0, position);
                        }
                    }

                    switch (cmd.ToLower())
                    {
                        case "exit":
                            serverRunning = false;
                            break;

                        case "onlinecount":
                        case "online":
                            Console.WriteLine("> {0}", gameengine.Onlinecount);
                            break;

                        case "refresh":
                        case "reload":
                            int worldsection = 1;
                            //var position = parameters.IndexOf(" ", StringComparison.Ordinal);
                            var spl = parameters.Split(' ');
                            if (spl.Length > 1)
                            {
                                if (!int.TryParse(spl[0].Trim(), out worldsection))
                                {
                                    Console.WriteLine("> Invalid worldsection");
                                    break;
                                }
                                parameters = spl[1];
                            }
                            else
                            {
                                Console.WriteLine("> Invalid worldsection");
                                break;
                            }

                            

                            switch (parameters.ToLower())
                            {
                                case "npc":
                                case "npcs":
                                    gameengine.SendToClientsNpcs(worldsection, loadFromDatabase: true);
                                    Console.WriteLine("Refreshed Npcs");
                                    break;

                                case "interactiveobjects":
                                case "objects":
                                    gameengine.SendToClientsInteractiveObjects(worldsection, loadFromDatabase: true);
                                    Console.WriteLine("Refreshed interactive objects");
                                    break;

                                default:
                                    Console.WriteLine("> Invalid parameter");
                                    break;
                            }
                            break;

                        default:
                            Console.WriteLine("> Invalid command");
                            break;
                    }

                } while (serverRunning);
            }
        }
    }
}
