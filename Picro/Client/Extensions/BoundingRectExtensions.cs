using Picro.Client.DataTypes.InteropObjects;

namespace Picro.Client.Extensions
{
    public static class BoundingRectExtensions
    {
        public static bool IsWithinXBounds(this BoundingRect boundingRect, double xPos)
        {
            return xPos >= boundingRect.Left && xPos <= boundingRect.Right;
        }
    }
}