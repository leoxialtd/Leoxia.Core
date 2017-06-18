using System;
using Leoxia.Diagnostics;
using Microsoft.Extensions.CommandLineUtils;


namespace Leoxia.CommandLine
{
    /// <summary>
    /// Base abstract class for handling command.
    /// </summary>
    /// <seealso cref="Leoxia.CommandLine.IConsoleCommandHandler" />
    public abstract class BaseConsoleCommandHandler : IConsoleCommandHandler
    {
        private readonly IConsoleCommandHandler _parentHandler;
        private readonly IProfilingManager _profilingManager;
        protected CommandLineApplication _command;


        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConsoleCommandHandler" /> class.
        /// </summary>
        /// <param name="parentHandler">The parent handler from which the handler was created.</param>
        /// <param name="profilingManager">The profiling manager.</param>
        protected BaseConsoleCommandHandler(IConsoleCommandHandler parentHandler, IProfilingManager profilingManager)
        {
            _parentHandler = parentHandler;
            _profilingManager = profilingManager;
        }

        /// <summary>
        /// Configure the newly created command and bind it to current handler.
        /// </summary>
        /// <param name="command">The command.</param>
        public void OnCommand(CommandLineApplication command)
        {
            _command = command;
            _command.Name = CommandName;            
            command.HelpOption("-h | --help");
            command.OnExecute((Func<int>)OnExecute);
            Configure();
        }

        protected abstract void Configure();

        public void RegisterCommand(IConsoleCommandHandler handler)
        {
            CommandHelper.RegisterCommandHandler(handler, _command);
        }

        /// <summary>
        /// Called when command is executed. 
        /// Validate command and then execute the relevant callback.
        /// </summary>
        /// <returns></returns>
        protected int OnExecute()
        {
            var timingOption = _command.GetAncestorOption("timing");
            return new TimingCommand(_command, timingOption, _profilingManager,
                    () =>
                    {
                        if (_command.OptionHelp.HasValue())
                        {
                            return 0;
                        }
                        var commandlineValidationResult = ParentCommandValidate(true);
                        if (commandlineValidationResult.IsValid)
                        {
                            return commandlineValidationResult.OnValidCommandLine();
                        }
                        _command.Error.WriteLine(commandlineValidationResult.ErrorMessage);
                        _command.ShowHint();
                        return 1;
                    }, _command.Out)
                .Invoke();
        }

        public abstract string Description { get; }

        /// <summary>
        /// Call validation for the chain of parents and for the current command.
        /// </summary>
        /// <param name="isCurrentCommandValidation">if set to <c>true</c> the validation has called for the current command.
        /// Otherwise it means it is called from a child command validation process.</param>
        /// <returns></returns>
        public CommandLineValidationResult ParentCommandValidate(bool isCurrentCommandValidation)
        {
            CommandLineValidationResult ancestorResult = null;
            if (_parentHandler != null)
            {
                ancestorResult = _parentHandler.ParentCommandValidate(false);
            }
            if (ancestorResult == null || ancestorResult.IsValid)
            {
                return Validate(isCurrentCommandValidation);
            }
            return ancestorResult;
        }

        /// <summary>
        /// Entry point for specific implementation validation code.
        /// </summary>
        /// <param name="isCurrentCommandValidation">if set to <c>true</c> the validation is done for the current command. 
        /// Otherwise it means it is called from a child command validation process.</param>
        /// <returns></returns>
        protected abstract CommandLineValidationResult Validate(bool isCurrentCommandValidation);

        public abstract string CommandName { get; }
    }
}