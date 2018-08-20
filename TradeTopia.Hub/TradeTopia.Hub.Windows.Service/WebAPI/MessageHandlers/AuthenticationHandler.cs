using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TradeTopia.Hub.Windows.Service.WebAPI.MessageHandlers
{
  public class AuthenticationHandler : DelegatingHandler
  {
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      try
      {
        var header = request.Headers.GetValues("Authorization").FirstOrDefault();
        if (header != null)
        {
          var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);
          if (authHeader != null && "Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
          {
            string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
            var parts = parameter.Split(':');

            string userName = parts[0];
            string password = parts[1];

            var usr = Data.usr_User.GetByName(userName);
            if (usr != null)
            {
              IPrincipal principal = new GenericPrincipal(new GenericIdentity(usr.usr_Name), "Administrator".Split(','));
              var ctx = request.GetRequestContext();
              ctx.Principal = principal;
            }
            //else
            //{
            //  //The user is unauthorize and return 401 status  
            //  var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            //  var tsc = new TaskCompletionSource<HttpResponseMessage>();
            //  tsc.SetResult(response);
            //  return tsc.Task;
            //}
          }
          else if (authHeader != null && "Bearer".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
          {
            var token = authHeader.Parameter;

            if ("ABC".Equals(token, StringComparison.OrdinalIgnoreCase))
            {
              IPrincipal principal = new GenericPrincipal(new GenericIdentity("Riaan"), "Administrator".Split(','));
              var ctx = request.GetRequestContext();
              ctx.Principal = principal;
            }
          }
          else
          {
            ////Bad Request request because Authentication header is set but value is null  
            //var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            //var tsc = new TaskCompletionSource<HttpResponseMessage>();
            //tsc.SetResult(response);
            //return tsc.Task;
          }
        }
      }
      catch
      {
        ////User did not set Authentication header  
        //var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
        //var tsc = new TaskCompletionSource<HttpResponseMessage>();
        //tsc.SetResult(response);
        //return tsc.Task;
      }

      return base.SendAsync(request, cancellationToken);
    }
  }
}
