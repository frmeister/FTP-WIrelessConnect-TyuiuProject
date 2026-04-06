using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace FilePacket
{
    public class FilePacket
    {
        public int PacketNumber { get; set; }
        public int TotalPackets { get; set; }
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string PacketId { get; set; }

        public static byte[] Serialize(FilePacket packet)
        {
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8);

            writer.Write(packet.PacketNumber);
            writer.Write(packet.TotalPackets);
            writer.Write(packet.FileName ?? "");
            writer.Write(packet.PacketId ?? "");

            int dataLength = packet.Data?.Length ?? 0;
            writer.Write(dataLength);
            if (dataLength > 0)
                writer.Write(packet.Data);
            return ms.ToArray();
        }
        public static FilePacket Deserialize(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var reader = new BinaryReader(ms, Encoding.UTF8);
            var packet = new FilePacket
            {
                PacketNumber = reader.ReadInt32(),
                TotalPackets = reader.ReadInt32(),
                FileName = reader.ReadString(),
                PacketId = reader.ReadString()
            };

            int dataLength = reader.ReadInt32();
            if (dataLength > 0)
                packet.Data = reader.ReadBytes(dataLength);
            else
                packet.Data = Array.Empty<byte>();
            return packet;
        }
    }
}