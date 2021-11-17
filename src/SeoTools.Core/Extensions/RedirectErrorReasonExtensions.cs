using System;
using SeoTools.Core.Services.Redirects;

namespace SeoTools.Core.Extensions
{
	public static class RedirectErrorReasonExtensions
	{
		public static string ToDisplay(this RedirectErrorReason reason)
		{
			switch (reason)
			{
				case RedirectErrorReason.IncorrectFormat:
					return "This line is in the incorrect format";
				case RedirectErrorReason.InvalidFromUri:
					return "The \"from\" URI is invalid";
				case RedirectErrorReason.InvalidToUri:
					return "The \"to\" URI is invalid";
				case RedirectErrorReason.InvalidFromExtension:
					return "The \"from\" URI has an invalid extension";
				case RedirectErrorReason.InvalidToExtension:
					return "The \"to\" URI has an invalid extension";
				case RedirectErrorReason.InvalidDestination:
					return "The destination URI is invalid";
				case RedirectErrorReason.DuplicateDetected:
					return "This redirect exists more than once";
				case RedirectErrorReason.RedirectingToSelf:
					return "This is redirecting to itself";
				default:
					throw new ArgumentOutOfRangeException(nameof(reason), reason, null);
			}
		}
	}
}