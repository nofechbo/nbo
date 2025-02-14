using System;
using System.Net.Sockets;
using System.Text;

class SimpleTcpClient
{
    static void Main()
    {
        string serverIp = "127.0.0.1"; // Localhost
        int port = 12345;

        try
        {
            // Connect to the server
            TcpClient client = new TcpClient(serverIp, port);
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Connected to the server. Type 'exit' to end the communication.");

            // Continuously send messages to the server
            while (true)
            {
                Console.Write("You: ");
                string message = Console.ReadLine();

                // Send message to the server
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // Receive and display the echo from the server
                data = new byte[256];
                int bytesRead = stream.Read(data, 0, data.Length);
                string serverMessage = Encoding.ASCII.GetString(data, 0, bytesRead);
                Console.WriteLine("Server: " + serverMessage);

                // If server sends 'exit', break the loop and close the connection
                if (serverMessage.ToLower() == "exit")
                {
                    Console.WriteLine("Server sent 'exit'. Closing the connection.");
                    break;
                }
            }

            // Close the client connection
            client.Close();
            Console.WriteLine("Connection closed");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }
}
