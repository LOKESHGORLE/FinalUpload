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
        public static void GetPermmssion(ClientContext context, string localrootfolder)
        {
            Class1 Dal = new Class1();
            string user;
            DirectorySecurity dSecurity = Directory.GetAccessControl(localrootfolder);

            Console.WriteLine("--------------------------------------Users and their rights------------\n\n");
            int cnt = 0;


            foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(NTAccount)))
            {

                Console.WriteLine(rule.IdentityReference.Value + ":" + rule.FileSystemRights.ToString());
                string[] splitRole = rule.FileSystemRights.ToString().Split(',');
                user = rule.IdentityReference.Value.Split(Convert.ToChar(92)).Last().ToString();
                foreach (string s in splitRole)
                {
                    Console.WriteLine(rule.IdentityReference.Value + ":" + s);

                    DataTable MappedRoles = Dal.GetMappingRole(s);
                    List<RoleType> MaproleList = new List<RoleType>();

                    foreach (DataRow row in MappedRoles.Rows)
                    {
                        foreach (RoleType value in Enum.GetValues(typeof(RoleType)))
                        {
                            Console.WriteLine(value.ToString());
                            if (value.ToString() == row["RoleType"].ToString())
                            {
                                MaproleList.Add(value);
                            }
                        }


                    }
                    if (MaproleList.Count > 0)
                    {
                        AssignPermission(context, user, localrootfolder, MaproleList);
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
