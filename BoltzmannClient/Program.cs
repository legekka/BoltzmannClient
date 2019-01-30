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

        static void Main(string[] args)
        {
            clientInfo = new ClientInfo(args);
            wsClient = new wsClient(clientInfo);

        }

    }
}
