using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MyRPS;

namespace tcp
{
    public class TcpServer
    {
        private readonly RPS _rps;
        private readonly TcpListener _listener;
        private const int Port = 12345;

        public TcpServer(RPS rps)
        {
            _rps = rps;
            _listener = new TcpListener(IPAddress.Any, Port);
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine("Server is listening on port " + Port);

            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using var stream = client.GetStream();
            var buffer = new byte[256];
            bool exitRequested = false;

            while (!exitRequested)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) {
                    break;
                }

                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine($"Received request: {request}");

                try
                {
                    // Check if client requested to exit
                    if (request.ToLower() == "exit")
                    {
                        await SendExitMessage(stream, client);
                        exitRequested = true;
                    }
                    else {
                        string response = await _rps.HandleRequestAsync(request);
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions thrown by RPS and send error response
                    string errorResponse = $"Error: {ex.Message}";
                    byte[] errorBytes = Encoding.UTF8.GetBytes(errorResponse);
                    await stream.WriteAsync(errorBytes, 0, errorBytes.Length);
                }
            }
            
            client.Close();
        }

        private async Task SendExitMessage(NetworkStream stream, TcpClient client)
        {
            string exitMessage = "exit";
            byte[] exitBytes = Encoding.UTF8.GetBytes(exitMessage);
            await stream.WriteAsync(exitBytes, 0, exitBytes.Length);
            Console.WriteLine("Server sent 'exit'. Closing the connection.");
        }
    }
}