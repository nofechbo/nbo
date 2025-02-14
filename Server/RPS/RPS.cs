using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Command;
using MyFactory;
using DataBase;

namespace MyRPS
{
    public class RPS : IRpsCommandHandler
    {
        private readonly Factory<string, Dictionary<string, string>, ICommand> _commandFactory;
        private readonly StringParser _parser = new StringParser();

        public RPS(DatabaseHandler dbHandler)
        {
            if (dbHandler == null) {
                throw new ArgumentNullException(nameof(dbHandler));
            }
            _commandFactory = new Factory<string, Dictionary<string, string>, ICommand>();

            //Inject `DatabaseHandler` into each command
            _commandFactory.Add("RegUpdate", args => new RegUpdate(args, dbHandler));
            _commandFactory.Add("SendMissiles", args => new SendMissiles(args, dbHandler));
            _commandFactory.Add("SendTechnician", args => new SendTechnician(args, dbHandler));
        }

        public async Task HandleRequestAsync(string input)
        {
            try
            {
                Dictionary<string, string> parsedArgs = await ParseInputAsync(input); // Parsing input
                ICommand command = await GetCommandAsync(parsedArgs); // Create command
                
                // Execute command if creation is successful
                command.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

        }

        private async Task<Dictionary<string, string>> ParseInputAsync(string input)
        {
            return await Task.FromResult(_parser.Parse(input));
        }

        private async Task<ICommand> GetCommandAsync(Dictionary<string, string> parsedArgs)
        {
            if (!parsedArgs.ContainsKey("command"))
            {
                throw new ArgumentException("Missing command in parsed input");
            }
            return await Task.FromResult(_commandFactory.Create(parsedArgs["command"], parsedArgs));
        }
    }
}
