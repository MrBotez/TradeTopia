using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TradeTopia.Common.Log;

namespace TradeTopia.Hub.Windows.Service.WindowsServices
{
  partial class WindowsService : ServiceBase
  {
    public WindowsService()
    {
      InitializeComponent();

      EventLogger.Instance.EventLogSource = this.ServiceName;
    }

    private IDisposable webService = null;

    protected override void OnStart(string[] args)
    {
      EventLogger.Instance.LogToEventLog("Starting the service.", LogType.ltInfo);

      try
      {
        Data.DataContext.Instance.Initialize();

        if (webService != null)
        {
          webService.Dispose();
        }
        else
        {
          string baseAddress = ConfigurationManager.AppSettings["baseAddress"];

          // Start OWIN host 
          webService = WebApp.Start<WebAPI.Startup>(url: baseAddress);
        }

        EventLogger.Instance.LogToEventLog("The service has started.", LogType.ltInfo);
      }
      catch (Exception ex)
      {
        EventLogger.Instance.LogToEventLog(String.Format("The service failed to start. {0}", ex.ToString()), LogType.ltError);
      }
    }

    protected override void OnStop()
    {
      EventLogger.Instance.LogToEventLog("Stopping the service.", LogType.ltInfo);

      try
      {
        webService?.Dispose();

        EventLogger.Instance.LogToEventLog("The service has stopped.", LogType.ltInfo);
      }
      catch (Exception ex)
      {
        EventLogger.Instance.LogToEventLog(String.Format("The service failed to stop. {0}", ex.ToString()), LogType.ltError);
      }
    }
  }
}