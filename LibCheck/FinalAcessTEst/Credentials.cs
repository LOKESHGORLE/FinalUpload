using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;


namespace FinalAcessTEst
{
    class Credentials
    {
        public string userName = "lokesh.gorle@acuvate.com";
        public SecureString password = GetPassword();
        private static SecureString GetPassword()
        {
            ConsoleKeyInfo info;
            //Get the user's password as a SecureString  
            SecureString securePassword = new SecureString();
            do
            {
                info = Console.ReadKey(true);
                if (info.Key != ConsoleKey.Enter)
                {
                    securePassword.AppendChar(info.KeyChar);
                }
            }
            while (info.Key != ConsoleKey.Enter);
            return securePassword;

        }
    }
}
