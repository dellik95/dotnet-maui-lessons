namespace Tasker.Extensions
{
    public static class ColorExtensions
    {
        public static Color GetRandomColor(this Color c)
        {
            var rnd = new Random();
            var r = (byte)rnd.Next(1, 256);
            var g = (byte)rnd.Next(1, 256);
            var b = (byte)rnd.Next(1, 256);

            var color = Color.FromRgb(r, g, b);
            return color;
        }
    }
}
