using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess
{
    public class perm
    {
        public static void GetFilePermmssion(string FilePath)
        {
            Class1 Dal = new Class1();
            string user;
            FileSecurity dSecurity = System.IO.File.GetAccessControl(FilePath);

            Console.WriteLine("--------------------------------------Users and their rights------------\n\n");



            foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(NTAccount)))
            {

                //Console.WriteLine(rule.IdentityReference.Value + ":" + rule.FileSystemRights.ToString());
                string[] UsernameSplit = rule.IdentityReference.Value.Split('\\');
                string DomainName = UsernameSplit[0];
                if (DomainName == Environment.UserDomainName)
                {


                    string[] splitRights = rule.FileSystemRights.ToString().Split(',');
                    user = rule.IdentityReference.Value.Split(Convert.ToChar(92)).Last().ToString() + "@acuvate.com";
                    foreach (string right in splitRights)
                    {
                        Console.WriteLine(UsernameSplit[1] + ":" + right);

                        Dal.InsertFilePermissions(1, user, right.Trim());

                    }
                }

            }

        }

        public static void GetFolderPermmssion(string FolderPath)
        {
            Class1 Dal = new Class1();
            string user;
            DirectorySecurity dSecurity = Directory.GetAccessControl(FolderPath);

            Console.WriteLine("--------------------------------------Users and their rights------------\n\n");



            foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(NTAccount)))
            {

                //Console.WriteLine(rule.IdentityReference.Value + ":" + rule.FileSystemRights.ToString());
                string[] UsernameSplit = rule.IdentityReference.Value.Split('\\');
                string DomainName = UsernameSplit[0];
                if (DomainName == Environment.UserDomainName)
                {


                    string[] splitRights = rule.FileSystemRights.ToString().Split(',');
                    user = rule.IdentityReference.Value.Split(Convert.ToChar(92)).Last().ToString() + "@acuvate.com";
                    foreach (string right in splitRights)
                    {
                        Console.WriteLine(UsernameSplit[1] + ":" + right);

                        Dal.InsertFolderPermissions(1, user, right.Trim());

                    }
                }

            }

        }

        public void GetUserRoleforFile(ClientContext ctx, string FilePath)
        {
            Class1 Dal = new Class1();
            string user;
            DataTable UserRolesTab = new DataTable();

            UserRolesTab = Dal.GetUserRoleForFile(FilePath);
            foreach (DataRow UserRoleRow in UserRolesTab.Rows)
            {
                foreach (RoleType value in Enum.GetValues(typeof(RoleType)))
                {
                    Console.WriteLine(value.ToString());
                    if (value.ToString() == UserRoleRow["RoleType"].ToString())
                    {
                        user = UserRoleRow["Name"].ToString();
                       // Console.WriteLine("ROle type : " + value.ToString());
                        // Console.WriteLine("User Name:   "+UserRoleRow["name"])
                        AssignFilePermission(ctx, user, FilePath, value);
                    }
                }
            }
        }


        static string AssignFilePermission(ClientContext ctx, string User, string fileUrl, RoleType Mappedrole)
        {
            try
            {

                var CurrFile = ctx.Web.GetFileByServerRelativeUrl(fileUrl);
               // ctx.Load(file, f => f.ListItemAllFields["FileRef"]);
                ctx.ExecuteQuery();
                CurrFile.ListItemAllFields.BreakRoleInheritance(false, true);


                //List dlname = ctx.Web.Lists.GetByTitle("LokeshPractice");
                //string fileName = fileUrl.Split('\\').Last();
                //Microsoft.SharePoint.Client.File newFolder = dlname.RootFolder.Files.GetByUrl(fileUrl);
                ctx.ExecuteQuery();

                if (User.Length > 0)
                {
                    CurrFile.ListItemAllFields.BreakRoleInheritance(false, true);
                    var roles = new RoleDefinitionBindingCollection(ctx);
                    roles.Add(ctx.Web.RoleDefinitions.GetByType(Mappedrole));
                    Microsoft.SharePoint.Client.Principal user1 = ctx.Web.EnsureUser(User);
                    CurrFile.ListItemAllFields.RoleAssignments.Add(user1, roles);
                    CurrFile.Update();
                    ctx.ExecuteQuery();
                    Console.WriteLine("Complete");
                }

                ctx.ExecuteQuery();
                return "Permission Assigned";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public void GetUserRoleforFolder(ClientContext ctx, string FolderPath)
        {
            Class1 Dal = new Class1();
            string user;
            DataTable UserRolesTab = new DataTable();

            UserRolesTab = Dal.GetUserRoleForFolder(FolderPath);
            foreach (DataRow UserRoleRow in UserRolesTab.Rows)
            {
                foreach (RoleType value in Enum.GetValues(typeof(RoleType)))
                {
                    Console.WriteLine(value.ToString());
                    if (value.ToString() == UserRoleRow["RoleType"].ToString())
                    {
                        user = UserRoleRow["Name"].ToString();
                       
                        AssignFolderPermission(ctx, user, FolderPath, value);
                    }
                }
            }
        }
        static string AssignFolderPermission(ClientContext ctx, string User, string folderUrl, RoleType Mappedrole)
        {
            try
            {
                List dlname = ctx.Web.Lists.GetByTitle("LokeshPractice");
                string foldername = folderUrl.Split('\\').Last();
                Folder newFolder = dlname.RootFolder.Folders.Add(foldername);
                ctx.ExecuteQuery();
               
                    if (User.Length > 0)
                    {
                        newFolder.ListItemAllFields.BreakRoleInheritance(false, true);
                        var roles = new RoleDefinitionBindingCollection(ctx);
                        roles.Add(ctx.Web.RoleDefinitions.GetByType(Mappedrole));
                        Microsoft.SharePoint.Client.Principal user1 = ctx.Web.EnsureUser(User);
                        newFolder.ListItemAllFields.RoleAssignments.Add(user1, roles);
                        newFolder.Update();
                        ctx.ExecuteQuery();
                        Console.WriteLine("Complete");
                    }
                
                ctx.ExecuteQuery();
                return "Permission Assigned";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
