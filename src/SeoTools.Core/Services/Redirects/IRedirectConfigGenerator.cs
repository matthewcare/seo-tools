using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SeoTools.Core.Services.Redirects
{
	public interface IRedirectConfigGenerator
	{
		Task GenerateAsync(StreamWriter streamWriter, IEnumerable<RedirectValidationResult> redirects);
	}
}