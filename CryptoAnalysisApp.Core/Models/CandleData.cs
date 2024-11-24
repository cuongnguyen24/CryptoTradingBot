using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAnalysisApp.Core.Models
{
    public class CandleData
    {
        public DateTime Timestamp { get; set; } // Thời gian của nến
        public decimal OpenPrice { get; set; }  // Giá mở cửa
        public decimal HighPrice { get; set; }  // Giá cao nhất
        public decimal LowPrice { get; set; }   // Giá thấp nhất
        public decimal ClosePrice { get; set; } // Giá đóng cửa
        public decimal Volume { get; set; }     // Khối lượng giao dịch
    }
}
