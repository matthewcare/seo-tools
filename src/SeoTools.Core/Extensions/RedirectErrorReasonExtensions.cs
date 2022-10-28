using System;
using SeoTools.Core.Services.Redirects;

namespace SeoTools.Core.Extensions
{
	public static class RedirectErrorReasonExtensions
	{
		public static string ToDisplay(this RedirectErrorReason reason)
        {
            return reason switch
            {
                RedirectErrorReason.IncorrectFormat => "This line is in the incorrect format",
                RedirectErrorReason.InvalidFromUri => "The \"from\" URI is invalid",
                RedirectErrorReason.InvalidToUri => "The \"to\" URI is invalid",
                RedirectErrorReason.InvalidFromExtension => "The \"from\" URI has an invalid extension",
                RedirectErrorReason.InvalidToExtension => "The \"to\" URI has an invalid extension",
                RedirectErrorReason.InvalidDestination => "The destination URI is invalid",
                RedirectErrorReason.DuplicateDetected => "This redirect exists more than once",
                RedirectErrorReason.RedirectingToSelf => "This is redirecting to itself",
                _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, null)
            };
        }
	}
}