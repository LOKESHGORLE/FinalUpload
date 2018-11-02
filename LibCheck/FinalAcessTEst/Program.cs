using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Microsoft.SharePoint.Client;
using System.Security.AccessControl;
using System.Security.Principal;

namespace FinalAcessTEst
{
    class Program
    {


        static void Main(string[] args)
        {

            Dataaccess.Class1 Dal = new Dataaccess.Class1();
            Dataaccess.Testing tests = new Dataaccess.Testing();
            Dataaccess.testuploadDB dbtrail = new Dataaccess.testuploadDB();


            string domainrootfolder = "/teams/ExampleGratia/LokeshPractice/UploadFolderTest";
            string localrootfolder = @"D:/UploadFolderTest";
            string fullPath = @"D:\UploadFolderTest/New Microsoft Word Document";


            


            Console.WriteLine("-----------------------------------------------------------");
            //foreach (var value in Enum.GetValues(typeof(RoleType)))
            //{
            //    string SQl = "insert into RoleTypes values ('" + value.ToString()+ "');";
            //    Dal.InsertInto("FolderPermissions", SQl);
            //    Console.WriteLine(value.ToString());
            //}
            Console.WriteLine("-----------------------------------------------------------");

            //DataTable dt = new DataTable();
            //string SQl = "select Path+'/'+Name as FullPath from FileInfo;";
            //dt = Dal.getTableInfo("FileInfo",SQl);
            //foreach(DataRow rw in dt.Rows)
            //{

            //    Console.WriteLine(rw["FullPath"]);
            //}




            DirectorySecurity dSecurity = Directory.GetAccessControl(localrootfolder);
            //DirectorySecurity dSecurity1 = Directory.GetAccessControl(localrootfolder).AccessRightType.;
            AuthorizationRuleCollection collection = Directory.
                                           GetAccessControl(localrootfolder)
                                           .GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
            foreach (FileSystemAccessRule rule in collection)
            {
                Console.WriteLine(rule.FileSystemRights.ToString() + "     :    " + rule.AccessControlType);

            }
            Console.WriteLine("--------------------------------------1212121------------\n\n");
            foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(NTAccount)))
            {
                Console.WriteLine(rule.IdentityReference.Value + ":" + rule.FileSystemRights.ToString());

                //foreach (var value in Enum.GetValues(typeof(FileSystemRights)))
                //{
                //    //Console.WriteLine(value.ToString());
                //    if (rule.FileSystemRights.ToString() == value.ToString())
                //    {
                //        Console.WriteLine("entered2");
                //        Console.WriteLine("Account:{0}  right:{1}", rule.IdentityReference.Value, value.ToString());
                //    }
                //}

                //if (rule.IdentityReference.Value.ToLower() == rule.IdentityReference.Value)
                //{
                //   // Console.WriteLine(rule.FileSystemRights);
                //}


            }
            Console.WriteLine("---------------new one-------------------------\n\n");
            //Console.WriteLine(rule.IdentityReference.Value + ":" + rule.FileSystemRights.ToString());


            //directoryInfo();


            Console.WriteLine("Enter your password.");
            Credentials crd = new Credentials();
            using (var context = new ClientContext("https://acuvatehyd.sharepoint.com/teams/ExampleGratia"))

            {
                Console.WriteLine("----------- sharepoint--------------");
                context.Credentials = new SharePointOnlineCredentials(crd.userName, crd.password);
                //GetFile(context);

                //tests.UploadFoldersRecursively(context, @"D:\UploadFolderTest", "LokeshPractice");
                ListSPPermissions12(context);
                //dbtrail.UploadFolderstart(context, @"D:\UploadFolderTest", "LokeshPractice");


                Console.WriteLine("Exceution done");

            }

            Console.Read();
        }

        private static void ListSPPermissions12(ClientContext cxt)
        {
            var list = cxt.Web.Lists.GetByTitle("LokeshPractice");
            var items = list.GetItems(CamlQuery.CreateAllFoldersQuery());
            cxt.Load(items, icol => icol.Include(i => i.RoleAssignments.Include(ra => ra.Member), i => i.DisplayName));
            cxt.ExecuteQuery();
            foreach (var item in items)
            {
                Console.WriteLine("{0} folder permissions", item.DisplayName);
                foreach (var assignment in item.RoleAssignments)
                {
                    Console.WriteLine(assignment.Member.Title);
                }
            }
        }

        private static void ListSPPermissions3(ClientContext cxt)
        {

            List list = cxt.Web.Lists.GetByTitle("LokeshPractice");

            cxt.Load(list.RootFolder.Folders); //load the client object list.RootFolder.Folders
            cxt.ExecuteQuery();
            int FolderCount = list.RootFolder.Folders.Count;

            foreach (Microsoft.SharePoint.Client.Folder folder in list.RootFolder.Folders)
            {

                RoleAssignmentCollection roleAssCol = folder.ListItemAllFields.RoleAssignments;

                cxt.Load(roleAssCol);
                cxt.ExecuteQuery(); // Exception property ListItemAllFields not found

                foreach (RoleAssignment roleAss in roleAssCol)
                {
                    Console.WriteLine(roleAss.Member.Title);
                }
            }

        }

        protected static void directoryInfo()
        {
            var di = new DirectoryInfo(@"D:/UploadFolderTest");
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                Console.WriteLine(dir.FullName + "<br/>");
                DirectorySecurity ds = dir.GetAccessControl(AccessControlSections.Access);
                foreach (FileSystemAccessRule fsar in ds.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
                {
                    string userName = fsar.IdentityReference.Value;
                    string userRights = fsar.FileSystemRights.ToString();
                    string userAccessType = fsar.AccessControlType.ToString();
                    string ruleSource = fsar.IsInherited ? "Inherited" : "Explicit";
                    string rulePropagation = fsar.PropagationFlags.ToString();
                    string ruleInheritance = fsar.InheritanceFlags.ToString();
                    Console.WriteLine(userName + " : " + userAccessType + " : " + userRights + " : " + ruleSource + " : " + rulePropagation + " : " + ruleInheritance + "<br/>");
                }
            }
        }



        public static void GetFile(ClientContext cxt)
        {
            List UploadToDocLib = cxt.Web.Lists.GetByTitle("LokeshPractice");
            CamlQuery camlQuery = new CamlQuery();
            Web web = cxt.Web;
            cxt.Load(web);
            cxt.ExecuteQuery();
            camlQuery.ViewXml = @"<View><Query></Query></View>";

            //camlQuery.ViewXml = @"<View><Query><Where><Eq><FieldRef Name='Name'/><Value Type='Text'>SharePointUploadList</Value></Eq></Where></Query></View>";
            ListItemCollection FoldersinLib = UploadToDocLib.GetItems(camlQuery);
            cxt.Load(FoldersinLib);
            cxt.ExecuteQuery();
            Folder rootfolder = UploadToDocLib.RootFolder;

            cxt.Load(rootfolder);
            cxt.ExecuteQuery();
            Console.WriteLine(rootfolder.ServerRelativeUrl);



            foreach (ListItem folder in FoldersinLib)
            {
                // Console.WriteLine(file.Title);
                Console.WriteLine(folder.FieldValues["FileLeafRef"]);
                cxt.Load(folder.Folder);
                cxt.ExecuteQuery();
                Console.WriteLine(folder.Folder.ServerRelativeUrl);

            }
        }

        public static List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return files;
        }
        public static List<String> GetAllFiles(String directory)
        {
            return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).ToList();
        }
    }
}
