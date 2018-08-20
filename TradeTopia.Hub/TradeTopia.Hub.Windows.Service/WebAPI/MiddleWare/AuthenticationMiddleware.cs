using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TradeTopia.Hub.Windows.Service.WebAPI.MiddleWare
{
  public class AuthenticationMiddleware : OwinMiddleware
  {
    public AuthenticationMiddleware(OwinMiddleware next) : base(next)
    {
    }

    public async override Task Invoke(IOwinContext context)
    {
      //context.Request.OnSendingHeaders(state =>
      //{
      //  var resp = (OwinResponse)state;

      //  if (resp.StatusCode == 401)
      //  {
      //    resp.SetHeader("WWW-Authenticate", "Basic");
      //  }
      //}, response);

      var header = context.Request.Headers.ContainsKey("Authorization") ? context.Request.Headers["Authorization"] : "";

      if (!String.IsNullOrWhiteSpace(header))
      {
        var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);

        if ("Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
        {
          string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
          var parts = parameter.Split(':');

          string userName = parts[0];
          string password = parts[1];

          var usr = Data.usr_User.GetByName(userName);
          if (usr != null)
          {
            if (usr.usr_Password == password)
            {
              var claims = new[] {
                new Claim(ClaimTypes.Name, usr.usr_Name),
                new Claim(ClaimTypes.Sid, Convert.ToString(usr.usr_Token))
              };
              var identity = new ClaimsIdentity(claims, "Basic");

              context.Authentication.User = new ClaimsPrincipal(identity);
            }
          }
        }
      }

      await Next.Invoke(context);
    }
  }
}
