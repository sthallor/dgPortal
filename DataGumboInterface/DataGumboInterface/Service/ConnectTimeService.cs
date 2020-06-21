using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGumboInterface.Models.Structured;

namespace DataGumboInterface.Service
{
    public class ConnectTimeService : ParentedStructuredDataService<ConnectTime>
    {
        public override string GumboTypeName
        {
            get { return "connectTime"; }
        }
        public override string GumboParentIdName
        {
            get { return "wellId"; }
        }
    }
}
