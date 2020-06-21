using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGumboInterface.Service;
using DataGumboInterface.Models.RealTime;

namespace DataGumboBackfill
{
    public class FindDataGapsInRealTimeDataQuery<T> : DataGumboBackfillQuery
    {
        private static readonly TimeSpan LoopTimeSpan = new TimeSpan(0, 1, 0, 0);

        public override void Execute()
        {
            SendDebugMessage(string.Format("Kicking off query to find data gaps greater than {0} between {1} and {2}", GapThrehold, Start, End));
            RealTimeDataService<T> svc = new RealTimeDataService<T>();
            
            DateTimeOffset loopDtmStart = Start;
            while (loopDtmStart < End) 
            {
                DateTimeOffset loopDtmEnd = loopDtmStart.AddTicks(LoopTimeSpan.Ticks).AddSeconds(-1);
                loopDtmEnd = loopDtmEnd > End ? End : loopDtmEnd;
                SendDebugMessage(string.Format("Checking between {0} and {1}", loopDtmStart, loopDtmEnd));
                var results = svc.Get(RigNumber, VariableName, loopDtmStart, loopDtmEnd);
                CheckForGaps(results, loopDtmStart, loopDtmEnd);
                loopDtmStart = loopDtmStart.AddTicks(LoopTimeSpan.Ticks);
            }
        }

        public void CheckForGaps(IEnumerable<RealTimeDataTuple<T>> records, DateTimeOffset start, DateTimeOffset end)
        {
            if (records.Any())
            {
                DateTimeOffset prevDtm = start;
                foreach (RealTimeDataTuple<T> record in records.OrderBy(rec => rec.Timestamp))
                {
                    TimeSpan differenceSincePrev = new TimeSpan(record.Timestamp.UtcTicks - prevDtm.UtcTicks);
                    if (differenceSincePrev.TotalMilliseconds > GapThrehold.TotalMilliseconds)
                    {
                        OnDataGumboBackfillQueryEvent(new DataGumboBackfillQueryDataGapEvent(this, prevDtm, record.Timestamp));
                        //SendWarningMessage(string.Format("Data gap found between {0} and {1}", prevDtm, record.Timestamp));
                    }
                    prevDtm = record.Timestamp;
                }
                TimeSpan differenceForLastRecord = new TimeSpan(end.UtcTicks - prevDtm.UtcTicks);
                if (differenceForLastRecord.TotalMilliseconds > GapThrehold.TotalMilliseconds)
                {
                    //SendWarningMessage(string.Format("Data gap found between {0} and {1}", prevDtm, end));
                    OnDataGumboBackfillQueryEvent(new DataGumboBackfillQueryDataGapEvent(this, prevDtm, end));
                }
            }
            else
            {
                //SendWarningMessage(string.Format("Data gap found between {0} and {1}", start, end));
                OnDataGumboBackfillQueryEvent(new DataGumboBackfillQueryDataGapEvent(this, start, end));
            }
        }
    }
}
