using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGumboInterface.Models.Structured;

namespace DataGumboInterface.Service
{
    public class OperationalRemarkService : ParentedStructuredDataService<OperationalRemark>
    {
        public override string GumboTypeName
        {
            get { return "operationalRemark"; }
        }
        public override string GumboParentIdName
        {
            get { return "wellId"; }
        }
    }
}
