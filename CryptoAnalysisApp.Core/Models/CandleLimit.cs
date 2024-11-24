using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAnalysisApp.Core.Models
{
    public class CandleLimit
    {
        public int Limit { get; set; }

        public CandleLimit(int limit)
        {
            Limit = limit;
        }

        public int GetCandleLimit()
        {
            return Limit;
        }
    }
}
