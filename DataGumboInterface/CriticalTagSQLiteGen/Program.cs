using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackGoldData;
using IgnitionSQLiteTagGen;
using System.IO;
using System.Data;

namespace CriticalTagSQLiteGen
{
    class Program
    {
        public static string[] rigs = new[] { "119", "121", "135", "137", "140", "142", "144", "145", "147", "148", "150", "151", "152", "153", "155", "156", "157", "158", "160", "161", "162", "548", "549", "550", "759", "760", "762", "765", "766", "767", "768", "769", "770", "771", "772", "773", "774", "775", "776", "777", "778", "784", "785", "786", "T047", "T052", "T054", "T100", "T120", "T121", "T125", "T137", "T139", "T140", "T141", "T223", "T224", "T225", "T226", "T701", "T704" };
        //public static string[] rigs = new[] { "772" };

        private static CriticalTagNestedTagExpressionGenerator GenerateTagForRig(Blackgold_PRODEntities ctxt, string rigNumber, string tagName, string controlSystem)
        {
            SimpleTagExpressionGenerator.TagTypeEnum tagType = SimpleTagExpressionGenerator.TagTypeEnum.Float;
            if (tagName.EndsWith("Set Point Enabled")) 
            {
                tagType = SimpleTagExpressionGenerator.TagTypeEnum.Boolean;
            }
            else if (tagName == "Rig State") 
            {
                tagType = SimpleTagExpressionGenerator.TagTypeEnum.Integer;
            }

            CriticalTagNestedTagExpressionGenerator ctnteg = new CriticalTagNestedTagExpressionGenerator(tagName, tagType);

            string[] controlSystems = controlSystem.Split('/'); // handles LTI/IEC case

            IEnumerable<UniversalTagPathSelector> utpsList = ctxt.UniversalTagPathSelectors.Where(utps => utps.tag == tagName && (utps.rig == null || utps.rig == rigNumber) && (controlSystems.Contains(utps.control_system)));
            //IEnumerable<UniversalTagPathSelector> utpsList = ctxt.UniversalTagPathSelectors.Where(utps => utps.tag == tagName && (utps.rig == null || utps.rig == rigNumber) && (utps.control_system == controlSystem));
            var grps = utpsList.GroupBy(utps => new { control_system = utps.control_system, attempt_ordinal = utps.attempt_ordinal });
            foreach (var grp in grps.OrderBy(g => g.Key.control_system).ThenBy(g => g.Key.attempt_ordinal))
            {
                UniversalTagPathSelector utps = grp.Any(u => u.rig != null) ? grp.First(u => u.rig != null) : grp.First();
                if (!ctnteg.TagPathExpressions.Any(ctpe => ctpe.TagPath == utps.tag_path))
                {
                    CriticalTagPathExpression expr = new CriticalTagPathExpression();
                    //new TagPathExpression(utps.tag_path, utps.uom_factor)
                    expr.FactorOrInterceptFirst = utps.uom_which_applies_first;
                    expr.Intercept = utps.uom_intercept;
                    expr.IsInverse = utps.is_inverse;
                    expr.Multiplier = utps.uom_factor;
                    expr.TagPath = utps.tag_path;
                    ctnteg.Add(expr);
                }

            }
            return ctnteg;
        }

        public static IEnumerable<CriticalTagNestedTagExpressionGenerator> GenerateTagsForRig(string rigNumber)
        {
            Blackgold_PRODEntities bgCtxt = new Blackgold_PRODEntities();
            // do this to open the connection
            string controlSystem = null;
            using (IDbConnection dbConn = bgCtxt.Database.Connection)
            {
                dbConn.Open();
                IDbCommand query = dbConn.CreateCommand();
                query.CommandText = "select max(control_system) from BLACKGOLD_IGNRPT.IgnitionEnterpriseReporting.dbo.sqlth_drv where nice_name='" + rigNumber + "'";
                Object o = query.ExecuteScalar();
                if (DBNull.Value.Equals(o))
                {
                    throw new Exception("No control system known for rig " + rigNumber);
                }
                controlSystem = o.ToString();
                controlSystem = controlSystem == "ACE" ? "Edge" : controlSystem;
                dbConn.Close();
            }
            List<CriticalTagNestedTagExpressionGenerator> ret = new List<CriticalTagNestedTagExpressionGenerator>();
            bgCtxt = new Blackgold_PRODEntities();
            foreach (string tagName in bgCtxt.UniversalTagPathSelectors.Select(utps => utps.tag).Distinct())
            {
                ret.Add(GenerateTagForRig(bgCtxt, rigNumber, tagName, controlSystem));
            }
            return ret;
        }

        private static string GetTheTypeForTheTag(string tag)
        {
            if (tag == "Rig State")
            {
                return "string";
            }
            else if (tag.EndsWith(" Active"))
            {
                return "boolean";
            }
            else
            {
                return "numeric";
            }
        }

        private static string GetTheUomForTheTag(string tag)
        {
            if (GetTheTypeForTheTag(tag) == "numeric")
            {
                if (tag == "Hole Depth" || tag == "Bit Depth" || tag == "Block Height")
                {
                    return "m";
                }
                if (tag == "Hookload" || tag == "WOB Set Point" || tag.StartsWith("Weight On Bit"))
                {
                    return "dan";
                }
                if (tag == "SPP" || tag.StartsWith("Differential"))
                {
                    return "kpa";
                }
                if (tag.Contains("Torque"))
                {
                    return "knm";
                }
                if (tag.EndsWith("Degrees"))
                {
                    return "deg";
                }
                if (tag.EndsWith("Revolutions"))
                {
                    return "rev";
                }
                if (tag == "ROP" || tag == "ROP Set Point")
                {
                    return "m/hr";
                }
                if (tag.EndsWith("RPM"))
                {
                    return "rpm";
                }
                if (tag.EndsWith("SPM"))
                {
                    return "spm";
                }
                if (tag.EndsWith("KW"))
                {
                    return "kw";
                }

            }
            else
            {
                return "na";
            }

            throw new Exception("Don't know the uom!");
        }

        private static string GetTheJson()
        {
            StringBuilder sb = new StringBuilder("{");
            sb.Append("\"rtVarConfigMap\":[");
            Blackgold_PRODEntities bgCtxt = new Blackgold_PRODEntities();

            IEnumerable<UniversalTagPathSelector> utpsList = bgCtxt.UniversalTagPathSelectors.ToList();
            List<object> toWrite = new List<object>();

            bool firstTag = true;
            foreach (string tagName in utpsList.Select(utps => utps.tag).Distinct())
            {
                string tag = tagName.Replace("Enabled", "Active");
                if (tag.StartsWith("WOB"))
                {
                    tag = tag.Replace("WOB", "Weight On Bit");
                }
                if (tag.StartsWith("QO "))
                {
                    tag = tag.Replace("QO ", "");
                }
                if (tag.StartsWith("Torque"))
                {
                    tag = "Top Drive " + tag;
                }
                if (tag == "Standpipe Pressure")
                {
                    tag = "SPP";
                }
                if (tag == "Torque")
                {
                    tag = "Top Drive Torque";
                }
                if (!firstTag)
                {
                    sb.Append(",");
                }
                sb.AppendFormat("{{\"VariableName\": \"{0}\",", tag);
                sb.AppendFormat("\"Type\": \"{0}\",", GetTheTypeForTheTag(tag));
                sb.AppendFormat("\"Uom\": \"{0}\",", GetTheUomForTheTag(tag));
                sb.Append("\"TagCandidates\":[");
                //sb.Append("1,2,3");
                IEnumerable<UniversalTagPathSelector> utpsForTag = utpsList.Where(utps => utps.tag == tagName).OrderBy(utps => utps.control_system).ThenBy(utps => utps.attempt_ordinal);

                bool firstCandidate = true;
                HashSet<string> tagPathCandidates = new HashSet<string>();
                foreach (UniversalTagPathSelector utps in utpsForTag)
                {
                    if (!firstCandidate)
                    {
                        sb.Append(",");
                    }
                    if (!tagPathCandidates.Contains(utps.tag_path))
                    {
                        StringBuilder candidateJson = new StringBuilder();
                        candidateJson.Append("{\"SourceSystem\": \"IgnitionEnterpriseReporting\",");
                        candidateJson.AppendFormat("\"TagPath\": \"{0}\",", utps.tag_path);
                        candidateJson.AppendFormat("\"Multiplier\": {0} }}", utps.uom_factor);
                        //sb.Append("{\"foo\":1}");
                        sb.Append(candidateJson.ToString());
                        tagPathCandidates.Add(utps.tag_path);
                    }
                    firstCandidate = false;
                }
                sb.Append("]}");
                firstTag = false;
            }
            sb.Append("]}");
            return sb.ToString();
        }

        private static bool DoThisRig(string rigNumber)
        {
            return !(rigNumber == "147" || rigNumber == "162" || rigNumber == "T054" || rigNumber == "770");
        }

        static void Main(string[] args)
        {
            //using (StreamWriter sw = new StreamWriter("c:\\temp\\the.json"))
            //{
            //    sw.WriteLine(GetTheJson());
            //}

            foreach (string rig in rigs.Where(r => DoThisRig(r)))
            {
                IEnumerable<CriticalTagNestedTagExpressionGenerator> tags = GenerateTagsForRig(rig);

                using (StreamWriter w = new StreamWriter(string.Format(@"C:\temp\SQLLiteCriticalTagGen\SQLiteTagGen_rig{0}.sql", rig)))
                {
                    //w.WriteLine(CriticalTagsStringTemplates.SqlLiteScanClassCreateSql);
                    w.WriteLine(string.Format(CriticalTagsStringTemplates.SqlLiteTagFolderCreateSql, rig));
                    foreach (SimpleTagExpressionGenerator tagGen in tags.Where(t => t.TagPathExpressions.Any()))
                    {
                        w.WriteLine(string.Format(CriticalTagsStringTemplates.SqlLiteTagGenSql, tagGen.TagName, rig, tagGen.GetTagExpression(), tagGen.TagTypeString));
                    }
                }
            }

        }
    }
}
