using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileFinder
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string inputFolderPath = string.Empty;
            string outputFilePath = string.Empty;
            getInputArgs(args, out inputFolderPath, out outputFilePath);

            var finder = new FolderFind(inputFolderPath);
            writeOutputFile(outputFilePath, finder.FileLocations);            
        }

        static void getInputArgs(string[] args, out string inputFolderPath, out string outputFilePath)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Input Folder Path");
                inputFolderPath = Console.ReadLine();
                Console.WriteLine("Output File Path");
                outputFilePath = Console.ReadLine();
            }
            else if (args.Length == 1)
            {
                inputFolderPath = args[0];
                Console.WriteLine("Output File Path");
                outputFilePath = Console.ReadLine();
            }
            else
            {
                inputFolderPath = args[0];
                outputFilePath = args[1];
            }

            if(Directory.Exists(inputFolderPath) == false)
            {
                throw new DirectoryNotFoundException(inputFolderPath + "does not exist");
            }
        }

        static void writeOutputFile(string outputFilePath, IDictionary<string, List<string>> locationMap)
        {
            using (var writer = new StreamWriter(outputFilePath))
            {
                foreach(string filename in locationMap.Keys)
                {
                    foreach(string location in locationMap[filename])
                    {
                        writer.WriteLine(filename + "\t" + location);
                    }
                }
            }
        }

    }
}
