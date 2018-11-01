using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Dataaccess
{
    public class Class1
    {
      
        DataTable TableInfo = new DataTable();
       
        public DataTable getTableInfo(string DatabaseTableName)
        {

            SqlDataAdapter adapter;
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=.;Initial Catalog=FolderUpload;Integrated Security=True";
                connection.Open();
                string sql = "select Path+Name as FullPath from " + DatabaseTableName;
                adapter = new SqlDataAdapter(sql, connection);
                //SqlCommand sqlCommand = new SqlCommand(sql, connection);
                adapter.Fill(TableInfo);

            }
            return TableInfo;
        }
        public DataTable getTableInfo(string DatabaseTableName,string SqlQuery)
        {

            SqlDataAdapter adapter;
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=.;Initial Catalog=FolderUpload;Integrated Security=True";
                connection.Open();
               // string sql = "select * from " + DatabaseTableName;
                adapter = new SqlDataAdapter(SqlQuery, connection);
                //SqlCommand sqlCommand = new SqlCommand(sql, connection);
                adapter.Fill(TableInfo);

            }
            return TableInfo;
        }
    }
}
