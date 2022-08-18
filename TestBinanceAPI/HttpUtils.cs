namespace TestBinanceAPI
{
    internal sealed class HttpUtils
    {
        private HttpUtils()
        {
        }

        public static string GetResponse(string request)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var task = httpClient.GetStringAsync(request);
                var response = task.Result;
                return response;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Request \"{request}\", exception \"{exception.Message}\".");
                return "";
            }
        }
    }
}
