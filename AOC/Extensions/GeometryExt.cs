using System.Drawing;
using Emgu.CV.ML;

namespace rsmith985.AOC.Y2023;

public static class GeometryExt
{
    public static Point ToPoint(this Size s) => new Point(s.Width, s.Height);
    public static PointF ToPoint(this SizeF s) => new PointF(s.Width, s.Height);
    public static Size ToSize(this Point s) => new Size(s.X, s.Y);
    public static SizeF ToSize(this PointF s) => new SizeF(s.X, s.Y);

    #region Point
    public static Point Plus(this Point p, int num) => new Point(p.X+num, p.Y+num);
    public static Point Plus(this Point p, int x, int y) => new Point(p.X+x, p.Y+y);
    public static Point Plus(this Point p, Point p2) => new Point(p.X+p2.X, p.Y+p2.Y);
    public static Point Mult(this Point p, int num) => new Point(p.X*num, p.Y*num);
    public static Point Mult(this Point p, Point p2) => new Point(p.X*p2.X, p.Y*p2.Y);
    public static Point Divide(this Point p, int num) => new Point(p.X/num, p.Y/num);
    public static Point Divide(this Point p, Point p2) => new Point(p.X/p.X, p.Y/p.Y);
    public static Point Invert(this Point p) => p.Mult(-1);

    public static PointF Plus(this PointF p, float num) => new PointF(p.X+num, p.Y+num);
    public static PointF Plus(this PointF p, float x, float y) => new PointF(p.X+x, p.Y+y);
    public static PointF Plus(this PointF p, PointF p2) => new PointF(p.X+p2.X, p.Y+p2.Y);
    public static PointF Mult(this PointF p, float num) => new PointF(p.X*num, p.Y*num);
    public static PointF Mult(this PointF p, PointF p2) => new PointF(p.X*p2.X, p.Y*p2.Y);
    public static PointF Divide(this PointF p, float num) => new PointF(p.X/num, p.Y/num);
    public static PointF Divide(this PointF p, PointF p2) => new PointF(p.X/p.X, p.Y/p.Y);
    public static PointF Invert(this PointF p) => p.Mult(-1);

    public static PointF ToFloat(this Point p) => new PointF(p.X, p.Y);
    public static Point ToNonFloat(this PointF p) => new Point((int)Math.Round(p.X), (int)Math.Round(p.Y));

    public static (int x, int y) ToTuple(this Point p) => (p.X, p.Y);
    public static (float x, float y) ToTuple(this PointF p) => (p.X, p.Y);

    public static Point Min(this Point p1, Point p2) => new Point(p1.X < p2.X ? p1.X : p2.X, p1.Y < p2.Y ? p1.Y : p2.Y);
    public static Point Max(this Point p1, Point p2) => new Point(p1.X > p2.X ? p1.X : p2.X, p1.Y > p2.Y ? p1.Y : p2.Y);
    public static PointF Min(this PointF p1, PointF p2) => new PointF(p1.X < p2.X ? p1.X : p2.X, p1.Y < p2.Y ? p1.Y : p2.Y);
    public static PointF Max(this PointF p1, PointF p2) => new PointF(p1.X > p2.X ? p1.X : p2.X, p1.Y > p2.Y ? p1.Y : p2.Y);
    #endregion

    #region Size
    public static Size Plus(this Size size, int num) => new Size(size.Width+num, size.Height+num);
    public static Size Plus(this Size size, Size s) => new Size(size.Width+s.Width, size.Height+s.Height);
    public static Size Mult(this Size size, int num) => new Size(size.Width*num, size.Height*num);
    public static Size Mult(this Size size, Size s) => new Size(size.Width*s.Width, size.Height*s.Height);
    public static Size Divide(this Size p, int num) => new Size(p.Width/num, p.Height/num);
    public static Size Divide(this Size p, Size p2) => new Size(p.Width/p.Width, p.Height/p.Height);
    public static Size Invert(this Size p) => p.Mult(-1);

    public static SizeF Plus(this SizeF size, float num) => new SizeF(size.Width+num, size.Height+num);
    public static SizeF Plus(this SizeF size, SizeF s) => new SizeF(size.Width+s.Width, size.Height+s.Height);
    public static SizeF Mult(this SizeF size, float num) => new SizeF(size.Width*num, size.Height*num);
    public static SizeF Mult(this SizeF size, SizeF s) => new SizeF(size.Width*s.Width, size.Height*s.Height);
    public static SizeF Divide(this SizeF p, float num) => new SizeF(p.Width/num, p.Height/num);
    public static SizeF Divide(this SizeF p, SizeF p2) => new SizeF(p.Width/p.Width, p.Height/p.Height);
    public static SizeF Invert(this SizeF p) => p.Mult(-1);

    public static SizeF ToFloat(this Size p) => new SizeF(p.Width, p.Height);
    public static Size ToNonFloat(this SizeF p) => new Size((int)Math.Round(p.Width), (int)Math.Round(p.Height));
    #endregion

    #region  Polygon
    public static bool PolygonContains(this IList<PointF> poly, PointF p)
    {
        var rv = false;
        if(poly.Count <= 2) return false;

        var prev = poly[poly.Count - 2];
        var curr = poly[poly.Count - 1];

        for(int i = 0; i < poly.Count; i++)
        {
            var p1 = curr.X > prev.X ? prev : curr;
            var p2 = curr.X > prev.X ? curr : prev;

            if((curr.X < p.X) == (p.X <= prev.X) && (p.Y - p1.Y) * (p2.X - p1.X) < (p2.Y - p1.Y) * (p.X - p1.X))
                rv = !rv;
            
            prev = curr;
            curr = poly[i];
        }

        return rv;
    }
    public static bool PolygonContains(this IList<Point> poly, Point p)
    {
        var rv = false;
        if(poly.Count <= 2) return false;

        var prev = poly[poly.Count - 2];
        var curr = poly[poly.Count - 1];

        for(int i = 0; i < poly.Count; i++)
        {
            var p1 = curr.X > prev.X ? prev : curr;
            var p2 = curr.X > prev.X ? curr : prev;

            if((curr.X < p.X) == (p.X <= prev.X) && (p.Y - p1.Y) * (p2.X - p1.X) < (p2.Y - p1.Y) * (p.X - p1.X))
                rv = !rv;
            
            prev = curr;
            curr = poly[i];
        }

        return rv;
    }
    #endregion

    #region Bounding Rectangle
    public static RectangleF GetBoundingRect(this IEnumerable<PointF> points)
    {
        var minX = float.MaxValue;
        var minY = float.MaxValue;
        var maxX = float.MinValue;
        var maxY = float.MinValue;

        foreach(var p in points)
        {
            if(p.X < minX) minX = p.X;
            if(p.X > maxX) maxX = p.X;
            if(p.Y < minY) minY = p.Y;
            if(p.Y > maxY) maxY = p.Y;
        }
        return new RectangleF(minX, minY, maxX-minX, maxY-minY);
    }
    public static Rectangle GetBoundingRect(this IEnumerable<Point> points)
    {
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;

        foreach(var p in points)
        {
            if(p.X < minX) minX = p.X;
            if(p.X > maxX) maxX = p.X;
            if(p.Y < minY) minY = p.Y;
            if(p.Y > maxY) maxY = p.Y;
        }
        return new Rectangle(minX, minY, maxX-minX, maxY-minY);
    }
    #endregion

    #region Direction
    public static Point GetNeighbor(this Point p, Direction d)
    {
        return d switch
        {
            Direction.N => p.Plus(0,-1),
            Direction.S => p.Plus(0,1),
            Direction.E => p.Plus(1, 0),
            Direction.W => p.Plus(-1, 0),
            Direction.NW => p.Plus(-1, -1),
            Direction.NE => p.Plus(1, -1),
            Direction.SW => p.Plus(-1, 1),
            Direction.SE => p.Plus(1, 1),
            _ => p
        };
    }
    #endregion
}
