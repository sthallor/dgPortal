using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;

namespace DataGumboBackfill
{
    public class DataGumboBackfillQueryEvent : EventArgs
    {
        public DataGumboBackfillQuery Query { get; set; }
        public string Message { get; set; }
        public Level LogLevel { get; set;}

        public DataGumboBackfillQueryEvent()
        {
            LogLevel = Level.Debug;
        }
    }
}
