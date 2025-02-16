using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tcp;
using System.Net.Sockets;
using System.Text;

namespace LauncherManagement
{
     public class LauncherListener
    {
        private readonly List<Launcher> _launchers = new();
        private const string ServerAddress = "localhost";
        private const int ServerPort = 12345;

        public void RegisterLauncher(Launcher launcher)
        {
            _launchers.Add(launcher);
            launcher.MalfunctionOccurred += HandleMalfunction;
            Console.WriteLine($"📝 Registered launcher {launcher.Code} for malfunction monitoring");
        }

        private void HandleMalfunction(string launcherCode)
        {
            Console.WriteLine($"⚡ Malfunction event received for launcher {launcherCode}");
            Task.Run(async () => await HandleMalfunctionAsync(launcherCode))
                .ContinueWith(t => 
                {
                    if (t.IsFaulted)
                    {
                        Console.WriteLine($"❌ Error handling malfunction: {t.Exception}");
                    }
                });
        }

        private async Task HandleMalfunctionAsync(string launcherCode)
        {
            string command = $"SendTechnician:{launcherCode},Error579";
            string response = await SendTechnicianRequest(command);
            Console.WriteLine($"📨 Server response: {response}");
        }

        private async Task<string> SendTechnicianRequest(string command)
        {
            try
            {
                using TcpClient client = new TcpClient();
                await client.ConnectAsync(ServerAddress, ServerPort);

                using NetworkStream stream = client.GetStream();

                // Send command
                byte[] commandBytes = Encoding.UTF8.GetBytes(command);
                await stream.WriteAsync(commandBytes, 0, commandBytes.Length);
                await stream.FlushAsync();

                // Read response with timeout
                byte[] responseBuffer = new byte[256];
                client.ReceiveTimeout = 5000; // 5 second timeout
                int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
                
                if (bytesRead == 0)
                {
                    return "Error: No response from server";
                }

                string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead).Trim();
                return response;
            }
            catch (Exception ex) when (ex is SocketException || ex is Exception)
            {
                return $"❌ Error: {ex.Message}";
            }
        }
    }
}



/*using System;
using System.Collections.Generic;
using MyRPS;

namespace LauncherManagement
{
    public class LauncherListener
    {
        private readonly IRpsCommandHandler _rps;
        public event Action<string>? OnMalfunctionDetected;
        private readonly List<Launcher> _launchers = new();

        public LauncherListener(IRpsCommandHandler rps)
        {
            _rps = rps;
        }

        public void RegisterLauncher(Launcher launcher)
        {
            _launchers.Add(launcher);
            launcher.MalfunctionOccurred += HandleMalfunction;
        }

        private void HandleMalfunction(string launcherCode)
        {
            Task.Run(async () => await HandleMalfunctionAsync(launcherCode));
        }

        private async Task HandleMalfunctionAsync(string launcherCode)
        {
            Console.WriteLine($"🚨 Malfunction detected on launcher {launcherCode}. Sending technician request...");
            string command = $"SendTechnician:{launcherCode}, Error579";
            string response = await _rps.HandleRequestAsync(command);
            // Fire event for additional listeners
            OnMalfunctionDetected?.Invoke(command);
            Console.WriteLine(response);
        }
    }
}*/
