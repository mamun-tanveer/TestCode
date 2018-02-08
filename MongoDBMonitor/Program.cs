using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MongoDBMonitor
{
    class Program
    {
        static IMongoDatabase db; 
        static IEnumerable<BsonDocument> mCommandList = new BsonDocument[] { new BsonDocument { { "serverStatus", 1 } },
                                                          new BsonDocument { { "currentOp", 1 } } };
        static int timeoutMs = 1000;
        static int periodMs = 10000;
    
        static void Main(string[] args)
        {

            try
            {
                db = getAdminDatabase("mongodb://localhost/");     
                var waiter = new System.Threading.AutoResetEvent(false);
                var timer = new System.Threading.Timer(runCommands, waiter, 0, periodMs);
                waiter.WaitOne();
                timer.Dispose();

            } catch (Exception ex)
            {
                Console.WriteLine("FAILURE: " + ex.Message);                
            }
            finally
            {               
                Console.WriteLine("Exiting");              
                if (args.Length == 0) { Console.ReadKey(); }
                Environment.Exit(4);
            }
        }

      
        private static void runCommands(object eventState)
        {
            var taskList = new List<Task<BsonDocument>>();
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            foreach(var command in mCommandList)
            {
                taskList.Add(runCommand(command));
            }
          
            if(Task.WaitAll(taskList.ToArray(), timeoutMs))
            {
                foreach(var result in taskList.Select<Task<BsonDocument>, BsonDocument>(x => x.Result))
                {
                    log(result, Console.OpenStandardOutput());
                }

                stopWatch.Stop();
                Console.WriteLine(mCommandList.Count() + "commands completed in " + stopWatch.ElapsedMilliseconds);
            }
            else
            {
                Console.WriteLine("Commands did not complete in the time provided");
                ((AutoResetEvent)eventState).Set();
            }
        }

        private static IMongoDatabase getAdminDatabase(string connectionString)
        {
            var client = new MongoClient(connectionString);
            return client.GetDatabase("admin");
        }

        private static async Task<BsonDocument> runCommand(BsonDocument commandDoc)
        {
            
            var command = new BsonDocumentCommand<BsonDocument>(commandDoc);
            var statusDoc = await db.RunCommandAsync<BsonDocument>(command, new ReadPreference(ReadPreferenceMode.Nearest));
            return statusDoc;
        }

        private static void log(BsonDocument resultDoc, Stream output)
        {
            using (var writer = new StreamWriter(output))
            {
                writer.WriteLine(resultDoc.ToJson());
            }

        }
    }
}
