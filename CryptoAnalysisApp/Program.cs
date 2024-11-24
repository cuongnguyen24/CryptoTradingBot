using CryptoAnalysisApp;
using CryptoAnalysisApp.Core.Models;
using CryptoAnalysisApp.Core.Services;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = "your_api_key_here";
        string secretKey = "your_secret_key_here";
        string passphrase = "your_passphrase_here";

        OKXApiClient okxClient = new OKXApiClient(apiKey, secretKey, passphrase);

        (TokenPair tokenPair, TimeInterval timeInterval, CandleLimit candleLimit) = GetMarketParameters();

        try
        {
            // Gọi API và lấy dữ liệu thị trường của cặp DOGS-USDT, với thời gian nến 1 giờ (1h), giới hạn 20 nến.
            string marketData = await okxClient.GetMarketData(tokenPair.GetPair(), timeInterval.GetInterval(), candleLimit.GetCandleLimit());

            // Parse JSON thành danh sách CandleData
            List<CandleData> candles = MarketDataParser.ParseMarketData(marketData);

            Console.WriteLine("Market Data:");
            foreach (var candle in candles)
            {
                Console.WriteLine($"Timestamp: {candle.Timestamp} (UTC), Open: {candle.OpenPrice}, High: {candle.HighPrice}, Low: {candle.LowPrice}, Close: {candle.ClosePrice}, Volume: {candle.Volume}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static (TokenPair, TimeInterval, CandleLimit) GetMarketParameters()
    {
        TokenPair tokenPair = new TokenPair("TON", "USDT");
        TimeInterval timeInterval = new TimeInterval("1H");
        CandleLimit candleLimit = new CandleLimit(20);

        return (tokenPair, timeInterval, candleLimit);
    }
}
