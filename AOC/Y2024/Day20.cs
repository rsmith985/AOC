using System;
using System.Reflection.Metadata.Ecma335;
using rsmith985.AOC.Y2023;

namespace rsmith985.AOC.Y2024;

public class Day20 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        (var grid, var start, var end) = parseInput();
        var graph = grid.ToGraph().ToDirectedGraph();

        var dijkstra = new Dijkstra<Point>(graph, start);

        var path = dijkstra.GetPathTo(end);

        return getNumberValidSkips(grid, path, 100, 2);
    }


    public override object Part2()
    {
        (var grid, var start, var end) = parseInput();
        var graph = grid.ToGraph().ToDirectedGraph();

        var dijkstra = new Dijkstra<Point>(graph, start);

        var path = dijkstra.GetPathTo(end);

        return getNumberValidSkips(grid, path, 100, 20);
    }

    private int getNumberValidSkips(bool[,] grid, List<Point> path, int minSkip, int maxDist)
    {
        var set = new HashSet<((int, int), (int, int))>();
        var num = 0;
        for(int i = 0; i < path.Count - minSkip + 2; i++)
        {   
            var p1 = path[i];
            for(int j = i + minSkip + 2;  j < path.Count; j++)
            {
                var p2 = path[j];
                var dist = p1.DistanceTo_CityBlock(p2);
                
                if(dist <= maxDist)
                {
                    if(j - i - dist >= minSkip)
                    {
                        set.Add((p1.ToTuple(), p2.ToTuple()));
                        //Console.WriteLine(p1 + " " + p2 + " " + (j - i - dist));
                        num++;
                    }
                }
            }
        }
        return num;
    }
    private (bool[,] grid, Point start, Point end) parseInput()
    {
        var cGrid = this.GetLines().To2DArray();

        var start = Point.Empty;
        var end= Point.Empty;
        var grid = new bool[cGrid.GetLength(0), cGrid.GetLength(1)];
        foreach(var p in cGrid.GetSize().GetPointsInGrid())
        {
            if(cGrid.Get(p) == '.')
                grid.Set(p, true);
            else if(cGrid.Get(p) == '#')
                grid.Set(p, false);
            else if(cGrid.Get(p) == 'S')
            {
                start = p;
                grid.Set(p, true);
            }
            else if(cGrid.Get(p) == 'E')
            {
                end = p;
                grid.Set(p, true);
            }
            else
                throw new Exception();
        }

        return (grid, start, end);
    }
}
