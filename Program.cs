// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System.Diagnostics;
using System.Text;

namespace Injector
{
    public class Program
    {
        private const int Pause_ms = 25;
        private const int Timeout_s = 3;
        public static int Main(string[] args)
        {
            if (args.Length != 1)
                return -2;

            var dllpathraw = args[0];
            var dllpath = dllpathraw.Replace("\"", ""); // remove required wrapper quotes for IPC

            var Hook = new DS2Hook(50, 50)
            {
                DllPath = dllpath
            };
            Hook.Start();

            
            var pause = new TimeSpan(0, 0, 0, 0, Pause_ms); // ms between attempts
            var timeout = new TimeSpan(0, 0, 0, Timeout_s); // seconds timeout period
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


