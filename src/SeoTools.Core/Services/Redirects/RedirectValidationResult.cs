using System.Collections.Generic;

namespace SeoTools.Core.Services.Redirects
{
	public class RedirectValidationResult
	{
		public string Original { get; set; }
		public int LineNumber { get; set; }
		public bool Success { get; set; }
		public string FromUri { get; set; }
		public string ToUri { get; set; }
		public List<RedirectErrorReason> ErrorReasons { get; set; } = new ();
	}
}