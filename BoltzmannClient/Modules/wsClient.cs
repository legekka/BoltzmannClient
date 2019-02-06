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
        public static List<FilePacket> filePackets;

        public static WebSocket ws;

        public wsClient()
        {
            ws = new WebSocket("ws://localhost:9880");
            ws.OnMessage += On_Message;
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

            Console.ReadLine();
        }

        public static void On_Message(object sender, MessageEventArgs e)
        {
            string msg = e.Data.Split('|')[0];
            switch (msg)
            {
                case "req clientinfo":
                    {
                        string message = "clientinfo|";
                        message += JsonConvert.SerializeObject(Program.clientInfo);
                        Send(message);
                    }
                    break;
                case "new job":
                    {
                        Job job = JsonConvert.DeserializeObject<Job>(e.Data.Split('|')[1]);
                        Program.JobHandler.NewJob(job);
                        Console.WriteLine("Job recieved. Job ID: " + job.JobID);
                    }
                    break;
                case "packet":
                    {
                        FilePacket filePacket = JsonConvert.DeserializeObject<FilePacket>(e.Data.Split('|')[1]);
                        if (filePackets == null)
                        {
                            filePackets = new List<FilePacket>();
                        }
                        Console.WriteLine("Got Packet{" + filePacket.PacketID + "}");

                        filePackets.Add(filePacket);
                        if (filePacket.LastPacket)
                        {
                            if (CheckFilePackets(filePacket.PacketID))
                            {
                                FilePacker.BuildFile(filePackets, Program.BlenderTaskPath);
                                Console.WriteLine("File built. ");
                                Program.JobHandler.Job.hasResources = true;
                                Console.WriteLine(filePacket.FileName + " Succesfully recieved.");
                                Program.JobHandler.ExecuteJob();
                            }
                        }
                    }
                    break;
            }
        }
 
        private static bool CheckFilePackets(int n)
        {
            
            int i = 0;
            while (i < n && filePackets[i] != null) { i++; }
            return (i == n);
        }

        public static void Send(string message)
        {
            ws.Send(message);
        }



    }

}
