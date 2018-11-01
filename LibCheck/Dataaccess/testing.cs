﻿using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess
{
    public class Testing
    {
        UploadTimeLog UploadTimeLog = new UploadTimeLog();

        public  void UploadDocument(ClientContext clientContext, string sourceFilePath, string serverRelativeDestinationPath)
        {
            UploadTimeLog UploadTimeLog = new UploadTimeLog();
            using (var fs = new FileStream(sourceFilePath, FileMode.Open))
            {
                DateTime StartTime = DateTime.Now;
                var fi = new FileInfo(sourceFilePath);
                Microsoft.SharePoint.Client.File.SaveBinaryDirect(clientContext, serverRelativeDestinationPath, fs, true);
                DateTime EndTime = DateTime.Now;
                UploadTimeLog.UploadTimeWrite(sourceFilePath.Split('/').Last(), sourceFilePath, StartTime, EndTime);
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
                        Console.WriteLine(fi.FullName);
                        clientContext.Load(folder);
                        clientContext.ExecuteQuery();
                        UploadDocument(clientContext, fi.FullName, folder.ServerRelativeUrl + "/" + fi.Name);
                    }

                    subDirs = folderInfo.GetDirectories();

                    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                    {
                        Folder subFolder = folder.Folders.Add(dirInfo.Name);
                        clientContext.ExecuteQuery();
                        UploadFolder(clientContext, dirInfo, subFolder);
                    }
                }
            }

            public  void UploadFoldersRecursively(ClientContext clientContext, string sourceFolder, string destinationLigraryTitle)
            {
                Web web = clientContext.Web;
                var query = clientContext.LoadQuery(web.Lists.Where(p => p.Title == destinationLigraryTitle));
                clientContext.ExecuteQuery();
                List documentsLibrary = query.FirstOrDefault();
                var folder = documentsLibrary.RootFolder;
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sourceFolder);

                clientContext.Load(documentsLibrary.RootFolder);
                clientContext.ExecuteQuery();

                folder = documentsLibrary.RootFolder.Folders.Add(di.Name);
                clientContext.ExecuteQuery();

                UploadFolder(clientContext, di, folder);
            }
        
    }
}
