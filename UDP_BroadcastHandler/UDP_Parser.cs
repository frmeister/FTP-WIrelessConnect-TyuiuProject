using System.Net;
using System.Net.Sockets;

namespace UDP_BroadcastHandler
{
    public class UDP_Parser
    {
        public static List<IPAddress> GetLocalIPv4Addresses()
        {
            var addresses = new List<IPAddress>();
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) 
                {
                    addresses.Add(ip);
                }
            }
            return addresses;
        }
    }
}
