using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace BoltzmannClient
{
    class wsClient
    {
        public List<FilePacket> filePackets;

        public static WebSocket ws;

        public wsClient(ClientInfo clientInfo)
        {
            ws = new WebSocket("ws://localhost:9880");
            ws.OnMessage += (sender, e) =>
            {
                string msg = e.Data.Split('|')[0];
                switch (msg)
                {
                    case "packet": {
                            FilePacket filePacket = JsonConvert.DeserializeObject<FilePacket>(e.Data.Split('|')[1]);
                            if (filePackets == null)
                            {
                                filePackets = new List<FilePacket>();
                            }
                            Console.WriteLine("Got Packet{" + filePacket.PacketID + "}");
                            
                            filePackets.Add(filePacket);
                            if (filePacket.LastPacket)
                                if (CheckFilePackets(filePacket.PacketID))
                                {
                                    FilePacker.BuildFile(filePackets, Program.BlenderTaskPath);
                                    Console.WriteLine(filePacket.FileName + " Succesfully recieved.");
                                }
                        }
                        break;
                    case "new job":
                        {

                        }
                        break;
                }
            };
            try
            {
                ws.Connect();
            }
            catch (Exception ConnectionException)
            {
                Console.WriteLine(ConnectionException.Data);
                Console.ReadLine();
            }

            /*var json = new JavaScriptSerializer().Serialize(clientInfo);
            ws.Send(json);
            */
            Console.ReadKey(true);
        }

        private bool CheckFilePackets(int n)
        {
            int i = 0;
            while (i < n && filePackets[i] != null) { i++; }

            return (i == n);
        }
    }

}
