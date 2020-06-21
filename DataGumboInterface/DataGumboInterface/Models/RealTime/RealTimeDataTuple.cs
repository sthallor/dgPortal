using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGumboInterface.Models.RealTime
{
    public class RealTimeDataTuple<T>
    {
        public DateTimeOffset Timestamp { get; set; }
        public T Value { get; set; }
    }
}
