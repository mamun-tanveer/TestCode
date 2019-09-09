using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QueryLoop
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string filePath = string.Empty;
                int maxThreads = 0;
                if (args.Length == 1) filePath = args[0];
                else if (args.Length > 1) int.TryParse(args[1], out maxThreads);
                validateInput(ref filePath, ref maxThreads);
                using (var writer = new StreamWriter("output.txt"))
                {
                    Task worker = processLoop(filePath, writer, maxThreads);
                    worker.Wait();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed: " + ex.Message);
                ErrorLogger.Log(ex);
            }
            finally
            {
                if(args.Length == 0)
                {
                    //interactive mode 
                    Console.WriteLine("Done");
                    Console.ReadKey();
                }
            }
        }

        private static void validateInput(ref string filePath, ref int maxThreads)
        {
            if(string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
            {
                Console.WriteLine("Invalid input file path. Please enter a filepath");
                filePath = Console.ReadLine();
                validateInput(ref filePath, ref maxThreads);
            }

            if (maxThreads <= 0 || maxThreads >= 10)
            {
                maxThreads = 10; 
            }
        }

        private async static Task processLoop(string inputFilePath, StreamWriter output, int maxThreads)
        {
            using (var reader = new StreamReader(inputFilePath))
            {
                string connectionString = reader.ReadLine();
                string databaseName = reader.ReadLine();
                string collectionName = reader.ReadLine();
                string line = reader.ReadLine();
                string[] lineParts = line.Split(';');
                string queryJson = lineParts[0];
                string projectionJson = "{}";

                if (lineParts.Length > 1) projectionJson = lineParts[1];

                var db = new MongoDB(connectionString, databaseName, collectionName);

                do
                {
                    line = await reader.ReadLineAsync();
                    queryJson.Replace("~", line);
                    var results = await db.DoQuery(queryJson, projectionJson);
                    writeResult(results, output);
                } while (!string.IsNullOrEmpty(line) && !reader.EndOfStream);
            }
        }

        private static void writeResult(IEnumerable<Result> results, StreamWriter output)
        {
            foreach(var result in results)
            {
                output.WriteLine(string.Join("\t", result.Values));
            }
        }
    }
}
