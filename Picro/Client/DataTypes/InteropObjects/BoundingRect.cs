namespace Picro.Client.DataTypes.InteropObjects
{
	public class BoundingRect
	{
		public float X { get; set; }

		public float Y { get; set; }

		public float Width { get; set; }

		public float Height { get; set; }

		public float Top { get; set; }

		public float Bottom { get; set; }

		public float Left { get; set; }

		public float Right { get; set; }

		public void ShiftRight(int x)
		{
			X += x;
			Left += x;
			Right += x;
		}
	}
}