using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Command;
using MyFactory;
using Moq;
using NUnit.Framework;

namespace mainServer.Tests
{
    [TestFixture]
    public class RPSTests
    {
        private RPS _rps;
        private Mock<StringParser> _mockParser;
        private Mock<Factory<string, Dictionary<string, string>, ICommand>> _mockFactory;

        [SetUp]
        public void Setup()
        {
            _mockParser = new Mock<StringParser>();
            _mockFactory = new Mock<Factory<string, Dictionary<string, string>, ICommand>> { CallBase = true };

            _rps = new RPS(_mockFactory.Object, _mockParser.Object);
        }

        [Test]
        public async Task HandleRequestAsync_ValidRegUpdateCommand_ExecutesSuccessfully()
        {
            string input = "RegUpdate:12345,needMaintenance";
            var parsedArgs = new Dictionary<string, string>
            {
                { "command", "RegUpdate" },
                { "launcherID", "12345" },
                { "info", "needMaintenance" }
            };
            var mockCommand = new Mock<ICommand>();

            _mockParser.Setup(p => p.Parse(input)).Returns(parsedArgs);
            _mockFactory.Setup(f => f.Create("RegUpdate", parsedArgs)).Returns(mockCommand.Object);

            ICommand command = await _rps.HandleRequestAsync(input);

            Assert.That(command, Is.Not.Null);
            Assert.That(command, Is.InstanceOf<ICommand>());
        }
        [Test]
        public async Task HandleRequestAsync_ValidSendTechnicianCommand_ExecutesSuccessfully()
        {
            string input = "SendTechnician:Patriot,BaseBravo";
            var parsedArgs = new Dictionary<string, string>
            {
                { "command", "SendTechnician" },
                { "launcherID", "Patriot" },
                { "info", "BaseBravo" }
            };

            var realCommand = new SendTechnician(parsedArgs); // ✅ Return the real `SendTechnician` instance

            _mockParser.Setup(p => p.Parse(input)).Returns(parsedArgs);
            _mockFactory.Setup(f => f.Create("SendTechnician", parsedArgs)).Returns(realCommand);  // ✅ Now returns a real object

            ICommand command = await _rps.HandleRequestAsync(input);

            Assert.That(command, Is.Not.Null);
            Assert.That(command, Is.InstanceOf<SendTechnician>()); // ✅ This now works
        }


        [Test]
        public async Task HandleRequestAsync_InvalidCommand_ThrowsArgumentException()
        {
            string input = "InvalidCommand:12345,something";
            _mockParser.Setup(p => p.Parse(input)).Throws(new InvalidCommandException("Invalid command"));

            Assert.ThrowsAsync<ArgumentException>(async () => await _rps.HandleRequestAsync(input));
        }

        [Test]
        public async Task HandleRequestAsync_InvalidFormat_ThrowsArgumentException()
        {
            string input = "RegUpdate:onlyOneArgument";

            _mockParser!.Setup(p => p.Parse(input)).Throws(new InvalidCommandException("Invalid command format"));

            // ✅ Correctly expects `ArgumentException`, since `HandleRequestAsync()` wraps errors
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _rps.HandleRequestAsync(input));
            
            // ✅ Ensure the inner exception is `InvalidCommandException`
            Assert.That(exception.InnerException, Is.TypeOf<InvalidCommandException>());
        }

    }
}
