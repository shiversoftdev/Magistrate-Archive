using System;

namespace LinuxTestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Core.Engine engine = new Engine.Core.Engine();
            Engine.Core.Scoring.StartEngine(engine);
            Console.WriteLine("Press any key to terminate the engine...");
            Console.ReadKey();
        }
    }
}
