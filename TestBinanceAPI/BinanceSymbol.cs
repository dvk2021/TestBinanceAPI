namespace TestBinanceAPI
{
	internal sealed class ExchangeInfoSymbols
	{
        public ExchangeInfoSymbols()
		{
			this.symbols = new List<BinanceSymbolName>();
		}
		public List<BinanceSymbolName> symbols { get; set; }
	}


	internal sealed class BinanceSymbolName
	{
		public string symbol { get; set; }
	}

    internal sealed class ExchangeInfoSymbol
    {
        public ExchangeInfoSymbol()
        {
            this.symbols = new List<BinanceSymbol>();
        }
        public List<BinanceSymbol> symbols { get; set; }
    }

    internal sealed class BinanceSymbol
    {
		public BinanceSymbol()
		{
			this.symbol = "";
			this.status = "";
			this.baseAsset = "";
			this.quoteAsset = "";
			this.orderTypes = new List<string>();
			this.filters = new List<Filter>();
		}

		public string symbol { get; set; }
		public string status { get; set; }
		public string baseAsset { get; set; }
		public int baseAssetPrecision { get; set; }
		public string quoteAsset { get; set; }
		public int quotePrecision { get; set; }
		public int quoteAssetPrecision { get; set; }
		public int baseCommissionPrecision { get; set; }
		public int quoteCommissionPrecision { get; set; }
		public List<string> orderTypes { get; set; }
        public bool icebergAllowed { get; set; }
        public bool ocoAllowed { get; set; }
        public bool quoteOrderQtyMarketAllowed { get; set; }
        public bool allowTrailingStop { get; set; }
        public bool cancelReplaceAllowed { get; set; }
        public bool isSpotTradingAllowed { get; set; }
        public bool isMarginTradingAllowed { get; set; }
		public List<Filter> filters { get; set; }

		internal sealed class Filter
		{
			public Filter()
			{
				this.filterType = "";
				this.minPrice = "";
				this.maxPrice = "";
				this.tickSize = "";
			}

			public string filterType { get; set; }
			public string minPrice { get; set; }
			public string maxPrice { get; set; }
			public string tickSize { get; set; }
        }

		public void WriteInfo()
		{
			Console.WriteLine(
				$"Symbol \"{this.symbol}\", status \"{this.status}\", base \"{this.baseAsset}\"({this.baseAssetPrecision}), quote \"{this.quoteAsset}\"({this.quoteAssetPrecision}).");
			if (this.orderTypes.Count > 0)
			{
				Console.Write("Order types: ");
				for (int index = 0; index < this.orderTypes.Count; index++)
				{
					if (index > 0)
					{
						Console.Write(", ");
					}
					Console.Write(this.orderTypes[index]);
				}
				Console.WriteLine(".");
			}
			Console.Write("Allowed: ");
			Console.Write($"iceberg {this.icebergAllowed}, ");
            Console.Write($"oco {this.ocoAllowed}, ");
            Console.Write($"quoteOrderQtyMarket {this.quoteOrderQtyMarketAllowed}, ");
            Console.Write($"trailingStop {this.allowTrailingStop}, ");
            Console.Write($"cancelReplace {this.cancelReplaceAllowed}, ");
            Console.Write($"isSpotTrading {this.isSpotTradingAllowed}, ");
            Console.WriteLine($"isMarginTrading {this.isMarginTradingAllowed}.");
			if (this.filters.Count > 0)
			{
				Console.WriteLine("Filters:");
				for (int index = 0; index < this.filters.Count; index++)
				{
					Filter filter = this.filters[index];
					Console.WriteLine($"{index}) type \"{filter.filterType}\", min price {filter.minPrice}, max price {filter.maxPrice}, tick size {filter.tickSize}.");

			    }
			}
		}
	}
}
