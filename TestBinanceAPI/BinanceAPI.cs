using System.Text.Json;

namespace TestBinanceAPI
{
    internal sealed class BinanceAPI
    {
        public BinanceAPI()
        {
            this.instrumentsLoaded = false;
            this.instruments = new HashSet<string>();
        }

        public const string COMMAND_INFO = "info";
        public const string COMMAND_SUBSCRIBE = "subscribe";

        public bool ProcessCommand(string command)
        {
            if (command == BinanceAPI.COMMAND_INFO)
            {
                this.LoadInstruments();
                return true;
            }
            else if (command.StartsWith(BinanceAPI.COMMAND_INFO) == true)
            {
                this.LoadInstruments();
                this.LoadInstrument(command);
                return true;
            }
            else if (command.StartsWith(BinanceAPI.COMMAND_SUBSCRIBE) == true)
            {
                this.LoadInstruments();
                this.Subscribe(command);
                return true;
            }
            return false;
        }

        private const string BINANCE_URL = "https://api.binance.com";

        private const string EXCHANGE_INFO = "/api/v3/exchangeInfo";
        private const string SYMBOL = "symbol";

        private void LoadInstruments()
        {
            if (this.instrumentsLoaded == false)
            {
                string instruments = HttpUtils.GetResponse($"{BinanceAPI.BINANCE_URL}{BinanceAPI.EXCHANGE_INFO}");
                if (instruments == "")
                {
                    Console.WriteLine("Can't load instruments.");
                }
                else
                {
                    var result = JsonSerializer.Deserialize<ExchangeInfoSymbols>(instruments);
                    if (result != null)
                    {
                        foreach (var symbol in result.symbols)
                        {
                            this.instruments.Add(symbol.symbol);
                        }
                        this.instrumentsLoaded = true;
                    }
                    Console.WriteLine($"Loaded {this.instruments.Count} instruments.");
                }
            }
        }
        private bool instrumentsLoaded { get; set; }
        private HashSet<string> instruments;

        private void LoadInstrument(string command)
        {
            if (this.instrumentsLoaded == true)
            {
                string[] symbols = command.Split(" ");
                for (int index = 1; index < symbols.Length; index++)
                {
                    string symbol = symbols[index].ToUpper();
                    if (this.instruments.Contains(symbol) == false)
                    {
                        Console.WriteLine($"Instrument \"{symbols[index]}\" not found.");
                    }
                    else
                    {
                        string instruments = HttpUtils.GetResponse(
                            $"{BinanceAPI.BINANCE_URL}{BinanceAPI.EXCHANGE_INFO}?{BinanceAPI.SYMBOL}={symbol}");
                        if (instruments == "")
                        {
                            Console.WriteLine($"Can't load instrument \"{symbol}\".");
                        }
                        else
                        {
                            var result = JsonSerializer.Deserialize<ExchangeInfoSymbol>(instruments);
                            if ((result != null) && (result.symbols.Count > 0))
                            {
                                result.symbols[0].WriteInfo();
                            }
                        }
                    }
                }
            }
        }

        private const string BINANCE_WS = "wss://stream.binance.com:9443/ws";
        private const string BINANCE_STREAM = "{0}@aggTrade";

        private void Subscribe(string command)
        {
            if (this.instrumentsLoaded == true)
            {
                string[] symbols = command.Split(" ");
                for (int index1 = 1; index1 < symbols.Length; index1++)
                {
                    string symbol = symbols[index1].ToUpper();
                    if (this.instruments.Contains(symbol) == false)
                    {
                        Console.WriteLine($"Instrument \"{symbols[index1]}\" not found.");
                    }
                    else
                    {
                        string stream = string.Format(BinanceAPI.BINANCE_STREAM, symbol.ToLower());
                        var webSocket = WebSocketUtils.Subscribe($"{BinanceAPI.BINANCE_WS}", stream);
                        if (webSocket != null)
                        {
                            for (int index2 = 1; index2 <= 1000; index2++)
                            {
                                string message = WebSocketUtils.Receive(webSocket, 1024);
                                if (message == "")
                                {
                                    break;
                                }
                                Console.WriteLine($"{index2}) {message}");
                            }
                            WebSocketUtils.UnSubscribe(webSocket, stream);
                        }
                    }
                }
            }
        }
    }
}
