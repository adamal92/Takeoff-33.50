using System.Threading;
using System.Diagnostics;

namespace TheThirdEye
{
    public class Needle
    {

        public void CurrentProccess()
        {
            //var thread = new Thread(() =>
            //{
            //    var currentProcess = Process.GetCurrentProcess();
            //    Console.WriteLine($"Current Process ID: {currentProcess.Id}");
            //    Console.WriteLine($"Current Process Name: {currentProcess.ProcessName}");
            //});

            var thread = Thread.CurrentThread;
            Console.WriteLine($"Current Thread ID: {thread.ManagedThreadId}");
            Console.WriteLine($"Current Thread Name: {thread.Name ?? "Unnamed"}");
            Console.WriteLine($"Is Background Thread: {thread.IsBackground}");
            Console.WriteLine($"Is Thread Pool Thread: {thread.IsThreadPoolThread}");
            Console.WriteLine($"Thread State: {thread.ThreadState}");
            Console.WriteLine($"Thread Priority: {thread.Priority}");

            var currentProcess = Process.GetCurrentProcess();
            Console.WriteLine($"Current Process ID: {currentProcess.Id}");
            Console.WriteLine($"Current Process Name: {currentProcess.ProcessName}");
            Console.WriteLine($"Total threads: {currentProcess.Threads.Count}");
            Console.WriteLine($"Total Processor Time: {currentProcess.TotalProcessorTime}");
            Console.WriteLine($"Current Process Main Module: {currentProcess.MainModule?.FileName}");
            Console.WriteLine($"Current Process Priority Class: {currentProcess.PriorityClass}");
            Console.WriteLine($"Current Process Start Time: {currentProcess.StartTime}");
        }

        public void StartSparkySparky()
        {
            var thread = new Thread(Run);
            thread.Start();
        }

        public void Run()
        {
            var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run --project ../SparkySparky/SparkySparky.csproj";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            // Wait for the application to start
            Thread.Sleep(5000);
            // Optionally, you can wait for the process to exit if needed
            // process.WaitForExit();
        }
    }
}
