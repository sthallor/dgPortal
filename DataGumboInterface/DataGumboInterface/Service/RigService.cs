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
    public class RigService : StructuredDataService<Rig>
    {
        public override string GumboTypeName
        {
            get { return "rig"; }
        }

        public Rig GetByRigName(string rigName)
        {
            return GetAll().FirstOrDefault(r => r.rigName == rigName);
        }
    }
}
