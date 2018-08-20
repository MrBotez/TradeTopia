using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TradeTopia.Hub.Windows.Service.WebAPI.Controller
{
  [RoutePrefix("api/auth")]
  public class IdentityController : ApiController
  {
    [Route("")]
    [HttpPost]
    public IHttpActionResult CreateRegistration([FromBody]string value)
    {

      return Ok();
    }

    //[Route("")]
    //[HttpGet]
    //[Authorize(Roles = "Administrator")]
    //public IHttpActionResult GetAllUsers()
    //{
    //  var lst = Data.usr_User.GetAll();

    //  return Ok(lst);
    //}

    //[Route("")]
    //[HttpDelete]
    //[Authorize(Roles = "Administrator")]
    //public IHttpActionResult DeleteUser(Guid userToken)
    //{
    //  //var lst = Data.usr_User.GetAll();

    //  return Ok();
    //}

    [Route("")]
    [HttpGet]
    public IHttpActionResult CreateSessionToken([FromBody]string value)
    {
      if (RequestContext.Principal != null)
      {
        var userName = RequestContext.Principal.Identity.Name;

        var rv = new Model.SessionToken() { Token = Guid.NewGuid() };

        return Ok(rv);
      }

      return Content(System.Net.HttpStatusCode.Unauthorized, Model.ErrorMessage.Unauthorized());
    }
  }
}
