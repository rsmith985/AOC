using System;
using System.Drawing.Printing;
using rsmith985.AOC.Y2023;

namespace rsmith985.AOC.Y2024;

public class Day18 : Day
{

    //protected override bool _useDefaultTestFile => true;

    private Size _size = new Size(71,71);
    public override object Part1()
    {
        var input = parseInput();
        var grid = createGrid(input, 12);
        //grid.PrintToConsole(i => i ? "#" : ".");
        var graph = toGraph(grid).ToDirectedGraph();

        var dijkstra = new Dijkstra<Point>(graph, new Point(1,1));

        /*
        var path = dijkstra.GetEdgePathTo(_size.ToPoint());
        var g2 = grid.Convert(i => i ? '#' : '.');
        foreach(var e in path)
        {
            g2.Set(e.From.Data, 'O');
        }
        g2.PrintToConsole();
        */

        return dijkstra.DistanceTo(_size.ToPoint());
    }

    public override object Part2()
    {
        var input = parseInput();

        var endPoint = _size.ToPoint();
        var max = input.Count;
        var num = max/2;
        for(var jump = max/2; jump > 1; jump /= 2)
        {
            var grid = createGrid(input, num);
            var graph = toGraph(grid).ToDirectedGraph();
            var dijkstra = new Dijkstra<Point>(graph, new Point(1,1));

            if(!dijkstra.PathExistsTo(endPoint))
            {
                num -= jump;
            }
            else
            {
                num += jump;  
            }
        }

        for(var i = num-2; i <= num+2; i++)
        {
            var grid = createGrid(input, i);
            var graph = toGraph(grid).ToDirectedGraph();
            var dijkstra = new Dijkstra<Point>(graph, new Point(1,1));
            if(!dijkstra.PathExistsTo(endPoint))
            {
                var p = input[i-1];
                return $"{p.X},{p.Y}";
            }
        }
        return "fail";
    }

    private bool[,] createGrid(List<Point> points, int limit = -1)
    {
        var grid = new bool[_size.Width, _size.Height];
        
        if(limit == -1) limit = points.Count;

        for(int i = 0; i < limit; i++)
        {
            var p = points[i];
            grid.Set(p, true);
        }
        return grid.PadBorder(true);
    }
    private Graph<Point> toGraph(bool[,] grid)
    {
        var graph = new Graph<Point>();
        foreach(var p in _size.GetPointsInGrid())
        {
            var pp = p.Plus(1);
            if(grid.Get(pp)) continue;
            
            var n = pp.GetNeighbor(Direction.N);
            var s = pp.GetNeighbor(Direction.S);
            var e = pp.GetNeighbor(Direction.E);
            var w = pp.GetNeighbor(Direction.W);
            if(!grid.Get(n)) graph.Add(pp, n);
            if(!grid.Get(s)) graph.Add(pp, s);
            if(!grid.Get(e)) graph.Add(pp, e);
            if(!grid.Get(w)) graph.Add(pp, w);
        }
        return graph; 
    }

    private List<Point> parseInput()
    {
        var rv = new List<Point>();
        foreach(var line in this.GetLines())
        {
            (var x, var y) = line.Split2(int.Parse, ",");
            rv.Add(new Point(x,y));
        }
        return rv;
    }
}
