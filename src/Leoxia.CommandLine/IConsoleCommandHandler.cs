using System.IO;
using Microsoft.Extensions.CommandLineUtils;

namespace Leoxia.CommandLine
{
    /// <summary>
    /// Interface for command handler
    /// </summary>
    public interface IConsoleCommandHandler
    {
        /// <summary>
        /// Configure the command.
        /// </summary>
        /// <param name="command">The command.</param>
        void OnCommand(CommandLineApplication command);

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        /// <value>
        /// The name of the command.
        /// </value>
        string CommandName { get; }

        string Description { get; }

        /// <summary>
        /// Call validation for parent command
        /// </summary>
        /// <param name="isCurrentCommandValidation">if set to <c>true</c> the validation is done locally.</param>
        /// <returns></returns>
        CommandLineValidationResult ParentCommandValidate(bool isCurrentCommandValidation);
    }
}