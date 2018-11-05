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
            int cnt = 0;


            foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(NTAccount)))
            {

                //Console.WriteLine(rule.IdentityReference.Value + ":" + rule.FileSystemRights.ToString());
                string[] UsernameSplit = rule.IdentityReference.Value.Split('\\');
                string DomainName = UsernameSplit[0];
                if (DomainName == Environment.UserDomainName)
                {


                    string[] splitRights = rule.FileSystemRights.ToString().Split(',');
                    user = rule.IdentityReference.Value.Split(Convert.ToChar(92)).Last().ToString()+"@acuvate.com";
                    foreach (string right in splitRights)
                    {
                        Console.WriteLine(UsernameSplit[1] + ":" + right);

                        Dal.InsertFilePermissions(1,user, right.Trim());
                        
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
            int cnt = 0;


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
        static string AssignPermission(ClientContext ctx, string User, string folderUrl, List<RoleType> Mappedrole)
        {


            try
            {



                List dlname = ctx.Web.Lists.GetByTitle("LokeshPractice");
                string foldername = folderUrl.Split('\\').Last();
                Folder newFolder = dlname.RootFolder.Folders.Add(foldername);
                ctx.ExecuteQuery();

                foreach (RoleType roleType in Mappedrole)
                {
                    if (User.Length > 0)
                    {

                        newFolder.ListItemAllFields.BreakRoleInheritance(false, true);
                        var roles = new RoleDefinitionBindingCollection(ctx);
                        roles.Add(ctx.Web.RoleDefinitions.GetByType(roleType));
                        Microsoft.SharePoint.Client.Principal user1 = ctx.Web.EnsureUser(User + "@" + Environment.UserDomainName.ToLower() + ".com");
                        newFolder.ListItemAllFields.RoleAssignments.Add(user1, roles);
                        newFolder.Update();
                        ctx.ExecuteQuery();
                        Console.WriteLine("Complete");

                    }
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
