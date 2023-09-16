namespace ASP_LABS.Extensions
{
	public static class HttpExtension
	{
		public static bool IsAjaxRequest(this HttpRequest request)
		{
			if (request == null)
			{
				return false;
			}
			System.Diagnostics.Debug.WriteLine(request.Headers);
			return request.Headers["X-Requested-With"] == "XMLHttpRequest";

		}
	}
}
