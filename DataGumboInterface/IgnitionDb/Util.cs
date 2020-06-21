using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace IgnitionDb
{
    public class Util
    {
        public string ConnString { get; set; }

        public Util(string connString)
        {
            ConnString = connString;
        }

        public IEnumerable<GumboLogRow> GetRecentRows(int? limit, DateTime? startDtm = null, DateTime? endDtm = null)
        {
            List<GumboLogRow> ret = new List<GumboLogRow>();
            using (MySqlConnection conn = new MySqlConnection(ConnString))
            {
                string sql = "select * from gumbo_log where 1=1 ";
                if (startDtm.HasValue)
                {
                    sql += string.Format(" and logDtm >= '{0:yyyy-MM-dd H:mm}' ", startDtm);
                }
                if (endDtm.HasValue)
                {
                    sql += string.Format(" and logDtm <= '{0:yyyy-MM-dd H:mm}' ", endDtm);
                }
                sql += " order by logId desc ";
                if (limit.HasValue)
                {
                    sql += string.Format(" limit {0} ", limit);
                }
                
                using (MySql.Data.MySqlClient.MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn))
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();

                    adapter.Fill(ds);
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        GumboLogRow glr = new GumboLogRow();
                        glr.logId = Convert.ToInt32(dr["logID"]);
                        glr.logDtm = Convert.ToDateTime(dr["logDtm"]);
                        glr.path = Convert.ToString(dr["path"]);
                        glr.rig = Convert.ToString(dr["rig"]);
                        glr.value = Convert.ToString(dr["value"]);
                        ret.Add(glr);
                    }
                }
            }
            return ret;
        }
    }
}
