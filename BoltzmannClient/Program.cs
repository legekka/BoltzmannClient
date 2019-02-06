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
        public static Blender Blender;
        public static JobHandler JobHandler;

        static void Main(string[] args)
        {
            blenderPath = Blender.GetBlenderPath(args);
            //clientInfo = new ClientInfo(400,450,true);
            clientInfo = new ClientInfo();
            JobHandler = new JobHandler();
            wsClient = new wsClient();
            
        } 
    }
}
