using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace TradeTopia.Hub.Windows.Service.WebAPI.MiddleWare
{
  public class LoggingMiddleWare : OwinMiddleware
  {
    public LoggingMiddleWare(OwinMiddleware next) : base(next)
    {
      
    }

    public async override Task Invoke(IOwinContext context)
    {
      try
      {
        var request = $"{DateTime.Now:yyyyMMddHHmmss}: Request made to {context.Request.Uri.AbsoluteUri} from {context.Request.RemoteIpAddress} by user {(context.Request.User == null ? "NULL" : context.Request.User.Identity.Name)}";

        Console.WriteLine(request);
      }
      catch { }

      await Next.Invoke(context);
    }
  }
}
