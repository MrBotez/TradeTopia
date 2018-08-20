using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTopia.Hub.Windows.Service.Data
{
  public class usr_User
  {
    public string usr_Name { get; set; }
    public string usr_Password { get; set; }
    public Guid usr_Token { get; set; }

    public static string GetSQLCreate()
    {
      var sb = new StringBuilder(1024);
      sb.AppendLine("CREATE TABLE [usr_User] (                ");
      sb.AppendLine("  [usr_Name] varchar(100) NOT NULL,      ");
      sb.AppendLine("  [usr_Password] varchar(100) NOT NULL,  ");
      sb.AppendLine("  [usr_Token] GUID NOT NULL,             ");
      sb.AppendLine("  CONSTRAINT[sqlite_autoindex_usr_User_1] PRIMARY KEY([usr_Name])); ");

      return sb.ToString();
    }

    public static usr_User GetByName(string value)
    {
      var sb = new StringBuilder();
      sb.AppendLine($@"select * from usr_User where usr_Name = '{value}'");
      var lst = DataContext.Instance.ExecuteSelect(sb.ToString());

      var usr = lst.FirstOrDefault();
      if (usr != null)
      {
        var rv = new usr_User()
        {
          usr_Name = Convert.ToString(usr.Columns[0].Value),
          usr_Password = Convert.ToString(usr.Columns[1].Value),
          usr_Token = Guid.Parse(Convert.ToString(usr.Columns[2].Value))
        };

        return rv;
      }

      return null;
    }

    public static List<usr_User> GetAll()
    {
      var sb = new StringBuilder();
      sb.AppendLine($@"select * from usr_User");
      var lst = DataContext.Instance.ExecuteSelect(sb.ToString());

      var rv = lst.Select(x => new usr_User()
      {
        usr_Name = Convert.ToString(x.Columns[0].Value),
        usr_Password = Convert.ToString(x.Columns[1].Value),
        usr_Token = Guid.Parse(Convert.ToString(x.Columns[2].Value))
      }).ToList();

      return rv;
    }

    public static usr_User GetByToken(Guid value)
    {
      var sb = new StringBuilder();
      sb.AppendLine($@"select * from usr_User where usr_Token = X'{value.ToHexString()}'");
      var lst = DataContext.Instance.ExecuteSelect(sb.ToString());

      var usr = lst.FirstOrDefault();
      if (usr != null)
      {
        var rv = new usr_User()
        {
          usr_Name = Convert.ToString(usr.Columns[0].Value),
          usr_Password = Convert.ToString(usr.Columns[1].Value),
          usr_Token = Guid.Parse(Convert.ToString(usr.Columns[2].Value))
        };

        return rv;
      }

      return null;
    }
  }
}
