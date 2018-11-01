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

            DateTime start;
            DateTime end;
            TimeSpan time;
            string domainrootfolder = "/teams/ExampleGratia/LokeshPractice/UploadFolderTest";
            string localrootfolder = @"D:/UploadFolderTest";
            string fullPath = @"D:\UploadFolderTest/New Microsoft Word Document";

            DataTable dt = new DataTable();
            string SQl = "select Path+'/'+Name as FullPath from FileInfo;";
            start = DateTime.Now;
            end = DateTime.Now;

            dt = Dal.getTableInfo("FileInfo",SQl);
            foreach(DataRow rw in dt.Rows)
            {
                
                Console.WriteLine(rw["FullPath"]);
            }

           


            DirectorySecurity dSecurity = Directory.GetAccessControl(localrootfolder);

            foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(NTAccount)))
            {
               

                
                Console.WriteLine(rule.IdentityReference.Value + ":" + rule.FileSystemRights.ToString());
                if (rule.IdentityReference.Value.ToLower() == rule.IdentityReference.Value)
                {
                    Console.WriteLine(rule.FileSystemRights);
                }

                if (rule.FileSystemRights == FileSystemRights.Read)
                {
                    Console.WriteLine("entered2");
                    Console.WriteLine("Account:{0}", rule.IdentityReference.Value);
                }
            }

            end = DateTime.Now;
            time = DateTime.Now - start;
            Console.WriteLine(start.ToString("hh.mm.ss.ffffff"));
            Console.WriteLine(end.ToString("hh.mm.ss.ffffff"));


            string Text = String.Format("{0}.{1}", time.Seconds, time.Milliseconds.ToString());
            Console.WriteLine(Text);

            Console.WriteLine("Enter your password.");
            Credentials crd = new Credentials();
            using (var context = new ClientContext("https://acuvatehyd.sharepoint.com/teams/ExampleGratia"))

            {
                Console.WriteLine("----------- sharepoint--------------");
                context.Credentials = new SharePointOnlineCredentials(crd.userName, crd.password);
                //GetFile(context);
               
                 tests.UploadFoldersRecursively(context, @"D:\UploadFolderTest","LokeshPractice");
                Console.WriteLine("Exceution done");

            }

            Console.Read();
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
