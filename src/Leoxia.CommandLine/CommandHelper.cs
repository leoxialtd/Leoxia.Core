using Microsoft.Extensions.CommandLineUtils;

namespace Leoxia.CommandLine
{
    public static class CommandHelper
    {
        public static void RegisterCommandHandler(IConsoleCommandHandler handler, CommandLineApplication mainCommand)
        {            
            var subCommand = mainCommand.Command(handler.CommandName, x => { }, false);
            subCommand.FullName = mainCommand.FullName;
            subCommand.Description = handler.Description; 
            // For correct cascading of Textwriter, 
            // it is important to keep this between instantiation and invocation
            subCommand.Out = mainCommand.Out;
            subCommand.Error = mainCommand.Error;
            handler.OnCommand(subCommand);
        }
    }
}