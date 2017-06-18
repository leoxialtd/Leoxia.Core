using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace Leoxia.CommandLine
{
    public static class CommandLineApplicationExtensions
    {
        public static CommandOption GetAncestorOption(this CommandLineApplication command, string name)
        {
            if (command == null)
            {
                return null;
            }
            var option = command.Options.FirstOrDefault(o => o.LongName == name);
            if (option == null)
            {
                return GetAncestorOption(command.Parent, name);
            }
            return option;
        }
    }
}
