using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using DataGumboInterface.Models.Structured;

namespace DataGumboInterface.Service
{
    public class WellService : ParentedStructuredDataService<Well>
    {
        public override string GumboTypeName
        {
            get { return "well"; }
        }

        public override string GumboParentIdName
        {
            get { return "currentRigId"; }
        }
    }
}
