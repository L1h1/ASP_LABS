using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASP_LABS.Helpers
{
	public class PageHelper :TagHelper
	{
		



		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			var builder = new TagBuilder("pager");

		}



	}
}
