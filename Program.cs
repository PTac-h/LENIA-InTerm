using System.Runtime.InteropServices;
using System;

namespace LENIA4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            float gamma = 2f;
            float radius = 6.7f;
            int iterations = 1000;

            TerminalSetup.Setup();

            foreach (string arg in args)
            {
                if (arg.StartsWith("--gamma="))
                {
                    gamma = float.Parse(arg.Substring("--gamma=".Length).Replace('.',','));
                }
                if (arg.StartsWith("--radius="))
                {
                    radius = float.Parse(arg.Substring("--radius=".Length).Replace('.', ','));
                }
                if (arg.StartsWith("--iter="))
                {
                    iterations = int.Parse(arg.Substring("--iter=".Length).Replace('.', ','));
                }
            }

            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            Simulation simulation = new Simulation(width,height,iterations,radius,gamma);

            while (true)
            {
                simulation.Update();
            }
        }
    }
    public class TerminalSetup
    {

        // ReSharper disable InconsistentNaming

        private const int STD_INPUT_HANDLE = -10;

        private const int STD_OUTPUT_HANDLE = -11;

        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        private const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;

        // ReSharper restore InconsistentNaming

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        public static void Setup()
        {
            var iStdIn = GetStdHandle(STD_INPUT_HANDLE);
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

            if (!GetConsoleMode(iStdIn, out uint inConsoleMode))
            {
                Console.WriteLine("failed to get input console mode");
                Console.ReadKey();
                return;
            }
            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                Console.ReadKey();
                return;
            }

            inConsoleMode |= ENABLE_VIRTUAL_TERMINAL_INPUT;
            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;

            if (!SetConsoleMode(iStdIn, inConsoleMode))
            {
                Console.WriteLine($"failed to set input console mode, error code: {GetLastError()}");
                Console.ReadKey();
                return;
            }
            if (!SetConsoleMode(iStdOut, outConsoleMode))
            {
                Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                Console.ReadKey();
                return;
            }
        }
    }
}
