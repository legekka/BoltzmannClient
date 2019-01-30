using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace BoltzmannClient
{
    class wsClient
    {
        public static WebSocket ws;

        public wsClient()
        {
            ws = new WebSocket("ws://localhost:9880");
            ws.OnMessage += (sender, e) =>
            {
                Console.WriteLine("Server: " + e.Data);
            };
            ws.Connect();
            ws.Send("ezkelllegyen");
            Console.ReadKey(true);
        }
    }
}
