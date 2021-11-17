using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SeoTools.Core.Services.Redirects
{
	public class DotNetRedirectConfigGenerator: IRedirectConfigGenerator
	{
		public async Task GenerateAsync(StreamWriter streamWriter, IEnumerable<RedirectValidationResult> redirects)
		{
			await streamWriter.WriteLineAsync("<rewriteMaps>");
			await streamWriter.WriteLineAsync("\t<rewriteMap name=\"Redirects\">");

			foreach (var redirect in redirects)
			{
				await streamWriter.WriteLineAsync($"\t\t<add key=\"{redirect.FromUri}\" value=\"{redirect.ToUri}\" />");
			}

			await streamWriter.WriteLineAsync("\t</rewriteMaps>");
			await streamWriter.WriteLineAsync("</rewriteMaps>");
		}
	}
}