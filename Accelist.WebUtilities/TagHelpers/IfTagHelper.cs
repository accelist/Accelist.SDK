using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Accelist.WebUtilities.TagHelpers
{
    /// <summary>
    /// Tag helper for conditionally rendering a tag.
    /// </summary>
    [HtmlTargetElement(Attributes = "ry-if")]
    public class IfTagHelper : TagHelper
    {
        /// <summary>
        /// Captures the ry-if tag attribute.
        /// </summary>
        public bool RyIf { set; get; }

        /// <summary>
        /// Renders the tag only if the ry-if attribute is set to false.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (RyIf == false)
            {
                output.SuppressOutput();
            }
        }
    }
}
