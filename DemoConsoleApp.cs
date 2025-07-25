using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Main Program Execution Analysis ===\n");
            
            // 1. Current Process Information
            var currentProcess = Process.GetCurrentProcess();
            Console.WriteLine($"1. APPLICATION PROCESS:");
            Console.WriteLine($"   Process Name: {currentProcess.ProcessName}");
            Console.WriteLine($"   Process ID: {currentProcess.Id}");
            Console.WriteLine($"   Start Time: {currentProcess.StartTime}");
            Console.WriteLine($"   Working Set: {currentProcess.WorkingSet64 / 1024 / 1024} MB");
            
            // 2. Runtime Information
            Console.WriteLine($"\n2. .NET RUNTIME:");
            Console.WriteLine($"   Framework: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
            Console.WriteLine($"   Runtime Identifier: {System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier}");
            Console.WriteLine($"   Process Architecture: {System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture}");
            Console.WriteLine($"   OS Architecture: {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture}");
            
            // 3. Assembly Information
            Console.WriteLine($"\n3. ASSEMBLY LOADING:");
            var assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine($"   Executing Assembly: {assembly.FullName}");
            Console.WriteLine($"   Location: {assembly.Location}");
            Console.WriteLine($"   GAC: {assembly.GlobalAssemblyCache}");
            
            // 4. Memory and GC Information
            Console.WriteLine($"\n4. MEMORY MANAGEMENT:");
            Console.WriteLine($"   Total Memory: {GC.GetTotalMemory(false) / 1024} KB");
            Console.WriteLine($"   Gen 0 Collections: {GC.CollectionCount(0)}");
            Console.WriteLine($"   Gen 1 Collections: {GC.CollectionCount(1)}");
            Console.WriteLine($"   Gen 2 Collections: {GC.CollectionCount(2)}");
            
            // 5. Thread Information
            Console.WriteLine($"\n5. THREADING:");
            Console.WriteLine($"   Current Thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"   Is Background: {System.Threading.Thread.CurrentThread.IsBackground}");
            Console.WriteLine($"   Thread Pool Threads: {System.Threading.ThreadPool.ThreadCount}");
            
            // 6. JIT Information
            Console.WriteLine($"\n6. JIT COMPILATION:");
            Console.WriteLine($"   Server GC: {GCSettings.IsServerGC}");
            Console.WriteLine($"   Latency Mode: {GCSettings.LatencyMode}");
            
            // 7. Related Processes (simplified)
            Console.WriteLine($"\n7. RELATED PROCESSES:");
            var processes = Process.GetProcesses()
                .Where(p => p.ProcessName.ToLower().Contains("dotnet") || 
                           p.ProcessName.ToLower().Contains(currentProcess.ProcessName.ToLower()))
                .Take(5)
                .ToList();
                
            foreach (var proc in processes)
            {
                try
                {
                    Console.WriteLine($"   {proc.ProcessName} (PID: {proc.Id})");
                }
                catch
                {
                    // Process might have exited
                }
            }
            
            Console.WriteLine($"\n8. EXECUTION:");
            Console.WriteLine($"   Your Main() method is now running!");
            Console.WriteLine($"   All the above infrastructure was set up to get here.");
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
} 