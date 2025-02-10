using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Command;
using MyFactory;


namespace mainServer
{
    public class RPS
    {
        private readonly Factory<string, Dictionary<string, string>, ICommand> _commandFactory;
        private readonly StringParser _parser;

        public RPS(Factory<string, Dictionary<string, string>, ICommand> factory, StringParser parser)
        {
            _commandFactory = factory;
            _parser = parser;
        }

        public async Task<ICommand> HandleRequestAsync(string input)
        {
            Dictionary<string, string> parsedArgs;
            try
            {
                parsedArgs = await ParseInputAsync(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine("invalid request");
                throw new ArgumentException("Parsing failed", ex);
            }

            return await GetCommandAsync(parsedArgs);
        }

        private Task<Dictionary<string, string>> ParseInputAsync(string input)
        {
            return Task.Run(() => _parser.Parse(input));
        }

        private Task<ICommand> GetCommandAsync(Dictionary<string, string> parsedArgs)
        {
            return Task.Run(() =>
            {
                if (!parsedArgs.ContainsKey("command"))
                {
                    throw new ArgumentException("Missing command in parsed input");
                }

                return _commandFactory.Create(parsedArgs["command"], parsedArgs);
            });
        }
    }
}
