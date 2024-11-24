using CryptoAnalysisApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoAnalysisApp.Core.Services
{
    public class TechnicalIndicators
    {
        // Tính SMA (Simple Moving Average) cho một khoảng thời gian nhất định
        // SMA là trung bình cộng của giá đóng cửa trong một khoảng thời gian xác định (ví dụ 14 ngày).
        // Nó giúp làm mượt các biến động giá ngắn hạn và xác định xu hướng chung của thị trường.
        public static decimal CalculateSMA(List<CandleData> candles, int period)
        {
            if (candles.Count < period)
                throw new ArgumentException($"Cần ít nhất {period} dữ liệu để tính toán SMA.");

            decimal sum = 0;
            for (int i = 0; i < period; i++)
            {
                sum += candles[i].ClosePrice;
            }

            return sum / period;
        }

        // Tính RSI (Relative Strength Index) cho một khoảng thời gian nhất định (mặc định 14 ngày)
        // RSI đo lường mức độ thay đổi giá trong một khoảng thời gian (thường là 14 ngày). RSI dao động từ 0 đến 100,
        // với giá trị trên 70 cho thấy tài sản có thể bị mua quá mức (overbought),
        // và dưới 30 cho thấy tài sản có thể bị bán quá mức (oversold).
        public static decimal CalculateRSI(List<CandleData> candles, int period = 14)
        {
            if (candles.Count < period)
                throw new ArgumentException($"Cần ít nhất {period} dữ liệu để tính toán RSI.");

            decimal gains = 0;
            decimal losses = 0;

            // Tính toán mức tăng và giảm cho mỗi nến
            for (int i = 1; i <= period; i++)
            {
                decimal change = candles[i].ClosePrice - candles[i - 1].ClosePrice;
                if (change > 0)
                    gains += change;
                else
                    losses -= change;
            }

            decimal averageGain = gains / period;
            decimal averageLoss = losses / period;

            // Tránh trường hợp chia cho 0
            if (averageLoss == 0)
                return 100;

            decimal rs = averageGain / averageLoss;
            return 100 - (100 / (1 + rs));
        }

        // Tính EMA (Exponential Moving Average)
        // EMA là trung bình động có trọng số, trong đó các giá trị gần đây có trọng số lớn hơn so với các giá trị trước đó.
        // EMA phản ánh nhanh hơn sự thay đổi của giá so với SMA.
        private static decimal CalculateEMA(List<CandleData> candles, int period)
        {
            decimal multiplier = 2m / (period + 1);
            decimal ema = candles[0].ClosePrice;

            for (int i = 1; i < candles.Count; i++)
            {
                ema = ((candles[i].ClosePrice - ema) * multiplier) + ema;
            }

            return ema;
        }

        // Tính MACD (Moving Average Convergence Divergence) và Signal Line
        // MACD là sự chênh lệch giữa hai EMA (12 ngày và 26 ngày), giúp xác định sự chuyển động của giá và xu hướng.
        // Signal Line là EMA của MACD, thường dùng để xác định các điểm vào và ra tiềm năng của thị trường.
        public static (decimal macdLine, decimal signalLine) CalculateMACD(List<CandleData> candles)
        {
            if (candles.Count < 26)
                throw new ArgumentException("Cần ít nhất 26 dữ liệu để tính toán MACD.");

            decimal ema12 = CalculateEMA(candles.Take(12).ToList(), 12);
            decimal ema26 = CalculateEMA(candles.Take(26).ToList(), 26);

            decimal macdLine = ema12 - ema26;

            // Tính Signal Line (EMA của MACD)
            decimal signalLine = CalculateEMA(candles.Take(9).ToList(), 9);

            return (macdLine, signalLine);
        }
    }
}
