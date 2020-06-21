using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;

namespace DataGumboBackfill
{
    public class DataGumboBackfillQueryDataGapEvent : DataGumboBackfillQueryEvent
    {
        public DateTimeOffset GapStart { get; set; }
        public DateTimeOffset GapEnd { get; set; }

        public DataGumboBackfillQueryDataGapEvent(DataGumboBackfillQuery query, DateTimeOffset gapStart, DateTimeOffset gapEnd) 
        {
            Query = query;
            LogLevel = Level.Warn;
            GapStart = gapStart;
            GapEnd = gapEnd;
            Message = string.Format("Data gap found between {0} and {1}", gapStart, gapEnd);
        }
    }
}
