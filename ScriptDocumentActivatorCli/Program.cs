using System;
using System.Diagnostics;
using System.IO;

namespace ScriptDocumentActivatorCli
{
    internal class Program
    {
        const string V18Path = @"C:\Program Files (x86)\Microsoft SQL Server Management Studio 18\Common7\IDE\Ssms.exe";
        const string V19Path = @"C:\Program Files (x86)\Microsoft SQL Server Management Studio 19\Common7\IDE\Ssms.exe";

        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 4)
            {
                Console.WriteLine("Valid usage: ScriptDocumentActivatorCli.exe DbServer DbName [UserName] [Password]");
                return;
            }

            OpenSSMSScriptDocument(args[0], args[1], args.Length > 2 ? args[2] : null, args.Length > 3 ? args[3] : null);
        }

        static void OpenSSMSScriptDocument(string dbServer, string dbName, string userName, string password)
        {
            if (Process.GetProcessesByName("ssms").Length == 0)
            {
                string ssmsExePath = V19Path;
                if (!File.Exists(ssmsExePath))
                    ssmsExePath = V18Path;
                if (!File.Exists(ssmsExePath))
                    ssmsExePath = "Ssms.exe";

                Console.WriteLine("Openning new instance of SSMS");
                string arguments = $"-S \"{dbServer}\" -d {dbName}";

                if (userName != null)
                    arguments += $" -U {userName}";
                else
                    arguments += $" -E";

                Process.Start(ssmsExePath, arguments);
            }
            else
            {
                Console.WriteLine("Connecting to SSMS");
                using (SsmsScriptDocumentActivatorProxy ssmsActivator = new SsmsScriptDocumentActivatorProxy())
                {
                    Console.WriteLine("Creating script document");
                    ssmsActivator.OpenDocument(dbServer, dbName, userName, password);
                }
            }
        }
    }
}
