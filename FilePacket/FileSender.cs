using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FilePacket
{
    public class FileSender
    {
        private const int PACKET_SIZE = 8192; 
        private UdpClient client;
        public event Action<int, int> OnProgress; 
        public event Action<string, bool> OnComplete; 
        public FileSender()
        {
            client = new UdpClient();
        }
        public async Task<bool> SendFile(string filePath, IPAddress targetIp, int port)
        {
            try
            {
                byte[] fileData = await File.ReadAllBytesAsync(filePath);
                string fileName = Path.GetFileName(filePath);
                string packetId = Guid.NewGuid().ToString();
                int totalPackets = (int)Math.Ceiling((double)fileData.Length / PACKET_SIZE);

                for (int i = 0; i < totalPackets; i++)
                {
                    int offset = i * PACKET_SIZE;
                    int length = Math.Min(PACKET_SIZE, fileData.Length - offset);
                    byte[] packetData = new byte[length];
                    Array.Copy(fileData, offset, packetData, 0, length);
                    var packet = new FilePacket
                    {
                        PacketNumber = i,
                        TotalPackets = totalPackets,
                        Data = packetData,
                        FileName = fileName,
                        PacketId = packetId
                    };
                    byte[] serializedPacket = FilePacket.Serialize(packet);
                    await client.SendAsync(serializedPacket, serializedPacket.Length,   new IPEndPoint(targetIp, port));

                    await Task.Delay(1);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки: {ex.Message}");
                return false;
            }
        }
    }
}
