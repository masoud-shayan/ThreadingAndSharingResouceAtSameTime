using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizingResourceAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please wait for the tasks to complete.");
            Stopwatch watch = Stopwatch.StartNew();
            Task a = Task.Factory.StartNew(MethodA);
            Task b = Task.Factory.StartNew(MethodB);
            Task.WaitAll(new Task[] { a, b });
            Console.WriteLine();
            Console.WriteLine($"Results: {Message}.");
            Console.WriteLine($"{watch.ElapsedMilliseconds:#,##0} elapsed milliseconds.");
            
            
        }
        
        
        static Random r = new Random();
        static string Message =""; // a shared resource
        static object flag = new object();  // the flag to lock the resource at the second step

        
        static void MethodA()
        {
            
            // First Way without Locking the resource(Message) so both threads has access to it at same time and it might cause a problem
            // for (int i = 0; i < 5; i++)
            // {
            //     Thread.Sleep(r.Next(2000));
            //     Message += "A";
            //     Console.Write(".");
            // }

            
            
            // Second Way with Locking the resource(Message) so the threads has access to it one after another but it may cause a deadlock 
            // lock (flag)
            // {
            //     for (int i = 0; i < 5; i++)
            //     {
            //         Thread.Sleep(r.Next(2000));
            //         Message += "A";
            //         Console.Write(".");
            //     }
            // }
            
            
            
            // Third Way with Monitor(compiled from Lock after compiling process actually1) to avoid from deadlock and make sure the resource will be release after a few seconds(timeout)
            try
            {
                Monitor.TryEnter(flag , TimeSpan.FromSeconds(15));
                
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(r.Next(2000));
                    Message += "A";
                    Console.Write(".");
                }
            }
            finally
            {
                Monitor.Exit(flag);
            }
            
            
            
            
            // Fourth Way is used  when we want to share multiple resources between threads and we use an atomic type instead of object flag and uses interlocked
        }
        
        static void MethodB()
        {    
            // First Way without Locking the resource(Message) so both threads has access to it at same time and it might cause a problem
            // for (int i = 0; i < 5; i++)
            // {
            //     Thread.Sleep(r.Next(2000));
            //     Message += "B";
            //     Console.Write(".");
            // }

            
            
            
            
            // Second Way with Locking the resource(Message) so the threads has access to it one after another but it may cause a deadlock 
            // lock (flag)
            // {
            //     for (int i = 0; i < 5; i++)
            //     {
            //         Thread.Sleep(r.Next(2000));
            //         Message += "B";
            //         Console.Write(".");
            //     }
            // }




            // Third Way with Monitor(compiled from Lock  after compiling process actually!) to avoid from deadlock and make sure the resource will be release after a few seconds(timeout)
            try
            {
                Monitor.TryEnter(flag , TimeSpan.FromSeconds(15));
                
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(r.Next(2000));
                    Message += "B";
                    Console.Write(".");
                }
            }
            finally
            {
                Monitor.Exit(flag);
            }
            
            
            
            
            // Fourth Way is used  when we want to share multiple resources between threads and we use an atomic type instead of object flag and uses interlocked

            
        }
    }
}