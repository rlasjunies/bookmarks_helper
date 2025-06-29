namespace EdgeBookmarksManager.Services;

public static class PortService
{
    public static int FindAvailablePort(int startPort = 5005, int maxTrials = 1000)
    {
        for (int port = startPort; port < startPort + maxTrials; port++)
        {
            try
            {
                using var listener = new TcpListener(IPAddress.Loopback, port);
                listener.Start();
                listener.Stop();
                return port;
            }
            catch (SocketException)
            {
                // Port is in use, try next one
                continue;
            }
        }
        throw new InvalidOperationException($"No available port found after {maxTrials} trials starting from {startPort}");
    }
}