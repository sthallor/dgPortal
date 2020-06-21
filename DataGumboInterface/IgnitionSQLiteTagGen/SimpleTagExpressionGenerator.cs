using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgnitionSQLiteTagGen
{
    public class SimpleTagExpressionGenerator
    {
        public enum TagTypeEnum
        {
            String, Integer, Float, Boolean
        }

        public String TagName { get; set;}
        public String TagExpression { get; set; }
        public TagTypeEnum TagType { get; set; }
        public string TagTypeString
        {
            get
            {
                if (TagType == TagTypeEnum.String)
                {
                    return "String";
                }
                if (TagType == TagTypeEnum.Integer)
                {
                    return "Int4";
                }
                if (TagType == TagTypeEnum.Float)
                {
                    return "Float4";
                }
                if (TagType == TagTypeEnum.Boolean)
                {
                    return "Boolean";
                }
                throw new Exception("Damnit!");
            }
        }
        public SimpleTagExpressionGenerator(string tagName, TagTypeEnum tagType) : this(tagName, tagType, null) { }
        public SimpleTagExpressionGenerator(string tagName, TagTypeEnum tagType, string tagExpression)
        {
            TagName = tagName;
            TagType = tagType;
            TagExpression = tagExpression;
        }
        public virtual string GetTagExpression()
        {
            return TagExpression;
        }
    }
}
