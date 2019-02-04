using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltzmannClient
{
    class FilePacket
    {
        public int PacketID;
        public int TotalCount;
        public string FileName;
        public byte[] Data;
        public bool LastPacket;

        public FilePacket(int packetId, string fileName, byte[] data)
        {
            PacketID = packetId;
            FileName = fileName;
            Data = data;
            LastPacket = false;
        }
        [JsonConstructor]
        public FilePacket(int packetId, string fileName, byte[] data, bool lastPacket)
        {
            PacketID = packetId;
            FileName = fileName;
            Data = data;
            LastPacket = lastPacket;
        }
    }
}
