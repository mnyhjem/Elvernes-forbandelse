using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Owin.Hosting;

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
                Console.ReadLine();
            }
        }
    }

    
}
