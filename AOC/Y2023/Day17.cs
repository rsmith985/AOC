using System.Data.SqlTypes;
using System.Drawing;

namespace rsmith985.AOC.Y2023;

public class Day17 : Day
{
    private Size _gridSize;
    private int W => _gridSize.Width;
    private int H => _gridSize.Height;

    private int[,] _input;
    private (int, int, bool) _start = (-1, -1, true);

    public override void Init()
    {
        _input = this.GetLines().To2DArray(i => i - 48);
        _gridSize = new Size(_input.GetLength(0), _input.GetLength(1));
    }
    public override object Part1()
    {
        var graph = makeGraph(1, 3);
        var djkstra = new Dijkstra<(int, int, bool)>(graph, _start);

        var d1 = djkstra.DistanceTo((this.W - 1, this.H - 1, true));
        var d2 = djkstra.DistanceTo((this.W - 1, this.H - 1, false));

        return Math.Min(d1, d2) - 1;
    }

    public override object Part2()
    {
        var graph = makeGraph(4, 10);
        var djkstra = new Dijkstra<(int, int, bool)>(graph, _start);

        var d1 = djkstra.DistanceTo((this.W - 1, this.H - 1, true));
        var d2 = djkstra.DistanceTo((this.W - 1, this.H - 1, false));

        return Math.Min(d1, d2) - 1;
    }

    private DirectedGraph<(int, int, bool)> makeGraph(int minDist, int maxDist)
    {
        var graph = new DirectedGraph<(int, int, bool)>();

        graph.Add(_start, (0, 0, true), 1);
        graph.Add(_start, (0, 0, false), 1);

        for(int x = 0; x < this.W; x++)
        {
            for(int y = 0; y < this.H; y++)
            {
                var sq1 = (x, y, false);
                for(int xx = minDist; xx <= maxDist; xx++)
                {
                    var forw = x + xx;
                    if(!outOfBounds(forw, y))
                    {
                        var sq = (forw, y, true);   
                        var w = getWeight(x, y, xx, 0, false);
                        graph.Add((sq1, sq), w);
                    }
                    var back = x - xx;
                    if(!outOfBounds(back, y))
                    {
                        var sq = (back, y, true);   
                        var w = getWeight(x, y, xx, 0, true);
                        graph.Add((sq1, sq), w);
                    }
                }
                var sq2 = (x, y, true);
                for(int yy = minDist; yy <= maxDist; yy++)
                {
                    var forw = y + yy;
                    if(!outOfBounds(forw, y))
                    {
                        var sq = (x, forw, false);   
                        var w = getWeight(x, y, 0, yy, false);
                        graph.Add((sq2, sq), w);
                    }
                    var back = y - yy;
                    if(!outOfBounds(back, y))
                    {
                        var sq = (x, back, false);   
                        var w = getWeight(x, y, 0, yy, true);
                        graph.Add((sq2, sq), w);
                    }
                }
            }
        }
        return graph;
    }

    private bool outOfBounds(int x, int y) => x < 0 || x >= this.W || y < 0 || y >= this.H;

    private double getWeight(int x, int y, int deltaX, int deltaY, bool backward)
    {
        if(deltaX > 0 && deltaY > 0) throw new Exception();

        var factor = backward ? -1 : 1;
        if(deltaX > 0)
        {
            var tot = 0;
            for(int i = 1; i <= deltaX; i++)
                tot += _input[x + i*factor, y];
            return tot;
        }
        else
        {
            var tot = 0;
            for(int i = 1; i <= deltaY; i++)
                tot += _input[x, y + i*factor];
            return tot;
        }
    }
}

class Square
{
    public Point Loc{get;}

    public bool Vertical{get;}

    public Square(Point loc, bool vert)
    {
        this.Loc = loc;
        this.Vertical = vert;
    }

    public override int GetHashCode()
    {
        return (this.Loc.X, this.Loc.Y, this.Vertical).GetHashCode();
    }
}
