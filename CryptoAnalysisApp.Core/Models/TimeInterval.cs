using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAnalysisApp.Core.Models
{
    public class TimeInterval
    {
        public string Interval { get; set; }

        public TimeInterval(string interval)
        {
            Interval = interval;
        }

        public string GetInterval()
        {
            return Interval;
        }
    }
}
