using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HttpUtil.TagHelpers
{
    [HtmlTargetElement(Attributes = "ry-if")]
    public class IfTagHelper : TagHelper
    {
        public bool RyIf { set; get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (RyIf == false)
            {
                output.SuppressOutput();
            }
        }
    }
}
