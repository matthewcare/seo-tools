using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SeoTools.Core.Services.Redirects;

namespace SeoTools.Web.Components
{
	public class RedirectValidator : ComponentBase
	{
		[Inject]
		protected IRedirectValidationService RedirectValidationService { get; set; }

		[Inject]
		protected IRedirectConfigGenerator RedirectConfigGenerator { get; set; }

		public string InputString { get; set; }
		public List<RedirectValidationResult> Output { get; set; } = new ();
		public string DotNetOutput { get; set; }
		public List<string> Suggestions { get; set; } = new ();
		public bool OnlyShowErrors { get; set; }

		public void Process()
		{
			if (string.IsNullOrWhiteSpace(InputString))
			{
				return;
			}

			var lines = InputString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
			ProcessLines(lines);
		}

		private void ProcessLines(List<string> input)
		{
			var validatedRedirects = RedirectValidationService.Validate(input).ToList();
			var suggestions = RedirectValidationService.GetSuggestions(validatedRedirects).ToList();

			Output = validatedRedirects;
			Suggestions = suggestions;
		}

		public void RemoveErrors()
		{
			var errorLines = Output.Where(x => !x.Success).Select(x => x.Original);
			var lines = InputString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
			lines.RemoveAll(x => errorLines.Contains(x));
			InputString = string.Join(Environment.NewLine, lines);
			Process();
		}

		public async Task DotNetDownload()
		{
			await using var ms = new MemoryStream();
			await using var sw = new StreamWriter(ms);
			await RedirectConfigGenerator.GenerateAsync(sw, Output);
			await sw.FlushAsync();

			ms.Position = 0;
			var reader = new StreamReader(ms);
			var output = await reader.ReadToEndAsync();

			DotNetOutput = output;
		}
	}
}