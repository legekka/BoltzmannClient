using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BoltzmannClient
{
    class FilePacker
    {
        private string filePath;
        private int packetSize;
        private List<FilePacket> filePackets;


        public FilePacker(string filepath, int packetsize)
        {
            filePath = filepath;
            packetSize = packetsize;
        }

        public void Generate()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();
            byte[] data = File.ReadAllBytes(filePath);
            int n = (int)Math.Ceiling((decimal)data.Length % packetSize);
            byte[] part = new byte[packetSize];
            FilePacket fPacket;

            for (int i = 0; i < n - 1; i++)
            {
                part = new byte[packetSize];
                Array.Copy(data, i * packetSize, part, 0, (i + 1) * packetSize);
                fPacket = new FilePacket(i, Path.GetFileName(filePath), part);
                filePackets.Add(fPacket);
            }
            part = new byte[packetSize];
            Array.Copy(data, (n - 1) * packetSize, part, 0, data.Length - ((n - 1) * packetSize));
            fPacket = new FilePacket((n - 1), Path.GetFileName(filePath), part, true);
            filePackets.Add(fPacket);
            var json = new JavaScriptSerializer().Serialize(filePackets);
            Console.WriteLine(json);
            Console.ReadLine();
        }

    }
    class FilePacket
    {
        public int PacketID;
        public string FileName;
        public byte[] Data;
        public bool LastPacket;

        public FilePacket(int packetId, string fileName, byte[] data)
        {
            PacketID = packetId;
            fileName = FileName;
            Data = data;
            LastPacket = false;
        }
        public FilePacket(int packetId, string fileName, byte[] data, bool lastPacket)
        {
            PacketID = packetId;
            fileName = FileName;
            Data = data;
            LastPacket = lastPacket;
        }
    }
}
