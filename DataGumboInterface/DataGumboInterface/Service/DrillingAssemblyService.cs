using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGumboInterface.Models.Structured;

namespace DataGumboInterface.Service
{
    public class DrillingAssemblyService : ParentedStructuredDataService<DrillingAssembly>
    {
        public override string GumboTypeName
        {
            get { return "drillingAssembly"; }
        }
        public override string GumboParentIdName
        {
            get { return "tourSheetId"; }
        }
    }
}
