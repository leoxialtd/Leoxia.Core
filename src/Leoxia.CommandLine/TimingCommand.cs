using System;
using System.Diagnostics;
using System.IO;
using Leoxia.Diagnostics;
using Microsoft.Extensions.CommandLineUtils;

namespace Leoxia.CommandLine
{
    public class TimingCommand
    {
        private readonly CommandLineApplication _command;
        private readonly CommandOption _timingOption;
        private readonly IProfilingManager _profilingManager;
        private readonly Func<int> _function;
        private readonly TextWriter _output;
        private readonly Stopwatch _stopWatch;

        public TimingCommand(CommandLineApplication command, CommandOption timingOption, 
            IProfilingManager profilingManager,
            Func<int> function, TextWriter output)
        {
            _command = command;
            _timingOption = timingOption;
            _profilingManager = profilingManager;
            _function = function;
            _output = output;
            _stopWatch = new Stopwatch();
        }

        public int Invoke()
        {
            if (_timingOption != null && _timingOption.HasValue())
            {
                _stopWatch.Start();
            }

            var res = _function();
            if (_stopWatch.IsRunning)
            {
                _stopWatch.Stop();
                var summary = _profilingManager.GetDetailedSummary().ToString();
                if (!string.IsNullOrEmpty(summary))
                {
                    _output.WriteLine(summary);
                }
                _output.WriteLine("Elapsed time: " + _stopWatch.Elapsed);
            }
            return res;
        }
    }
}