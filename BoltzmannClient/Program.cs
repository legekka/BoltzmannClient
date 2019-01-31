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
            /*GetBlenderPath(args);
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

        private static string GetBlenderPath(string[] args)
        {
            string path = "";
            string[] value = System.Environment.GetEnvironmentVariable("PATH").ToLower().Split(';');
            if (value.Length == 0)
                value = System.Environment.GetEnvironmentVariable("Path").ToLower().Split(';');
            int i = 0;
            while (i < value.Length && !value[i].Contains("blender")) { i++; }

            if (i == value.Length)
            {
                Console.WriteLine("Blender was not found in %PATH%");
                if (args.Length < 1)
                {
                    Console.WriteLine("You must specify the Blender folder's path!");
                    Console.ReadLine();
                    Environment.Exit(200);
                }
                else
                {
                    path = args[0];

                }
            }
            else
            {
                path = value[i];
            }

            if (path[path.Length - 1] != '\\')
                path += @"\";
            return path;
        }
    }
}
