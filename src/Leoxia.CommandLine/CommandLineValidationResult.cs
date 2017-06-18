using System;

namespace Leoxia.CommandLine
{
    /// <summary>
    /// Result of command line validation
    /// </summary>
    public class CommandLineValidationResult
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="CommandLineValidationResult"/> class from being created.
        /// </summary>
        private CommandLineValidationResult()
        {
        }

        /// <summary>
        /// Returns true if the commmand line is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the related command line is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the error message to display is command line is not valid.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the function to invoke on valid command line.
        /// </summary>
        /// <value>
        /// The invoke.
        /// </value>
        public Func<int> OnValidCommandLine { get; set; }

        /// <summary>
        /// Gets the failure command line result.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public static CommandLineValidationResult GetFailure(string errorMessage)
        {
            return new CommandLineValidationResult
            {
                IsValid = false,
                ErrorMessage = errorMessage
            };
        }

        public static CommandLineValidationResult Invoke(Func<int> action)
        {
            return new CommandLineValidationResult
            {
                IsValid = true,
                OnValidCommandLine = action
            };
        }

        /// <summary>
        /// Get the singleton valid result that do nothing and returns 0.
        /// </summary>
        public static readonly CommandLineValidationResult Valid =
            new CommandLineValidationResult
            {
                IsValid = true,
                OnValidCommandLine = () => 0
            };
    }
}