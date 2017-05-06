using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Accelist.SDK.CoreMvc.TagHelpers
{
    /// <summary>
    /// Tag helper for conditionally rendering a tag.
    /// </summary>
    [HtmlTargetElement(Attributes = "acl-if")]
    public class IfTagHelper : TagHelper
    {
        /// <summary>
        /// Captures the acl-if tag attribute.
        /// </summary>
        public bool AclIf { set; get; }

        /// <summary>
        /// Renders the tag only if the ry-if attribute is set to false.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AclIf == false)
            {
                output.SuppressOutput();
            }
        }
    }
}
