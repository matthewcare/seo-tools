using Microsoft.AspNetCore.Components;
using MudBlazor;
using SeoTools.Core.Services.Redirects;

namespace SeoTools.Web.Components
{
	public class RedirectValidator : ComponentBase
    {
        private string? _inputString;

        [Inject]
        protected IRedirectValidationService RedirectValidationService { get; set; } = null!;

		[Inject]
		protected IRedirectConfigGenerator RedirectConfigGenerator { get; set; } = null!;

        public string? InputString
        {
            get => _inputString;
            set
            {
                _inputString = value;
				Process();
            }
        }

        public int TotalErrors => Output.Count(x => !x.Success);

        public List<RedirectValidationResult> Output { get; set; } = new();
		public string? DotNetOutput { get; set; }
		public List<string> Suggestions { get; set; } = new();
		public bool OnlyShowErrors { get; set; }

		private void Process()
		{
			if (string.IsNullOrWhiteSpace(InputString))
			{
				return;
			}

			var lines = InputString.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
			ProcessLines(lines);
		}

		private void ProcessLines(List<string> input)
		{
			var validatedRedirects = RedirectValidationService.Validate(input).ToList();
			var suggestions = RedirectValidationService.GetSuggestions(validatedRedirects).ToList();

			Output = validatedRedirects;
			Suggestions = suggestions;
		}

        public async Task<TableData<RedirectValidationResult>> GetTableDataAsync(TableState state)
        {
			var paged = Output.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
            var result = new TableData<RedirectValidationResult>
            {
                TotalItems = Output.Count,
                Items = paged
            };

            return await Task.FromResult(result);
        }

        public void OnSearch(string s)
        {
			
        }

		public void RemoveErrors()
		{
            if (string.IsNullOrEmpty(InputString))
            {
                return;
            }

			var errorLines = Output.Where(x => !x.Success).Select(x => x.Original);
			var lines = InputString.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
			lines.RemoveAll(x => errorLines.Contains(x));
			InputString = string.Join(Environment.NewLine, lines);
			Process();
		}

		public async Task GenerateOutput()
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