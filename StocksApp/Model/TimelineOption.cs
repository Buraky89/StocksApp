using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Model
{
    public class TimelineOption
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public bool IsDefault { get; set; }

        public TimelineOption()
        {
        }
    }
}
