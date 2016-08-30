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

            string baseUrl = string.Empty;
            string baselineFolder = string.Empty;
            if (args.Length >= 2)
            {
                baseUrl = args[0];
                baselineFolder = args[1];

                inputValid = validateFolder(baselineFolder);
                
            } 
            
            if (!inputValid)
            {
                getConsoleInput(out baseUrl, out baselineFolder);
            }

            processRequests(new Comparer(), baseUrl, baselineFolder);
        }

        static void getConsoleInput(out string baseUrl, out string baselineFolder)
        {

            Console.WriteLine("Input Base URL, including http://");
            baseUrl = Console.ReadLine();

            Console.WriteLine("Input baseline folder");
            baselineFolder = Console.ReadLine();

            if (!validateFolder(baselineFolder))
            {
                getConsoleInput(out baseUrl, out baselineFolder);
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

        static void processRequests(Comparer engine, string baseUrl, string baselineFolder)
        {
            Console.WriteLine("Starting Process (Multithreaded)");
            var files = Directory.GetFiles(baselineFolder); 
            Parallel.ForEach<string>(files, (baselineFilePath) => { processRequest(engine, baseUrl, baselineFilePath); });
        }

        static void processRequest(Comparer engine, string baseUrl, string baselineFilePath)
        {
            var parse = new XmlFileParse(baselineFilePath);
            var baseline = new RequestResponse(baseUrl, parse);

            //empty the response for the test
            parse.Response = string.Empty;
            var test = new RequestResponse(baseUrl, parse);
            test.MakeRequest();

            //compare
            if (!engine.CompareVsBaseline(test, baseline))
            {
                Console.WriteLine("Different in " + test.ID);
            }
        }
    }
}
