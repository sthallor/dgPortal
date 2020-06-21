using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgnitionSQLiteTagGen
{
    public class NestedTagExpressionGenerator : SimpleTagExpressionGenerator
    {
        public List<TagPathExpression> TagPathExpressions { get; set; }
        public NestedTagExpressionGenerator(string tagName, TagTypeEnum tagType)
            : base(tagName, tagType)
        {
            TagPathExpressions = new List<TagPathExpression>();
        }

        public void Add(TagPathExpression tagPathExpression)
        {
            TagPathExpressions.Add(tagPathExpression);
        }

        public override string GetTagExpression()
        {
            return GetTagExpression(TagPathExpressions.First(), TagPathExpressions.Skip(1));
        }

        private string GetTagExpression(TagPathExpression expression, IEnumerable<TagPathExpression> remainingExpressions)
        {
            if (remainingExpressions.Any())
            {
                return string.Format("if(isnull({{[~]{0}}}),{2},{{[~]{0}}}{1})", expression.TagPath, GenerateMultiplierExpressionPortion(expression.Multiplier), GetTagExpression(remainingExpressions.First(), remainingExpressions.Skip(1)));
            }
            else
            {
                return string.Format("{{[~]{0}}}{1}", expression.TagPath, GenerateMultiplierExpressionPortion(expression.Multiplier));
            }
        }

        public static string GenerateMultiplierExpressionPortion(double multiplier)
        {
            if (multiplier == 1.0)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("*{0}", multiplier);
            }
        }
    }

    public class TagPathExpression
    {
        public string TagPath { get; set; }
        public double Multiplier { get; set; }

        public TagPathExpression(string tagPath) : this(tagPath, 1.0) {}
        public TagPathExpression(string tagPath, double multiplier)
        {
            TagPath = tagPath;
            Multiplier = multiplier;
        }
    }
}
