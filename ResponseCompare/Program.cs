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
            string regexFilePath = string.Empty;
            if (args.Length >= 2)
            {
                requestFolder = args[0];
                baselineFolder = args[1];
              
                inputValid = (validateFolder(requestFolder) && validateFolder(baselineFolder));

                if (inputValid && args.Length > 2)
                {
                    regexFilePath = args[3];
                    if(!File.Exists(regexFilePath))
                    {
                        Console.WriteLine(regexFilePath + " invalid. Proceeding without regExs");
                        regexFilePath = string.Empty;
                    }
                }

            } 
            
            if (!inputValid)
            {
                getConsoleInput(out requestFolder, out baselineFolder, out regexFilePath);
            }

            processRequests(initComparer(regexFilePath), requestFolder, baselineFolder);
        }

        static void getConsoleInput(out string requests, out string baselines, out string regExs)
        {
            Console.WriteLine("Input request folder");
            requests = Console.ReadLine();

            Console.WriteLine("Input baseline folder");
            baselines = Console.ReadLine();

            if (!validateFolder(requests) || !validateFolder(baselines))
            {
                getConsoleInput(out requests, out baselines, out regExs);
            }

            Console.WriteLine("Input RegEx file path");
            regExs = Console.ReadLine();

            if (!File.Exists(regExs))
            {
                Console.WriteLine(regExs + " invalid. Proceeding without regExs");
                regExs = string.Empty;
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

        static Comparer initComparer(string regexFilePath)
        {
            return new Comparer(new RegexCache(regexFilePath));
        }

        static void processRequests(Comparer engine, string requests, string baselines)
        {
            Console.WriteLine("Starting Process (Multithreaded)");
            Parallel.ForEach<string>(Directory.GetFiles(requests), (currentFile) => { processRequestFile(engine, currentFile, baselines); });
        }

        static void processRequestFile(Comparer engine, string currentFile, string baselines)
        {
            var parse = new RequestFileParse(currentFile);
            var request = new RequestResponse(parse);
            request.MakeRequest();
            if (!engine.CompareVsBaseline(request, baselines))
            {
                Console.WriteLine("Different in " + currentFile);
            }
        }
    }
}
