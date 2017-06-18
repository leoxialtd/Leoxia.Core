using System;
using Leoxia.Log;
using Leoxia.Log.IO;
using Leoxia.Scripting;

namespace Leoxia.CommandLine
{
    /// <summary>
    /// Run a console application and catch error codes and exceptions.
    /// </summary>
    public class ConsoleApplicationRunner
    {
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(ConsoleApplicationRunner));

        public static int Run(Func<string[], int> main, string[] args)
        {
            LogManager.AppenderMediator.Subscribe(new ColoredConsoleAppender());
            int exitCode = 1;
            try
            {
                exitCode = main(args);
            }
            catch (Exception exception)
            {
                _logger.ErrorFormat("Exception killing process: {0}", exception.ToString());
            }
            ScriptLogger.WaitOnDebug();
            return exitCode;
        }

        public static int Run(IConsoleApplication application, string[] args)
        {
            return Run(application.Run, args);
        }
    }
}