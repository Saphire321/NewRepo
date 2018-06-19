using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class DataHandler
    {
        private static DataConn dbc;
        private static DataSet ds;

        public DataHandler()
        {
            dbc = new DataConn();
            ds = new DataSet();
        }

        public DataTable GetData(string table)
        {
            ds = dbc.Read(table);
            return ds.Tables[table];
        }

        public bool Insert(Dictionary<string, object> values, string table)
        {
            try
            {
                DataRow dr = ds.Tables[table].NewRow();
                foreach (var item in values)
                {
                    dr[item.Key] = item.Value;
                }
                ds.Tables[table].Rows.Add(dr);

                if (dbc.Write(ds, table))
                {
                    return true;
                }

            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            return false;
        }

        public int GetRow(DataSet ds, string table, string identifier)
        {
            int index = 0;
            foreach (DataRow dr in ds.Tables[table].Rows)
            {
                if (dr == ds.Tables[table].Rows.Find(identifier))
                {
                    index = (int)ds.Tables[table].Rows.IndexOf(dr);
                }
            }
            return index;
        }

        public bool Update(Dictionary<string, object> value, string table, string identifier)
        {
            bool upd = false;
            try
            {
                foreach (var item in value)
                {
                    ds.Tables[table].Rows[GetRow(ds, table, identifier)][item.Key] = item.Value;
                }
                dbc.Write(ds, table);
                upd = true;
            }
            catch (SqlException e)
            {

                throw new Exception(e.Message);
            }
            return upd;
        }

        public bool Delete(string table, string identifier)
        {
            bool del = false;
            try
            {
                ds.Tables[table].Rows[GetRow(ds, table, identifier)].Delete();
                dbc.Write(ds, table);
                del = true;
            }
            catch (SqlException e) { throw new Exception(e.Message); }
            return del;
        }
    }
}
