using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Command;
using DataBase;
using Moq;
using NUnit.Framework;
using MyRPS;

namespace RpsSql.Tests
{
    [TestFixture]
    public class RPSTests
    {
        private RPS _rps;
        private Mock<DatabaseHandler> _mockDbHandler;

        [SetUp]
        public void Setup()
        {
            _mockDbHandler = new Mock<DatabaseHandler>(); // ✅ Mock DatabaseHandler
            _rps = new RPS(_mockDbHandler.Object); // ✅ Inject Mock DatabaseHandler
        }

        [Test]
        public async Task HandleRequestAsync_ValidRegUpdateCommand_ExecutesSuccessfully()
        {
            string input = "RegUpdate:12345,NewLocation";
            var command = await _rps.HandleRequestAsync(input);

            Assert.That(command, Is.Not.Null);
            Assert.That(command, Is.InstanceOf<RegUpdate>());
        }

        [Test]
        public async Task HandleRequestAsync_ValidSendMissilesCommand_ExecutesSuccessfully()
        {
            string input = "SendMissiles:12345,2";
            var command = await _rps.HandleRequestAsync(input);

            Assert.That(command, Is.Not.Null);
            Assert.That(command, Is.InstanceOf<SendMissiles>());
        }

        [Test]
        public async Task HandleRequestAsync_InvalidCommand_ThrowsArgumentException()
        {
            string input = "InvalidCommand:12345";
            Assert.ThrowsAsync<ArgumentException>(async () => await _rps.HandleRequestAsync(input));
        }
    }
}
