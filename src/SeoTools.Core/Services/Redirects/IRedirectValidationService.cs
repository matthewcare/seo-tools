using System.Collections.Generic;

namespace SeoTools.Core.Services.Redirects
{
	public interface IRedirectValidationService
	{
		RedirectValidationResult Validate(string redirect);
		IEnumerable<RedirectValidationResult> Validate(List<string> redirects);
		void CheckForDuplicates(List<RedirectValidationResult> redirects);
		IEnumerable<string> GetSuggestions(List<RedirectValidationResult> redirects);
		bool IsValidUri(string uri, out string relativeUri);
		bool IsValidFromExtension(string input, out string extension);
		bool IsValidToExtension(string input, out string extension);
	}
}