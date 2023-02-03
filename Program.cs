// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System.Diagnostics;
using System.Text;

namespace Injector
{
    public class Program
    {
        public static int Main(string[] args)
        {
#if DEBUG
            var dllpath = "C:\\Users\\adaml\\Documents\\Coding\\meta_speedhack\\DS2S META\\bin\\Debug\\net6.0-windows\\DS2S META 0.7.0.0\\Resources\\DLLs\\x86\\Speedhack.dll";
#else
            if (args.Length != 1)
                return -2;

            var dllpathraw = args[0];
            var dllpath = dllpathraw.Replace("\"", ""); // remove required wrapper quotes for IPC
#endif


            // checking inputs
            //using StreamWriter file = new("WriteLines2.txt");
            //file.WriteLine(dllpath);
            //file.Close();


            var Hook = new DS2Hook(50, 50)
            {
                DllPath = dllpath
            };
            Hook.Start();

            var pause = new TimeSpan(0, 0, 0, 0, 25); // ms between attempts
            var timeout = new TimeSpan(0, 0, 0, 3); // seconds timeout period
            var injected = RetryUntilSuccessOrTimeout(() => Hook.IsInjected, timeout, pause);

            // Not sure its a good idea to return this way
            if (injected)
                return (int)Hook.SpeedhackDllPtr;

            // Failure:
            //Console.WriteLine("Inject failed!");
            return -1;

        }

        public static bool RetryUntilSuccessOrTimeout(Func<bool> task, TimeSpan timeout, TimeSpan pause)
        {

            if (pause.TotalMilliseconds < 0)
            {
                throw new ArgumentException("pause must be >= 0 milliseconds");
            }
            var stopwatch = Stopwatch.StartNew();
            do
            {
                if (task()) { return true; }
                Thread.Sleep((int)pause.TotalMilliseconds);
            }
            while (stopwatch.Elapsed < timeout);
            return false;
        }

    }

}


