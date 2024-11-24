using CryptoAnalysisApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace CryptoAnalysisApp.Core.Services
{
    public class MarketDataParser
    {
        public static List<CandleData> ParseMarketData(string jsonData)
        {
            // Deserialize JSON thành đối tượng chứa code, msg và data
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonData);
            var candles = new List<CandleData>();

            // Kiểm tra xem code có phải là 0 (thành công) hay không
            if (apiResponse.Code == "0")
            {
                // Chuyển đổi từng phần tử trong data thành CandleData
                foreach (var item in apiResponse.Data)
                {
                    candles.Add(new CandleData
                    {
                        Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(item[0])).UtcDateTime,
                        OpenPrice = decimal.Parse(item[1], CultureInfo.InvariantCulture),
                        HighPrice = decimal.Parse(item[2], CultureInfo.InvariantCulture),
                        LowPrice = decimal.Parse(item[3], CultureInfo.InvariantCulture),
                        ClosePrice = decimal.Parse(item[4], CultureInfo.InvariantCulture),
                        Volume = decimal.Parse(item[5], CultureInfo.InvariantCulture)
                    });
                }
            }
            else
            {
                throw new Exception("Error fetching market data: " + apiResponse.Msg);
            }

            return candles;
        }
    }

    // Lớp đại diện cho phản hồi của API (bao gồm code, msg và data)
    public class ApiResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("data")]
        public List<List<string>> Data { get; set; }
    }
}
