using System;
using System.Threading.Tasks;
using MyRPS;  
using DataBase;  
using tcp;


// Initialize any dependencies (like DatabaseHandler, RPS, etc.)
var rps = new RPS(new DatabaseHandler());  
var server = new TcpServer(rps);

// Start the server asynchronously
await server.StartAsync();


