using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace ResponseCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            bool inputValid = false;
            string requestFolder = string.Empty;
            string baselineFolder = string.Empty;
            if (args.Length >= 2)
            {
                requestFolder = args[0];
                baselineFolder = args[1];

                inputValid = (validateFolder(requestFolder) && validateFolder(baselineFolder));
            } 
            
            if (!inputValid)
            {
                getInput(out requestFolder, out baselineFolder);
            }
        }

        static void getInput(out string requests, out string baselines)
        {
            Console.WriteLine("Input request folder");
            requests = Console.ReadLine();

            Console.WriteLine("Input baseline folder");
            baselines = Console.ReadLine();

            if (!validateFolder(requests) || !validateFolder(baselines))
            {
                getInput(out requests, out baselines);
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

        static void processRequests(string requests, string baselines)
        {
            Console.WriteLine("Starting Process");
            Parallel.ForEach<string>(Directory.GetFiles(requests), (currentFile) => { processRequestFile(currentFile, baselines); });
        }

        static void processRequestFile(string currentFile, string baselines)
        {
            var parse = new RequestFileParse(currentFile);
            var request = new RequestResponse(parse);
            request.MakeRequest();
        }
    }
}
