using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess
{
    public  class UploadTimeLog
    {

        public  void UploadTimeWrite(string FileName, string FilePath,string Status, DateTime StartTime,DateTime EndTime)
        {
            TimeSpan UploadSpan = EndTime - StartTime;
            string UploadSpanString = String.Format("{0}.{1}", UploadSpan.Seconds, UploadSpan.Milliseconds.ToString());
            string Loggingrecord = DateTime.Today.ToString("dd-MM-yy") + " File Name: " + FileName + "  File Path:  " + FilePath +" "+ Status + "  Started  " + StartTime.ToString("hh.mm.ss.ffffff") + " Ended  " + EndTime.ToString("hh.mm.ss.ffffff") + " Total time to upload " + UploadSpanString;

            string filepath = @"D:\UploadTimeDetailsFile";  //Text File Path
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(filepath))
            {
                sw.WriteLine("-----------Upload Details on " + " " + DateTime.Now.ToString() + "-----------------");
                sw.WriteLine("-------------------------------------------------------------------------------------");
                sw.WriteLine(Loggingrecord);
                sw.WriteLine("--------------------------------*End*------------------------------------------");
                 sw.Flush();
                sw.Close();
            }
        }
    }
}
