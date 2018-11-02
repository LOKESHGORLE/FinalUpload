using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess
{
    public class testuploadDB
    {

        public void UploadFolderstart(ClientContext clientContext, string sourceFolder, string destinationLibraryTitle)
        {
            string[] FolderArr = sourceFolder.Split('\\');
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

            //UploadFolder(clientContext, di, folder);
        }

    }
}
