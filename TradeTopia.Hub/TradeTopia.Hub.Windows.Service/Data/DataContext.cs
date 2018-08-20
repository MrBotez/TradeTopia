using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTopia.Hub.Windows.Service.Data
{
  public class DataContext : IDisposable
  {
    #region Singleton stuff

    private static volatile DataContext instance;
    private static object syncRoot = new Object();

    public static DataContext Instance
    {
      get
      {
        if (instance == null)
        {
          lock (syncRoot)
          {
            if (instance == null)
              instance = new DataContext();
          }
        }

        return instance;
      }
    }

    #endregion

    public string DatabaseLocation { get { return System.Reflection.Assembly.GetEntryAssembly().Location.Replace(@".exe", @".sqlite"); } }

    protected SQLiteConnection sQLiteConnection = null;

    private DataContext()
    {

    }

    public async virtual void Initialize()
    {
      bool createdNewDatabase = false;

      var fi = new FileInfo(DatabaseLocation);
      if (!fi.Exists)
      {
        SQLiteConnection.CreateFile(fi.FullName);
        createdNewDatabase = true;
      }

      sQLiteConnection = new SQLiteConnection($"Data Source={fi.Name};Version=3;");
      sQLiteConnection.Open();

      if (createdNewDatabase)
      {
        var sb = new StringBuilder(1024);
        sb.AppendLine(usr_User.GetSQLCreate());

        var cmd = new SQLiteCommand(sb.ToString(), sQLiteConnection);
        await cmd.ExecuteNonQueryAsync();
      }

      var usr = usr_User.GetByToken(Guid.Parse("{9ACD822F-F5C1-493E-A5FF-B99DC22FD73F}"));
    }

    public void Dispose()
    {
      sQLiteConnection?.Close();
    }

    public List<TableRow> ExecuteSelect(string sql)
    {
      var cmd = new SQLiteCommand(sql, sQLiteConnection);
      var rdr = cmd.ExecuteReader();

      return rdr.ToList().ToList();
    }
  }
}
