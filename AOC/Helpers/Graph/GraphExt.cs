using System;

namespace rsmith985.AOC;

public static class GraphExt
{
    public static Graph<Point> ToGraph(this bool[,] grid)
    {
        var graph = new Graph<Point>();
        foreach(var p in grid.GetSize().GetPointsInGrid())
        {
            if(!grid.Get(p)) continue;

            var s = p.GetNeighbor(Direction.S);
            var e = p.GetNeighbor(Direction.E);
            if(grid.Get(s))
                graph.Add(p, s);
            if(grid.Get(e))
                graph.Add(p, e);
        }
        return graph;
    }
}
