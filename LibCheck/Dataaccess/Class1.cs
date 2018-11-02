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
                connection.Close();
            }
            
            return TableInfo;
        }

        public void InsertInto(string DatabaseTableName, string SqlQuery)
        {

            SqlConnection cn = new SqlConnection(@"Data Source=.;Initial Catalog=FolderUpload;Integrated Security=True");

            SqlCommand cmd = new SqlCommand(SqlQuery, cn);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();


        }

        public void UpdateAfterDeleteServiceLine()//copy
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=.;Initial Catalog=NotificationHub;Integrated Security=True";
                connection.Open();
               
               
                //foreach (string slms in Slm1)
                //{
                //    SqlCommand sqlCommand1 = new SqlCommand("Proc_InsertServLineOpManagers", connection);
                //    sqlCommand1.CommandType = CommandType.StoredProcedure;
                //    //sqlCommand1.Parameters.Add("@pId", SqlDbType.Int).Value =pId;
                //    sqlCommand1.Parameters.Add("@pServiceLineId", SqlDbType.Int).Value = pServLineId;
                   
                //    sqlCommand1.ExecuteNonQuery();
                //}
                connection.Close();
            }
       }
    }
}
