using System.Collections.Generic;

namespace SeoTools.Core.Services.Redirects
{
	public class RedirectValidatorConfig
	{
		public List<string> ValidFromExtensions { get; set; }
		public List<string> ValidToExtensions { get; set; }
		public List<string> InvalidDestinations { get; set; }
	}
}