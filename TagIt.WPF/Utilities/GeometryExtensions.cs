using System.Windows;

namespace TagIt.WPF.Utilities
{
    public static class GeometryExtensions
    {
        public static double LengthSquared(this Point point) => point.X * point.X + point.Y * point.Y;

        
    }
}
