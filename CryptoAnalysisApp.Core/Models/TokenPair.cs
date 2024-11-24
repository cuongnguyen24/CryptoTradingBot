using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAnalysisApp.Core.Models
{
    public class TokenPair
    {
        public string BaseToken { get; set; }
        public string QuoteToken { get; set; }

        public TokenPair(string baseToken, string quoteToken)
        {
            BaseToken = baseToken;
            QuoteToken = quoteToken;
        }

        public string GetPair()
        {
            return $"{BaseToken}-{QuoteToken}";
        }
    }
}
