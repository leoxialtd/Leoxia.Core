using System;
using System.Reflection;
using Leoxia.Abstractions.IO;
using Leoxia.Diagnostics;
using Leoxia.Scripting;
using Microsoft.Extensions.CommandLineUtils;

namespace Leoxia.CommandLine
{
    public abstract class BaseConsoleApplication : IConsoleApplication
    {
        private readonly IProfilingManager _profilingManager;
        protected CommandLineApplication _application;
        protected CommandOption _versionOption;
        protected CommandOption _verboseOption;
        protected CommandOption _helpOption;
        private Version _assemblyVersion;
        private readonly CommandOption _timingOption;
        protected abstract string ApplicationName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConsoleApplication" /> class.
        /// </summary>
        /// <param name="provider">The output and error writer provider.</param>
        /// <param name="profilingManager">The profiling manager.</param>
        protected BaseConsoleApplication(IStandardWriterProvider provider, IProfilingManager profilingManager)
        {
            _profilingManager = profilingManager;
            _application = new CommandLineApplication(throwOnUnexpectedArg: false);
            _application.Out = provider.Out;
            _application.Error = provider.Error;
            _timingOption = _application.Option("-t | --timing", "Time the current command", CommandOptionType.NoValue);
            _versionOption = _application.Option("-V | --version", "Show version information", CommandOptionType.NoValue);
            _verboseOption = _application.Option("-v | --verbose", "Increase debugging console output", CommandOptionType.NoValue);
            _helpOption = _application.HelpOption("-h | --help");
            _helpOption.Description = "Show help";
        }

        public void ShowCopyrights()
        {
            Console.WriteLine(Consts.Copyright);
        }

        public void RegisterCommand(IConsoleCommandHandler handler)
        {
            CommandHelper.RegisterCommandHandler(handler, _application);
        }

        public int Run(string[] arguments)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var assemblyName = entryAssembly.GetName();
            _assemblyVersion = assemblyName.Version;
            _application.FullName = $"Lx Command Line Tools ({GetShortVersion()})" 
                + Environment.NewLine 
                + Consts.Copyright;
            _application.Name = assemblyName.Name;
            Configure();
            _application.OnExecute((Func<int>)OnRunWithoutCommand);
            return _application.Execute(arguments);
        }

        private string GetLongVersion()
        {
            return _assemblyVersion.ToString();
        }

        private string GetShortVersion()
        {
            return $"{_assemblyVersion.Major}.{_assemblyVersion.Minor}.{_assemblyVersion.Build}";
        }

        protected virtual int OnRunWithoutCommand()
        {
            return new TimingCommand(_application, _timingOption, _profilingManager,
                () =>
                {
                    ScriptLogger.IsVerbose = _verboseOption.HasValue();
                    if (_versionOption.HasValue())
                    {
                        _application.Out.WriteLine(GetLongVersion());
                    }
                    else if (_helpOption.HasValue())
                    {
                        _application.ShowHelp();
                    }
                    else
                    {
                        _application.ShowRootCommandFullNameAndVersion();
                        _application.ShowHint();
                    }
                    return 0;
                }, _application.Out).Invoke();
        }

        protected abstract void Configure();
    }
}
