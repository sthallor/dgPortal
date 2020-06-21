using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGumboInterface.Models.Structured;

namespace DataGumboInterface.Service
{
    public class DrillingAssemblyComponentService : ParentedStructuredDataService<DrillingAssemblyComponent>
    {
        public override string GumboTypeName
        {
            get { return "drillingAssemblyComponent"; }
        }
        public override string GumboParentIdName
        {
            get { return "drillingAssemblyId"; }
        }
    }
}
