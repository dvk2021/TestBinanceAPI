using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;

using Newtonsoft.Json;

namespace TestBinanceAPI
{
    internal sealed class WebSocketUtils
    {
        private WebSocketUtils()
        {
        }

        private const string SUBSCRIBE_METHOD = "SUBSCRIBE";
        private const string UNSUBSCRIBE_METHOD = "UNSUBSCRIBE";

        internal sealed class Message
        {
            public Message(string method, int messageID, string stream)
            {
                this.method = method;
                this.@params = new List<string>();
                this.@params.Add(stream);
                this.id = messageID;
            }

            public string method;
            public List<string> @params;
            public int id;
        }

        public static ClientWebSocket? Subscribe(string uriString, string stream)
        {
            try
            {
                ClientWebSocket webSocket = new ClientWebSocket();
                webSocket.ConnectAsync(new Uri(uriString), CancellationToken.None).Wait();
                if (webSocket.State == WebSocketState.Open)
                {
                    Message message = new Message(WebSocketUtils.SUBSCRIBE_METHOD, 1, stream);
                    string subscribeMessage = JsonConvert.SerializeObject(message);
                    WebSocketUtils.Send(webSocket, subscribeMessage);
                    return webSocket;
                }
                else
                {
                    Console.WriteLine($"Can't open web socket \"{uriString}.");
                    return null;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Web socket subscribe, exception \"{exception.Message}\".");
                return null;
            }
        }

        private static void Send(ClientWebSocket webSocket, string message)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            }
        }

        public static string Receive(ClientWebSocket webSocket, int bufferSize = 1024)
        {
            try
            {
                string buffer = "";
                var receiveBuffer = WebSocket.CreateClientBuffer(bufferSize, bufferSize);
                while (true)
                {
                    if (webSocket.State != WebSocketState.Open)
                    {
                        Console.WriteLine("Receive: invalid web socket.");
                        break;
                    }
                    var result = webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None).Result;
                    if (result != null)
                    {
                        if (receiveBuffer.Array != null)
                        {
                            buffer += Encoding.UTF8.GetString(receiveBuffer.Array, 0, result.Count);
                        }
                        if (result.EndOfMessage == true)
                        {
                            break;
                        }
                    }
                }
                return buffer;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Web socket receive, exception \"{exception.Message}\".");
                return "";
            }
        }

        public static void UnSubscribe(ClientWebSocket webSocket, string stream)
        {
            try
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    Message message = new Message(WebSocketUtils.UNSUBSCRIBE_METHOD, 1, stream);
                    string subscribeMessage = JsonConvert.SerializeObject(message);
                    WebSocketUtils.Send(webSocket, subscribeMessage);
                    webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "try close", CancellationToken.None).Wait();
                    if (webSocket.State != WebSocketState.Closed)
                    {
                        Console.WriteLine("Can't close web socket.");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Web socket un-subscribe, exception \"{exception.Message}\".");
            }
        }
    }
}
