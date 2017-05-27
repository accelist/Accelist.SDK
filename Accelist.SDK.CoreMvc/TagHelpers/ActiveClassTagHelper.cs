using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Accelist.SDK.CoreMvc.TagHelpers
{
    /// <summary>
    /// Tag helper for appending active CSS class for Bootstrap components. 
    /// </summary>
    [HtmlTargetElement(Attributes = "acl-active")]
    public class ActiveClassTagHelper : ClassAppendTagHelper
    {
        /// <summary>
        /// Captures acl-active tag attribute.
        /// </summary>
        public bool AclActive { set; get; }

        /// <summary>
        /// Appends active CSS class if ry-active property is set to true.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            this.ExecuteAppendLogic(output, AclActive, "active");
        }
    }
}
