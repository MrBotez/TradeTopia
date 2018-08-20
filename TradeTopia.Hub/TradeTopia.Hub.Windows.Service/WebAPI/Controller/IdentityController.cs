using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TradeTopia.Hub.Windows.Service.WebAPI.Controller
{
  [RoutePrefix("identity")]
  public class IdentityController : ApiController
  {
    [Route("register")]
    // POST api/identity 
    public void Post([FromBody]string value)
    {
    }

    // GET api/identity 
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    // GET api/identity/5 
    public string Get(int id)
    {
      return "value";
    }

    //// POST api/identity 
    //public void Post([FromBody]string value)
    //{
    //}

    // PUT api/identity/5 
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/identity/5 
    public void Delete(int id)
    {
    }
  }
}
