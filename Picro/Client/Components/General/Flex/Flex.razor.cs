using Microsoft.AspNetCore.Components;
using Picro.Client.Components.General.Flex.Enums;
using Picro.Client.Utils;
using System;
using System.Threading.Tasks;

namespace Picro.Client.Components.General.Flex
{
	public partial class Flex : ComponentBase
	{
		#region Parameters

		[Parameter]
		public string? Class { get; init; }

		[Parameter]
		public string? Style { get; init; }

		[Parameter]
		public Func<Task>? OnClick { get; init; }

		[Parameter]
		public ElementReference Ref { get; set; }

		[Parameter]
		public FlexDirection Direction { get; init; } = FlexDirection.Row;

		[Parameter]
		public FlexWrap? Wrap { get; init; }

		[Parameter]
		public FlexSpace? Space { get; init; }

		[Parameter]
		public FlexAlign? MainAlign { get; init; }

		[Parameter]
		public FlexAlign? MainAlignSelf { get; init; }

		[Parameter]
		public FlexAlign? CrossAlign { get; init; }

		[Parameter]
		public FlexAlign? CrossAlignSelf { get; init; }

		[Parameter]
		public RenderFragment? ChildContent { get; init; }

		[Parameter]
		public string Id { get; init; }

		[Parameter]
		public string? Title { get; init; }

		#endregion Parameters

		private string _combinedClass { get; set; } = "";

		protected override void OnParametersSet()
		{
			// Build new definite class set
			var cb = new ClassBuilder();

			if (Class != null)
			{
				cb.AddClass(Class);
			}

			cb.AddClass("Flex")
				.AddClass($"Flex{Direction}");

			if (Wrap != null)
			{
				cb.AddClass($"Flex{Wrap}");
			}

			if (MainAlign != null)
			{
				cb.AddClass($"FlexMain{MainAlign}");
			}

			if (MainAlignSelf != null)
			{
				cb.AddClass($"FlexMain{MainAlignSelf}Self");
			}

			if (Space != null)
			{
				cb.AddClass($"Flex{Space}");
			}

			if (CrossAlign != null)
			{
				cb.AddClass($"FlexCross{CrossAlign}");
			}

			if (CrossAlignSelf != null)
			{
				cb.AddClass($"FlexCross{CrossAlignSelf}Self");
			}

			_combinedClass = cb.ToString();

			base.OnParametersSet();
		}
	}
}