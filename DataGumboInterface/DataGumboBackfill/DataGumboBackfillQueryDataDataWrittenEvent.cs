using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using DataGumboInterface.Service;

namespace DataGumboBackfill
{
    public class DataGumboBackfillQueryDataDataWrittenEvent : DataGumboBackfillQueryEvent
    {
        public long Timestamp { get; set; }
        public object Value { get; set; }

        public DataGumboBackfillQueryDataDataWrittenEvent(DataGumboBackfillQuery query, long timestamp, object value) 
        {
            Query = query;
            Timestamp = timestamp;
            Value = value;
            Message = string.Format("Wrote data to DataGumbo - {0}: {1}", RealTimeDataService<int>.ConvertToDate(timestamp), value);
        }

        public DataGumboBackfillQueryDataDataWrittenEvent(DataGumboBackfillQuery query, DateTimeOffset dto, object value)
        {
            Query = query;
            Timestamp = RealTimeDataService<int>.ConvertDate(dto);
            Value = value;
            Message = string.Format("Wrote data to DataGumbo - {0}: {1}", dto, value);
        }

    }
}
