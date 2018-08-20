using System;
using System.Configuration;
using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace TradeTopia.Hub.Windows.Service
{
  class Program
  {
    static void Main(string[] args)
    {
      System.ServiceProcess.ServiceBase[] servicesToRun = new System.ServiceProcess.ServiceBase[] { new WindowsServices.WindowsService() };

      if (Environment.UserInteractive)
      {
        Console.WriteLine("Welcome to the windows service(s) - Console Mode");
        Console.WriteLine();

        var onStartMethod = typeof(System.ServiceProcess.ServiceBase).GetMethod("OnStart", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        foreach (var service in servicesToRun)
        {
          Console.Write("Starting {0}...", service.ServiceName);
          var args1 = new Object[1];
          args1[0] = new String[1];
          onStartMethod.Invoke(service, args1);
          Console.WriteLine("Started");
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Press any key to stop the services and end the process...");
        Console.ReadKey();
        Console.WriteLine();

        var onStopMethod = typeof(System.ServiceProcess.ServiceBase).GetMethod("OnStop", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        foreach (var service in servicesToRun)
        {
          Console.Write("Stopping {0}...", service.ServiceName);
          onStopMethod.Invoke(service, null);
          Console.WriteLine("Stopped");
        }

        Console.WriteLine("All services stopped.");
        Console.WriteLine();
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
      }
      else
      {
        System.ServiceProcess.ServiceBase.Run(servicesToRun);
      }
    }
  }
}
