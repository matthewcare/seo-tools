namespace SeoTools.Core.Services.Redirects
{
	/// <summary>
	/// A redirect might not be valid for any of these reasons
	/// </summary>
	public enum RedirectErrorReason
	{
		/// <summary>
		/// If the supplied CSV is not in the correct format
		/// </summary>
		IncorrectFormat,

		/// <summary>
		/// If the from URI isn't validated as being a URL
		/// </summary>
		InvalidFromUri,

		/// <summary>
		/// If the to URI isn't validated as being a URL
		/// </summary>
		InvalidToUri,

		/// <summary>
		/// If the from URI has an extension that is not allowed
		/// </summary>
		InvalidFromExtension,

		/// <summary>
		/// If the to URI has an extension that is not allowed
		/// </summary>
		InvalidToExtension,

		/// <summary>
		/// If the to URI is to a destination that is not allowed
		/// </summary>
		InvalidDestination,

		/// <summary>
		/// If there are two redirects with the same from URI
		/// </summary>
		DuplicateDetected,

		/// <summary>
		/// If a redirect is redirecting to itself
		/// </summary>
		/// <remarks>
		///	Seen when trying to redirect /a-url/ to /a-url
		/// </remarks>
		RedirectingToSelf

	}
}