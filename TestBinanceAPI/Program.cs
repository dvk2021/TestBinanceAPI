
const string COMMAND_EXIT = "exit";
const string COMMAND_HELP = "help";

TestBinanceAPI.BinanceAPI binanceAPI = new TestBinanceAPI.BinanceAPI();
while (true)
{
    Console.Write("Enter command> ");
    string command = Console.ReadLine().Trim();
    if (command == COMMAND_EXIT)
    {
        break;
    }
    if (command == COMMAND_HELP)
    {
        Console.WriteLine(
            "Commands:\n" +
            $"\"{COMMAND_EXIT}\" - exit\n" +
            $"\"{COMMAND_HELP}\" - command list\n" +
            $"\"{TestBinanceAPI.BinanceAPI.COMMAND_INFO}\" - load Binance instruments\n" +
            $"\"{TestBinanceAPI.BinanceAPI.COMMAND_INFO} symbol\" - get Binance instrument info\n" +
            $"\"{TestBinanceAPI.BinanceAPI.COMMAND_SUBSCRIBE} symbol\" - subscribe/unsubscribe on Aggregate Trade Streams");
    }
    else if (binanceAPI.ProcessCommand(command) == false)
    {
        Console.WriteLine($"Command \"{command}\" not found. Enter \"{COMMAND_HELP}\" for more information.");
    }
}
