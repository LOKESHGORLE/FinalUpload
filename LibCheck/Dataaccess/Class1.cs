using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace Dataaccess
{
    public class Class1
    {
       // var connection =  System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
      //  public static String Constr = @"DataSource=ACUPC-208; Initial Catalog = FolderUpload; Integrated Security = True";
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
        public DataTable getTableInfo(string DatabaseTableName, string SqlQuery)
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

        public void InsertInto( string SqlQuery)
        {

            SqlConnection cn = new SqlConnection(@"Data Source=.;Initial Catalog=FolderUpload;Integrated Security=True");

            SqlCommand cmd = new SqlCommand(SqlQuery, cn);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();


        }

        public void InsertFilePermissions(int Id,string Username,string FilePermission)//copy
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=.;Initial Catalog=FolderUpload;Integrated Security=True";
                connection.Open();
                                              
                
                    SqlCommand sqlCommand = new SqlCommand("Proc_InsertFilePermissions", connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add("@pFileID", SqlDbType.Int).Value = Id;
                sqlCommand.Parameters.Add("@pUserName", SqlDbType.NVarChar).Value = Username;
                sqlCommand.Parameters.Add("@pFilePermission", SqlDbType.NVarChar).Value = FilePermission;

                sqlCommand.ExecuteNonQuery();
                
                connection.Close();
            }
        }

        public void InsertFolderPermissions(int Id, string Username, string FolderPermission)//copy
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=.;Initial Catalog=FolderUpload;Integrated Security=True";
                connection.Open();


                SqlCommand sqlCommand = new SqlCommand("Proc_InsertFolderPermissions", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add("@pFolderID", SqlDbType.Int).Value = Id;
                sqlCommand.Parameters.Add("@pUserName", SqlDbType.NVarChar).Value = Username;
                sqlCommand.Parameters.Add("@pFolderPermission", SqlDbType.NVarChar).Value = FolderPermission;

                sqlCommand.ExecuteNonQuery();

                connection.Close();
            }
        }

        public DataTable GetMappingRole(string MappedRole)
        {
            SqlDataAdapter adapter;
            using (SqlConnection connection = new SqlConnection())
            {
                DataTable TableInfo1 = new DataTable();
                connection.ConnectionString = @"Data Source=.;Initial Catalog=FolderUpload;Integrated Security=True";
                connection.Open();
                string SqlQuery = "select RoleType from RoleTypes  where Id in (select RoleTypesId from FolderToSPPermissions where FolderPermissionsId = (select Id from FolderPermissions where PermissionLevel = '"+MappedRole+"')); ";  
                adapter = new SqlDataAdapter(SqlQuery, connection);
                adapter.Fill(TableInfo1);             
                connection.Close();
                return TableInfo1;
            }
        }
        public DataTable GetUserRoleForFile(string FilePath)
        {
            SqlDataAdapter adapter=new SqlDataAdapter();
            using (SqlConnection connection = new SqlConnection())
            {
                DataTable TableInfo1 = new DataTable();
                connection.ConnectionString = @"Data Source=.;Initial Catalog=FolderUpload;Integrated Security=True";
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("Proc_GetUserRoleforFile", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add("@pFilePath", SqlDbType.NVarChar).Value = FilePath;
                adapter.SelectCommand = sqlCommand;
                adapter.Fill(TableInfo1);
                return TableInfo1;
            }
           
        }
        public DataTable GetUserRoleForFolder(string FolderPath)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            using (SqlConnection connection = new SqlConnection())
            {
                DataTable TableInfo1 = new DataTable();
                connection.ConnectionString = @"Data Source=.;Initial Catalog=FolderUpload;Integrated Security=True";
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("Proc_GetUserRoleforFolder", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add("@pFilePath", SqlDbType.NVarChar).Value = FolderPath;
                adapter.SelectCommand = sqlCommand;
                adapter.Fill(TableInfo1);
                return TableInfo1;
            }

        }
    }
}
