using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGumboInterface.Models.Structured;

namespace DataGumboInterface.Service
{
    public class TrippingSpeedService : ParentedStructuredDataService<TrippingSpeed>
    {
        public override string GumboTypeName
        {
            get { return "trippingSpeed"; }
        }
        public override string GumboParentIdName
        {
            get { return "wellId"; }
        }
    }
}
