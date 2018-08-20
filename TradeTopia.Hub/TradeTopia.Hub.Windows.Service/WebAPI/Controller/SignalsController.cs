using System.Collections.Generic;
using System.Web.Http;

namespace TradeTopia.Hub.Windows.Service.WebAPI.Controller
{
  [Authorize]
  public class SignalsController : ApiController
  {
    // GET api/signals 
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    // GET api/signals/5 
    public string Get(int id)
    {
      return "value";
    }

    // POST api/signals 
    public void Post([FromBody]string value)
    {
    }

    // PUT api/signals/5 
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/signals/5 
    public void Delete(int id)
    {
    }
  }
}
