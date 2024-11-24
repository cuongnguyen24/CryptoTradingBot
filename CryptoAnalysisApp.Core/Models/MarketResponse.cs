using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAnalysisApp.Core.Models
{
    internal class MarketResponse
    {
        public string Code { get; set; }  // Mã lỗi hoặc mã thành công
        public string Msg { get; set; }   // Thông báo lỗi hoặc thông báo thành công
        public List<CandleData> Data { get; set; } // Danh sách các nến
    }
}
