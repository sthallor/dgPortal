using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IgnitionSQLiteTagGen;

namespace BlackGoldData
{
    public class CriticalTagNestedTagExpressionGenerator : SimpleTagExpressionGenerator
    {
        public List<CriticalTagPathExpression> TagPathExpressions { get; set; }
        public CriticalTagNestedTagExpressionGenerator(string tagName, TagTypeEnum tagType)
            : base(tagName, tagType)
        {
            TagPathExpressions = new List<CriticalTagPathExpression>();
        }

        public void Add(CriticalTagPathExpression tagPathExpression)
        {
            TagPathExpressions.Add(tagPathExpression);
        }

        public override string GetTagExpression()
        {
            return GetTagExpression(TagPathExpressions.First(), TagPathExpressions.Skip(1));
        }

        private string GetTagExpression(CriticalTagPathExpression expression, IEnumerable<CriticalTagPathExpression> remainingExpressions)
        {
            if (remainingExpressions.Any())
            {
                return string.Format("if(isnull({{[~]{0}}}),{1},{2})", expression.TagPath, GetTagExpression(remainingExpressions.First(), remainingExpressions.Skip(1)), GenerateFormula(expression));
            }
            else
            {
                return GenerateFormula(expression);
            }
        }


        public static string GenerateFormula(CriticalTagPathExpression expr)
        {
            double multiplierToUse = expr.IsInverse ? (1 / expr.Multiplier) : expr.Multiplier;

            if (expr.Intercept == 0.0)
            {
                if (multiplierToUse == 1.0)
                {
                    return string.Format("{{[~]{0}}}", expr.TagPath);
                }
                else
                {
                    return string.Format("{{[~]{0}}}*{1}", expr.TagPath, multiplierToUse);
                }
            }
            else
            {
                if (multiplierToUse == 1.0)
                {
                    return string.Format("{{[~]{0}}}+{1}", expr.TagPath, expr.Intercept);
                }
                else
                {
                    if (expr.FactorOrInterceptFirst == "factor")
                    {
                        return string.Format("{{[~]{0}}}*{1}+{2}", expr.TagPath, multiplierToUse, expr.Intercept);
                    }
                    else
                    {
                        return string.Format("({{[~]{0}}}+{1})*{2}", expr.TagPath, expr.Intercept, multiplierToUse);
                    }
                }
            }
        }
    }

    public class CriticalTagPathExpression
    {
        public string TagPath { get; set; }
        public double Multiplier { get; set; }
        public bool IsInverse { get; set; }
        public double Intercept { get; set; }
        public string FactorOrInterceptFirst { get; set; }

        public CriticalTagPathExpression() { }

        public CriticalTagPathExpression(string tagPath, double multiplier, bool isInverse, double intercept, string factorOrInterceptFirst)
        {
            TagPath = tagPath;
            Multiplier = multiplier;
            IsInverse = isInverse;
            Intercept = intercept;
            FactorOrInterceptFirst = factorOrInterceptFirst;
        }
    }
}
