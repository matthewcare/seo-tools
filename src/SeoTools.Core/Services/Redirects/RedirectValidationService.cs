using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeoTools.Core.Services.Redirects
{
	public class RedirectValidationService : IRedirectValidationService
	{
		private readonly RedirectValidatorConfig _configuration;

		public RedirectValidationService(RedirectValidatorConfig configuration)
		{
			_configuration = configuration;
		}

		public RedirectValidationResult Validate(string redirect)
		{
			var result = new RedirectValidationResult
			{
				Original = redirect,
				Success = true
			};
			
			var keyValuePair = redirect.Split(',');

			if (keyValuePair.Length != 2)
			{
				return new RedirectValidationResult
				{
					Success = false,
					ErrorReasons = new List<RedirectErrorReason> { RedirectErrorReason.IncorrectFormat }
				};
			}

			if (!IsValidUri(keyValuePair[0], out var fromRelativeUri))
			{
				result.Success = false;
				result.ErrorReasons.Add(RedirectErrorReason.InvalidFromUri);
			}

			if (!IsValidUri(keyValuePair[1], out var toRelativeUri))
			{
				result.Success = false;
				result.ErrorReasons.Add(RedirectErrorReason.InvalidToUri);
			}

			result.FromUri = fromRelativeUri == "/" ? fromRelativeUri : fromRelativeUri.TrimEnd('/');
			result.ToUri = toRelativeUri == "/" ? toRelativeUri : toRelativeUri.TrimEnd('/');

			if (result.FromUri == result.ToUri)
			{
				result.Success = false;
				result.ErrorReasons.Add(RedirectErrorReason.RedirectingToSelf);
			}

			if (!IsValidFromExtension(result.FromUri, out _))
			{
				result.Success = false;
				result.ErrorReasons.Add(RedirectErrorReason.InvalidFromExtension);
			}

			if (!IsValidToExtension(result.ToUri, out _))
			{
				result.Success = false;
				result.ErrorReasons.Add(RedirectErrorReason.InvalidToExtension);
			}

			if (_configuration.InvalidDestinations.Contains(result.ToUri))
			{
				result.Success = false;
				result.ErrorReasons.Add(RedirectErrorReason.InvalidDestination);
			}

			return result;
		}

		public IEnumerable<RedirectValidationResult> Validate(List<string> redirects)
		{
			var validated = redirects.Select((x, i) =>
			{
				var validated = Validate(x);
				validated.LineNumber = i + 1;
				return validated;
			}).ToList();

			CheckForDuplicates(validated);
			return validated;
		}

		public void CheckForDuplicates(List<RedirectValidationResult> redirects)
		{
			var multipleKeyGroups = redirects
				.GroupBy(x => x.FromUri)
				.Where(x => x.Count() > 1);

			foreach (var group in multipleKeyGroups)
			{
				foreach (var item in redirects.Where(x => x.FromUri == group.Key))
				{
					item.Success = false;
					item.ErrorReasons.Add(RedirectErrorReason.DuplicateDetected);
				}
			}
		}

		public IEnumerable<string> GetSuggestions(List<RedirectValidationResult> redirects)
		{
			redirects ??= new List<RedirectValidationResult>();
			var commonPaths = redirects.Where(x => x.Success)
				.GroupBy(x => new
				{
					From = x.FromUri.Split('/', StringSplitOptions.RemoveEmptyEntries)[0],
					To = x.ToUri
				}).Where(x => x.Count() > 10);

			foreach (var commonPath in commonPaths)
			{
				yield return $"{commonPath.Key.From} has {commonPath.Count()} entries redirecting to {commonPath.Key.To}. Suggest adding a custom rule";
			}
		}

		public bool IsValidUri(string uri, out string relativeUri)
		{
			relativeUri = string.Empty;
			if (Uri.TryCreate(uri, UriKind.Absolute, out var absolute))
			{
				relativeUri = absolute.AbsolutePath;
				return true;
			}

			if (Uri.TryCreate(uri, UriKind.Relative, out _))
			{
				relativeUri = uri;
				return true;
			}

			return false;
		}

		public bool IsValidFromExtension(string input, out string extension)
		{
			extension = Path.GetExtension(input);
			return string.IsNullOrEmpty(extension) || _configuration.ValidFromExtensions.Contains(extension);
		}

		public bool IsValidToExtension(string input, out string extension)
		{
			extension = Path.GetExtension(input);
			return string.IsNullOrEmpty(extension) || _configuration.ValidToExtensions.Contains(extension);
		}
	}
}