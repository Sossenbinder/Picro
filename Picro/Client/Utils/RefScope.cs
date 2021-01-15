using System;
using Blazorise;

namespace Picro.Client.Utils
{
	public class RefScope : IDisposable
	{
		private readonly BaseComponent _component;

		private readonly string _class;

		public RefScope(BaseComponent component)
		{
			_component = component;
			_class = component.Class;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);

			_component.Class = _class;
		}
	}
}