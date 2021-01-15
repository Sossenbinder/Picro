using Picro.Common.Extensions;
using System;

namespace Picro.Client.Utils
{
	public class ClassBuilder
	{
		private string _classString;

		private readonly bool _ignoreNullClasses;

		public ClassBuilder(bool ignoreNullClasses = true)
		{
			_ignoreNullClasses = ignoreNullClasses;
			_classString = "";
		}

		public ClassBuilder AddClass(string? @class)
		{
			if (!_ignoreNullClasses && @class == null)
			{
				throw new ArgumentNullException(@class, "Class cannot be null");
			}

			_classString = _classString.IsNullOrEmpty() ? @class! : $"{_classString} {@class}";

			return this;
		}

		public override string ToString() => _classString;
	}
}