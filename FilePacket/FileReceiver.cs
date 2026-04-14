using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkHandler;
using System.IO;

namespace FilePacket
{
    public class FileReceiver
    {
        private UdpClient listener;
        private Dictionary<string, ReceiveState> activeTransfers = new Dictionary<string, ReceiveState>();
        private bool isRunning = true;
        public StringBuilder ReceivedContent { get; private set; } = new StringBuilder();
        public string CurrentFileName { get; private set; } = "";
        public int CurrentPacket { get; private set; } = 0;
        public int TotalPackets { get; private set; } = 0;
        public bool IsComplete { get; private set; } = false;
        private string currentPacketId = "";
        private class ReceiveState
        {
            public Dictionary<int, byte[]> Packets { get; set; } = new Dictionary<int, byte[]>();
            public int TotalPackets { get; set; }
            public string FileName { get; set; }
            public DateTime LastUpdate { get; set; }
        }

        public async Task StartReceiving(int port)
        {
            listener = new UdpClient(port);
            while (isRunning)
            {
                try
                {
                    var result = await listener.ReceiveAsync();
                    ProcessPacket(result.Buffer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка приема: {ex.Message}");
                }
            }
        }

        private void ProcessPacket(byte[] data)
        {
            try
            {
                var packet = FilePacket.Deserialize(data);
                lock (activeTransfers)
                {
                    if (!activeTransfers.ContainsKey(packet.PacketId))
                    {
                        activeTransfers[packet.PacketId] = new ReceiveState
                        {
                            TotalPackets = packet.TotalPackets,
                            FileName = packet.FileName,
                            LastUpdate = DateTime.Now
                        };
                        CurrentFileName = packet.FileName;
                        TotalPackets = packet.TotalPackets;
                        IsComplete = false;
                        currentPacketId = packet.PacketId;
                        ReceivedContent.Clear();
                    }

                    var state = activeTransfers[packet.PacketId];
                    if (!state.Packets.ContainsKey(packet.PacketNumber))
                    {
                        state.Packets[packet.PacketNumber] = packet.Data;
                        state.LastUpdate = DateTime.Now;
                        CurrentPacket = state.Packets.Count;

                        string textData = Encoding.UTF8.GetString(packet.Data);
                        ReceivedContent.AppendLine($"=== Пакет {CurrentPacket}/{TotalPackets} ===");
                        ReceivedContent.AppendLine(textData);
                        ReceivedContent.AppendLine();

                        if (state.Packets.Count == packet.TotalPackets)
                        {
                            IsComplete = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки пакета: {ex.Message}");
            }
        }

        public bool SaveFile(string savePath)
        {
            if (string.IsNullOrEmpty(currentPacketId))
                return false;

            lock (activeTransfers)
            {
                if (!activeTransfers.ContainsKey(currentPacketId))
                    return false;
                var state = activeTransfers[currentPacketId];
                if (state.Packets.Count != state.TotalPackets)
                    return false;
                try
                {
                    using (var fs = new FileStream(savePath, FileMode.Create))
                    {
                        for (int i = 0; i < state.TotalPackets; i++)
                        {
                            if (state.Packets.ContainsKey(i))
                            {
                                fs.Write(state.Packets[i], 0, state.Packets[i].Length);
                            }
                        }
                    }
                    activeTransfers.Remove(currentPacketId);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public void ResetReceiver()
        {
            activeTransfers.Clear();
            ReceivedContent.Clear();
            CurrentFileName = "";
            CurrentPacket = 0;
            TotalPackets = 0;
            IsComplete = false;
            currentPacketId = "";  
        }
    }
}