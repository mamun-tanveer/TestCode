using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

namespace TaskTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int timeoutMs = 0; 
            try
            {
                Console.WriteLine("Input timeout milliseconds");
                int.TryParse(Console.ReadLine(), out timeoutMs);              
                Task.Delay(1000).TimeoutAfter(timeoutMs).Wait();
                Console.WriteLine("Task completed");
            }
            catch (AggregateException ex)
            {
                Console.Write(ex.InnerException.Message);
                Console.WriteLine(timeoutMs);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }        
    }
}
