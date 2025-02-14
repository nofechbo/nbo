using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tcp
{
    class SimpleTcpServer
    {
        static void Main()
        {
            int port = 12345;
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server is listening on port " + port);

            while (true)
            {
                // Accept incoming client connections
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");

                // Get the network stream
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[256];
                int bytesRead;

                // Keep the connection open for multiple messages
                while ((bytesRead = stream.Read(data, 0, data.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(data, 0, bytesRead);
                    Console.WriteLine("Received: " + message);

                    // If the client sends "exit", break the loop to end the communication
                    if (message.ToLower() == "exit")
                    {
                        Console.WriteLine("Client requested to close the connection. Sending 'exit' to client.");
                        break;
                    }

                    // Echo the received message back to the client
                    stream.Write(data, 0, bytesRead);
                }

                // After finishing communication, send "exit" to the client to close the connection
                string exitMessage = "exit";
                byte[] exitData = Encoding.ASCII.GetBytes(exitMessage);
                stream.Write(exitData, 0, exitData.Length);
                Console.WriteLine("Sent 'exit' to the client");

                // Close the client connection
                client.Close();
                Console.WriteLine("Client disconnected");
            }
        }
    }

    
    
    /*public class TcpServer
    {
        private readonly RPS _rps;
        private readonly TcpListener _listener;

        public TcpServer(int port, RPS rps)
        {
            _rps = rps;
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine("TCP Server started...");

            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead == 0) return;

            string requestString = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
            Console.WriteLine($"Received: {requestString}");

            string response = await _rps.ProcessRequestAsync(requestString);
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);

            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            Console.WriteLine($"Sent: {response}");
        }
    }*/
}