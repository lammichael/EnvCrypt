using System;
using System.IO;

namespace EnvCrypt.Console.UnitTest.Helper
{
    /// <summary>
    /// Gets the string written to the console's standard out stream.
    /// </summary>
    class ConsoleScraper
    {
        private readonly Action _action;

        public ConsoleScraper(Action action)
        {
            _action = action;
        }


        public string Run()
        {
            string consoleOutput;
            var originalConsoleOut = System.Console.Out; // preserve the original stream
            using (var writer = new StringWriter())
            {
                System.Console.SetOut(writer);
                _action.Invoke();
                writer.Flush(); // when you're done, make sure everything is written out

                consoleOutput = writer.GetStringBuilder().ToString();
            }
            System.Console.SetOut(originalConsoleOut); // restore Console.Out
            return consoleOutput;
        }
    }
}
