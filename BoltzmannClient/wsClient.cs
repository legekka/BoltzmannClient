using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Web.Script.Serialization;

namespace BoltzmannClient
{
    class wsClient
    {
        public static WebSocket ws;

        public wsClient(ClientInfo clientInfo)
        {
            ws = new WebSocket("ws://localhost:9880");
            ws.OnMessage += (sender, e) =>
            {
                if (!e.IsBinary) {
                    string msg = e.Data;
                    Console.WriteLine("Server: " + e.Data);
                }
                
                
            };
            try
            {
                ws.Connect();
            }
            catch(Exception ConnectionException)
            {
                Console.WriteLine(ConnectionException.Data);
                Console.ReadLine();
            }

            var json = new JavaScriptSerializer().Serialize(clientInfo);
            ws.Send(json);

            Console.ReadKey(true);
        }
    }
}
