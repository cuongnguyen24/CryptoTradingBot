using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace CryptoAnalysisApp
{
    public class OKXApiClient
    {
        private readonly HttpClient? _client;
        private readonly string? _apiKey;
        private readonly string? _secretKey;
        private readonly string? _passphrase;

        public OKXApiClient(string apiKey, string secretKey, string passphrase)
        {
            _client = new HttpClient();
            _apiKey = apiKey;
            _secretKey = secretKey;
            _passphrase = passphrase;
        }

        // Hàm tạo chữ ký xác thực yêu cầu API
        private string GenerateSignature(string timestamp, string method, string endpoint, string body)
        {
            // Tạo chuỗi cần mã hóa từ timestamp, phương thức HTTP, endpoint và body.
            string preHash = $"{timestamp}{method.ToUpper()}{endpoint}{body}";

            // Sử dụng HMACSHA256 để mã hóa chuỗi với Secret Key.
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(preHash)); // Mã hóa chuỗi.
                return Convert.ToBase64String(hash); // Chuyển kết quả mã hóa sang chuỗi Base64.
            }
        }

        // Hàm lấy timestamp hiện tại theo định dạng chuẩn ISO8601
        private string GetTimestamp()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); // Trả về thời gian UTC theo định dạng chuẩn.
        }

        // Phương thức gọi API để lấy dữ liệu thị trường (candlestick data)
        public async Task<string> GetMarketData(string instrumentId, string bar = "1m", int limit = 100)
        {
            string endpoint = $"/api/v5/market/candles?instId={instrumentId}&bar={bar}&limit={limit}";
            string url = $"https://www.okx.com{endpoint}";
            string timestamp = GetTimestamp();
            string body = ""; 
            // Tạo chữ ký cho yêu cầu GET
            string signature = GenerateSignature(timestamp, "GET", endpoint, body);

            // Thiết lập các header yêu cầu API cần thiết
            _client.DefaultRequestHeaders.Clear(); // Xóa các header cũ.
            _client.DefaultRequestHeaders.Add("OK-ACCESS-KEY", _apiKey); // Thêm header API Key.
            _client.DefaultRequestHeaders.Add("OK-ACCESS-SIGN", signature); // Thêm chữ ký vào header.
            _client.DefaultRequestHeaders.Add("OK-ACCESS-TIMESTAMP", timestamp); // Thêm timestamp vào header.
            _client.DefaultRequestHeaders.Add("OK-ACCESS-PASSPHRASE", _passphrase); // Thêm passphrase vào header.

            // Gửi yêu cầu GET tới OKX API
            HttpResponseMessage response = await _client.GetAsync(url);

            // Kiểm tra xem yêu cầu có thành công không
            response.EnsureSuccessStatusCode();

            // Đọc kết quả trả về từ API và trả lại dưới dạng chuỗi JSON
            return await response.Content.ReadAsStringAsync();
        }
    }
}
