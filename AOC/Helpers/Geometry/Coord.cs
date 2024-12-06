using System.Diagnostics.CodeAnalysis;

namespace rsmith985.AOC;

public struct Coord
{
    public long X{get;set;}
    public long Y{get;set;}

    public Coord(long x, long y)
    {
        this.X = x;
        this.Y = y;
    }

    public Coord Plus(long x, long y) => new Coord(this.X + x, this.Y + y);
    public Coord Plus(Coord p2) => new Coord(this.X + p2.X, this.Y + p2.Y);

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        return obj is Coord coord && this.X == coord.X && this.Y == coord.Y;
    }
    public override int GetHashCode()
    {
        return (this.X, this.Y).GetHashCode();
    }
}
