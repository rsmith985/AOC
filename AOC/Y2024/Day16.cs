using System;
using rsmith985.AOC.Y2023;

namespace rsmith985.AOC.Y2024;

public class Day16 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        (var graph, var grid) = parseInput();
        var startPoint = new Point(1,grid.GetLength(1)-2);
        var endPoint = new Point(grid.GetLength(0)-2, 1);
        var startSpot = new Spot(startPoint, Direction.E);
        var endSpot1 = new Spot(endPoint, Direction.E);
        var endSpot2 = new Spot(endPoint, Direction.N);
        graph.Add(startSpot, new Spot(startPoint, Direction.N), 1000);
        graph.Simplify();

        var path = new Dijkstra<Spot>(graph, startSpot);
        var d1 = graph.Nodes.ContainsKey(endSpot1) && path.PathExistsTo(endSpot1) ? path.DistanceTo(endSpot1) : double.MaxValue;
        var d2 = graph.Nodes.ContainsKey(endSpot2) && path.PathExistsTo(endSpot2) ? path.DistanceTo(endSpot2) : double.MaxValue;

        return Math.Min(d1, d2);
    }

    public override object Part2()
    {
        (var graph, var grid) = parseInput();
        var startPoint = new Point(1,grid.GetLength(1)-2);
        var endPoint = new Point(grid.GetLength(0)-2, 1);
        var startSpot = new Spot(startPoint, Direction.E);
        var endSpot1 = new Spot(endPoint, Direction.E);
        var endSpot2 = new Spot(endPoint, Direction.N);
        graph.Add(endSpot1);
        graph.Add(endSpot2);
        graph.Add(startSpot, new Spot(startPoint, Direction.N), 1000);

        //graph.Simplify();

        var dijkstra = new Dijkstra<Spot>(graph, startSpot);
        var d1 = dijkstra.DistanceTo(endSpot1);
        var d2 = dijkstra.DistanceTo(endSpot2);

        var endSpot = d1 < d2 ? endSpot1 : endSpot2;
        var minDist = Math.Min(d1, d2);
        var minPath = d1 < d2 ? dijkstra.GetEdgePathTo(endSpot1) : dijkstra.GetEdgePathTo(endSpot2);

        var allPaths = discoverMinPaths(graph, minDist, minPath, startSpot, endSpot, new HashSet<string>());

        var set = new HashSet<(int, int)>();
        foreach(var path in allPaths)
        {
            foreach(var edge in path)
            {
                set.Add(edge.From.Data.Loc.ToTuple());
            }
        }
        set.Add(endSpot.Loc.ToTuple());

        foreach(var p in set)
        {
            grid.Set(p.ToPoint(), 'O');
        }
        grid.PrintToConsole();
        return set.Count;
    }

    private List<List<DirectedEdge<Spot>>> discoverMinPaths(DirectedGraph<Spot> graph, double minDist, List<DirectedEdge<Spot>> minPath, Spot startSpot, Spot endSpot, HashSet<string> attempted, int level = 0)
    {
        var rv = new List<List<DirectedEdge<Spot>>>();
        foreach(var edge in minPath)
        {
            if(attempted.Contains(edge.Key)) continue;
            graph.Remove(edge);
            attempted.Add(edge.Key);

            var dijkstra = new Dijkstra<Spot>(graph, startSpot);
            var d = dijkstra.DistanceTo(endSpot);
            if(d == minDist)
            {
                var path = dijkstra.GetEdgePathTo(endSpot);
                rv.Add(path);
                
                var g = graph.Copy();
                var pathCopy = new List<DirectedEdge<Spot>>();
                foreach(var p in path)
                    pathCopy.Add(g.Edges[p.Key]);

                Console.WriteLine(level + " " + edge.Key);
                foreach(var p in discoverMinPaths(g, minDist, pathCopy, startSpot, endSpot, attempted, level +1))
                    rv.Add(p);
            }
            
            graph.Add(edge.From.Data, edge.To.Data, edge.Weight);
        }
        return rv;
    }

    private (DirectedGraph<Spot>, char[,]) parseInput()
    {
        var grid = this.GetGrid_Char();

        grid[1, grid.GetLength(1) - 2] = '.';
        grid[grid.GetLength(0) - 2, 1] = '.';

        var graph = new DirectedGraph<Spot>();

        for(int x = 1; x < grid.GetLength(0); x++)
        {
            for(int y = 1; y < grid.GetLength(1); y++)
            {
                if(grid[x,y] == '#') continue;

                var p = new Point(x,y);
                var n = p.GetNeighbor(Direction.N);
                var s = p.GetNeighbor(Direction.S);
                var e = p.GetNeighbor(Direction.E);
                var w = p.GetNeighbor(Direction.W);
                var nn = grid.Get(n) == '.';
                var ss = grid.Get(s) == '.';
                var ee = grid.Get(e) == '.';
                var ww = grid.Get(w) == '.';

                if(nn)
                    graph.Add(new Spot(p, Direction.N), new Spot(n, Direction.N), 1);
                if(ss)
                    graph.Add(new Spot(p, Direction.S), new Spot(s, Direction.S), 1);
                if(ee)
                    graph.Add(new Spot(p, Direction.E), new Spot(e, Direction.E), 1);
                if(ww)
                    graph.Add(new Spot(p, Direction.W), new Spot(w, Direction.W), 1);
                
                if(nn && ee)
                {
                    graph.Add(new Spot(p, Direction.S), new Spot(p, Direction.E), 1000);
                    graph.Add(new Spot(p, Direction.W), new Spot(p, Direction.N), 1000);
                }
                if(nn && ww)
                {
                    graph.Add(new Spot(p, Direction.S), new Spot(p, Direction.W), 1000);
                    graph.Add(new Spot(p, Direction.E), new Spot(p, Direction.N), 1000);
                }
                if(ss && ee)
                {
                    graph.Add(new Spot(p, Direction.N), new Spot(p, Direction.E), 1000);
                    graph.Add(new Spot(p, Direction.W), new Spot(p, Direction.S), 1000);
                }
                if(ss && ww)
                {
                    graph.Add(new Spot(p, Direction.N), new Spot(p, Direction.W), 1000);
                    graph.Add(new Spot(p, Direction.E), new Spot(p, Direction.S), 1000);
                }
            }
        }
        return (graph, grid);
    }

    record Spot
    {
        public Point Loc;
        public Direction Dir;

        public Spot(Point loc, Direction dir)
        {
            this.Loc = loc;
            this.Dir = dir;
        }

        public override string ToString()
        {
            return $"{this.Loc.X}.{this.Loc.Y}.{this.Dir}";
        }
    }
}


