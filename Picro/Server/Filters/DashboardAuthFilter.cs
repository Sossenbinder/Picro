using Hangfire.Dashboard;

namespace Picro.Server.Filters
{
	public class DashboardAuthFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			var key = context.Request.GetQuery("key");

			return true;
		}
	}
}