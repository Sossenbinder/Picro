using Blazorise;
using Picro.Client.Utils;

namespace Picro.Client.Extensions
{
	public static class BaseComponentExtensions
	{
		public static RefScope BeginScope(this BaseComponent component)
		{
			return new(component);
		}
	}
}