using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FinalAcessTEst
{
    class Program
    {
        static void Main(string[] args)
        {
            string DirectoryPath = @"C:\Users\lokesh.gorle\Desktop\SPFx PPT";

            List<String> ab= DirSearch(DirectoryPath);
            foreach(string i in ab)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("--------------with second one");
            ab = GetAllFiles(DirectoryPath);
            foreach (string i in ab)
            {
                Console.WriteLine(i);
            }
            Console.Read();
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
