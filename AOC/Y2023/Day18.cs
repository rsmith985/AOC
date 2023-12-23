using System.Drawing;
using System.Formats.Asn1;

namespace rsmith985.AOC.Y2023;

public class Day18 : Day
{
    private List<(Direction dir, int len, Direction dir2, long len2)> _input;

    public override void Init()
    {
        _input = new List<(Direction, int, Direction, long)>();
        foreach(var line in this.GetLines())
        {
            var parts = line.Split(' ');
            var dir = parts[0][0].ToDirection();
            var len = int.Parse(parts[1]);
            
            var txt = parts[2].Trim('(', ')', '#');
            var num = txt[^1] - 48;

            var dir2 = 
                num == 0 ? Direction.E :
                num == 1 ? Direction.S :
                num == 2 ? Direction.W :
                num == 3 ? Direction.N :
                throw new Exception();
            
            var len2 = Convert.ToInt32("0x" + txt[..^1], 16);

            _input.Add((dir, len, dir2, len2));
        }
    }

    public override object Part1()
    {
        var poly = getPoly();
        var perimeter = _input.Sum(i => i.len);
        var area = poly.PolygonArea();
        return perimeter/2 + area + 1;
    }

    public override object Part2()
    {
        var poly = getPoly2();
        var perimeter = _input.Sum(i => i.len2);
        var area = poly.PolygonArea();
        return perimeter/2 + area + 1;
    }

    private Point[] getPoly()
    {
        var poly = new Point[_input.Count];

        poly[0] = new Point(0, 0);
        for(int i = 0; i < _input.Count-1; i++)
        {
            var item = _input[i];
            poly[i+1] = poly[i].GetNeighbor(item.dir, item.len);
        }
        return poly;
    }
    private (long x, long y)[] getPoly2()
    {
        var poly = new (long x, long y)[_input.Count];

        poly[0] = (0, 0);
        for(int i = 0; i < _input.Count-1; i++)
        {
            var item = _input[i];
            poly[i+1] = poly[i].GetNeighbor(item.dir2, item.len2);
        }
        return poly;
    }

}
