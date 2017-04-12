using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HttpUtil.TagHelpers
{
    [HtmlTargetElement("li", Attributes = "ry-active")]
    public class ActiveTagHelper : TagHelper
    {
        public bool RyActive { set; get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (RyActive)
            {
                output.Attributes.SetAttribute("class", "active");
            }
        }
    }
}
