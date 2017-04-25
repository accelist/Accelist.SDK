using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HttpUtil.TagHelpers
{
    /// <summary>
    /// Abstract tag helper for appending a class to existing tag class attribute. 
    /// </summary>
    public abstract class ClassAppendTagHelper : TagHelper
    {
        /// <summary>
        /// Captures class attribute of a tag.
        /// </summary>
        public string Class { set; get; }

        /// <summary>
        /// Append the specified class to existing class attribute, if it does not exist yet.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public string AppendClass(string className)
        {
            if (string.IsNullOrEmpty(this.Class))
            {
                return className;
            }
            
            if (Class.Split(' ').Contains(className))
            {
                return this.Class;
            }

            return $"{this.Class} {className}";
        }

        /// <summary>
        /// Append the specified class if doAppend is set to true. Otherwise, set the original class attribute back.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="doAppend"></param>
        /// <param name="className"></param>
        public void ExecuteAppendLogic(TagHelperOutput output, bool doAppend, string className)
        {
            this.Class = this.Class.Trim();

            if (doAppend)
            {
                var result = AppendClass(className);
                output.Attributes.SetAttribute("class", result);
            }
            else
            {
                if (string.IsNullOrEmpty(this.Class) == false)
                {
                    output.Attributes.SetAttribute("class", this.Class);
                }
            }
        }
    }
}
