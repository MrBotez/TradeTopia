using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTopia.Hub.Windows.Service.WebAPI.Model
{
  public class ErrorMessage
  {
    public string Message { get; set; }

    public static ErrorMessage Unauthorized() { return new ErrorMessage() { Message = "You are not authorized" }; }
  }
}
