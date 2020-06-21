using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;

namespace DataGumboBackfill
{
    public abstract class DataGumboBackfillQuery
    {
        public string RigNumber { get; set; }
        public string VariableName { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public TimeSpan GapThrehold { get; set; }

        public abstract void Execute();

        #region event handling
        public delegate void DataGumboBackfillQueryEventhandler(DataGumboBackfillQueryEvent e);
        public event DataGumboBackfillQueryEventhandler DgbfqeProcessing;
        #endregion

        public void OnDataGumboBackfillQueryEvent(DataGumboBackfillQueryEvent evt)
        {
            DgbfqeProcessing(evt);
        }

        public void SendDebugMessage(string message)
        {
            FireEvent(message, Level.Debug);
        }

        public void SendWarningMessage(string message)
        {
            FireEvent(message, Level.Warn);
        }

        public void SendErrorMessage(string message)
        {
            FireEvent(message, Level.Error);
        }

        public void FireEvent(string message, Level logLevel)
        {
            OnDataGumboBackfillQueryEvent(new DataGumboBackfillQueryEvent { Query = this, Message = message, LogLevel = logLevel });
        }
    }
}
