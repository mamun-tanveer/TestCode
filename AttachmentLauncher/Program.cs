using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AttachmentLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string path = string.Empty;
                if (args.Length == 0)
                {
                    path = getInput();
                }
                else
                {
                    path = args[0];
                }

                string file = string.Empty;
                if (Path.GetExtension(path) == ".attach")
                {
                    var handler = new AttachmentHandler(path);
                    file = handler.GetAttachmentFile();
                }
                else file = path;

                System.Diagnostics.Process.Start(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed! Contact Support");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        static string getInput()
        {
            Console.WriteLine("Invalid Path provided. Input Path");
            string returnPath = Console.ReadLine();
            if (!File.Exists(returnPath)) returnPath = getInput();
            return returnPath;            
        }
    }
}
