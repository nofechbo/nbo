using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace mainServer
{
    public class TcpServer
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
    }
}