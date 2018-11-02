using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client.DocumentManagement;

namespace Dataaccess
{
    public class Testing
    {
        UploadTimeLog UploadTimeLog = new UploadTimeLog();
        ErrrorLog ErrrorLog = new ErrrorLog();

        public  void UploadDocument(ClientContext clientContext, string sourceFilePath, string serverRelativeDestinationPath)
        {
            string  UploadStatus;
            UploadTimeLog UploadTimeLog = new UploadTimeLog();
            using (var fs = new FileStream(sourceFilePath, FileMode.Open))
            {
                DateTime StartTime = DateTime.Now;
                try {
                    var fi = new FileInfo(sourceFilePath);
                    Microsoft.SharePoint.Client.File.SaveBinaryDirect(clientContext, serverRelativeDestinationPath, fs, true);
                    UploadStatus = "Success";
                }
                catch(Exception ex)
                {
                    UploadStatus = "Failure";
                    throw ex;
                    //ErrrorLog.ErrorlogWrite(ex);
                    
                }                              
                    DateTime EndTime = DateTime.Now;
                    UploadTimeLog.UploadTimeWrite(sourceFilePath.Split('/').Last(), sourceFilePath, UploadStatus, StartTime, EndTime);
                                
            }
        }

            public  void UploadFolder(ClientContext clientContext, System.IO.DirectoryInfo folderInfo, Folder folder)
            {
                System.IO.FileInfo[] files = null;
                System.IO.DirectoryInfo[] subDirs = null;

                try
                {
                    files = folderInfo.GetFiles("*.*");
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                }

                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }

                if (files != null)
                {
                    foreach (System.IO.FileInfo fi in files)
                    {
                       // Console.WriteLine(fi.FullName);
                        clientContext.Load(folder);
                        clientContext.ExecuteQuery();
                        UploadDocument(clientContext, fi.FullName, folder.ServerRelativeUrl + "/" + fi.Name);
                    }

                    subDirs = folderInfo.GetDirectories();

                    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                    {
                    Console.WriteLine(dirInfo.FullName);
                    DirectorySecurity dSecurity = dirInfo.GetAccessControl();
                   // DirectorySecurity dSecurity = Directory.GetAccessControl(localrootfolder);

                    foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                    {
                       
                        Console.WriteLine(rule.IdentityReference.Value + ":" + rule.FileSystemRights.ToString());


                    }

                        Folder subFolder = folder.Folders.Add(dirInfo.Name);
                        clientContext.ExecuteQuery();
                        UploadFolder(clientContext, dirInfo, subFolder);
                    }
                }
            }

            public  void UploadFoldersRecursively(ClientContext clientContext, string sourceFolder, string destinationLibraryTitle)
            {
                string[] FolderArr= sourceFolder.Split('\\');
            foreach (string i in FolderArr)
            {
                Console.WriteLine(i);
            }


                Web web = clientContext.Web;
                var query = clientContext.LoadQuery(web.Lists.Where(p => p.Title == destinationLibraryTitle));
                clientContext.ExecuteQuery();
                List documentsLibrary = query.FirstOrDefault();
                var folder = documentsLibrary.RootFolder;
                //System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sourceFolder);

                clientContext.Load(documentsLibrary.RootFolder);
                clientContext.ExecuteQuery();

               // folder = documentsLibrary.RootFolder.Folders.Add(di.Name);
                clientContext.ExecuteQuery();

               // UploadFolder(clientContext, di, folder);
            }
        
    }
}
