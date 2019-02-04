using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using static System.Net.Mime.MediaTypeNames;

namespace BoltzmannClient
{
    class Program
    {
        public static ClientInfo clientInfo;
        public static wsClient wsClient;
        public static string blenderPath;
        public const string BlenderTaskPath = @".\Blender\";

        static void Main(string[] args)
        {
            /*Blender.GetBlenderPath(args);
            clientInfo = new ClientInfo();
            Console.ReadLine();
            RenderSetting rS = new RenderSetting(@"H:\teszt.blend", @"H:\kep", 250, 2);
            Blender blender = new Blender(clientInfo);
            blender.RunBlenderTask(rS);
            wsClient = new wsClient(clientInfo);
            */

            wsClient = new wsClient(clientInfo);
            Console.ReadLine();
        }

        
    }
}
