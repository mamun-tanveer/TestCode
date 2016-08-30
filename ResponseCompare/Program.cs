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
            string[] requestIds = { };
            string baselineFolder = string.Empty;
            string regexFilePath = string.Empty;
            if (args.Length >= 1)
            {
                baselineFolder = args[0];
                inputValid = validateFolder(baselineFolder);
                
                if (inputValid && args.Length > 1)
                {
                    regexFilePath = args[1];
                    if(!File.Exists(regexFilePath))
                    {
                        Console.WriteLine(regexFilePath + " invalid. Proceeding without regExs");
                        regexFilePath = string.Empty;
                    }

                    //TODO: take request ids as input
                }

            } 
            
            if (!inputValid)
            {
                getConsoleInput(out baselineFolder, out regexFilePath);
            }

            processRequests(initComparer(regexFilePath), requestIds, baselineFolder);
        }

        static void getConsoleInput(out string baselines, out string regExs)
        {

            Console.WriteLine("Input baseline folder");
            baselines = Console.ReadLine();

            if (!validateFolder(baselines))
            {
                getConsoleInput(out baselines, out regExs);
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

        static void processRequests(Comparer engine, IEnumerable<string> requestFileNames, string baselineFolder)
        {
            Console.WriteLine("Starting Process (Multithreaded)");
            if(requestFileNames.Count() == 0 ) { requestFileNames = Directory.GetFiles(baselineFolder); }
            Parallel.ForEach<string>(requestFileNames, (requestFileName) => { processRequest(engine, requestFileName, baselineFolder); });
        }

        static void processRequest(Comparer engine, string requestId, string baselineFolderPath)
        {
            string baselineFilePath = Path.Combine(baselineFolderPath, requestId);
            var parse = new XmlFileParse(baselineFilePath);
            var request = new RequestResponse(parse);
            request.MakeRequest();
            if (!engine.CompareVsBaseline(request, baselineFilePath))
            {
                Console.WriteLine("Different in " + requestId);
            }
        }
    }
}
