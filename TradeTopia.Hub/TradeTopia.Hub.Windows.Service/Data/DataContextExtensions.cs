using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTopia.Hub.Windows.Service.Data
{
  public class TableColumn
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public object Value { get; set; }
  }

  public class TableRow
  {
    public TableRow()
    {
      Columns = new Dictionary<int, TableColumn>();
    }

    public int Id { get; set; }
    public Dictionary<int, TableColumn> Columns { get; set; }
  }

  public static class DataContextExtensions
  {
    public static String ToHexString(this Guid value)
    {
      return String.Join("", value.ToByteArray().Select(i => i.ToString("x2")));
    }

    public static IEnumerable<TableRow> ToList(this DbDataReader rdr)
    {
      if (rdr != null)
      {
        var rowIdx = 0;

        var schema = rdr.GetSchemaTable();
        var columnNames = new List<String>();

        foreach (DataRow row in schema.Rows)
        {
          columnNames.Add(row["ColumnName"].ToString());
        }
        
        while (rdr.Read())
        {
          var row = new TableRow();
          row.Id = rowIdx;

          for (var colIdx = 0; colIdx < rdr.FieldCount; colIdx++)
          {
            row.Columns[colIdx] = new TableColumn() { Id = colIdx, Name = columnNames[colIdx], Value = rdr.GetValue(colIdx) };
          }

          yield return row;

          rowIdx++;
        }
      }
    }
  }
}
