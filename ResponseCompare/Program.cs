using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponseCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            bool inputValid = false;
            string folder1 = string.Empty;
            string folder2 = string.Empty;
            if (args.Length >= 2)
            {
                folder1 = args[0];
                folder2 = args[1];

                inputValid = (validateFolder(folder1) && validateFolder(folder2));
            } 
            
            if (!inputValid)
            {
                getInput(out folder1, out folder2);
            }
        }

        static void getInput(out string x, out string y)
        {
            Console.WriteLine("Input first folder");
            x = Console.ReadLine();

            Console.WriteLine("Input second folder");
            y = Console.ReadLine();

            if (!validateFolder(x) || !validateFolder(y))
            {
                getInput(out x, out y);
            }       
        }

        static bool validateFolder(string path)
        {

            bool returnValue = System.IO.Directory.Exists(path);
            if (returnValue == false)
            {
                Console.WriteLine(path + " doesn't exist");
            }
            return returnValue;
        }

        
                
    }
}
