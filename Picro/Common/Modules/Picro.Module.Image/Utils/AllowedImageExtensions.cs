using System.Collections.Generic;

namespace Picro.Module.Image.Utils
{
    public static class AllowedImageExtensions
    {
        public static List<string> ImageExtensions { get; } = new() { ".jpg", ".jpeg", ".png" };
    }
}