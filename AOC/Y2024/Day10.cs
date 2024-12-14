using System;

namespace rsmith985.AOC.Y2024;

public class Day10 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var grid = this.GetGrid_Int().PadBorder(-1);
        var size = grid.GetSize();

        var startLocs = size.GetPointsInGrid().Where(p => grid.Get(p) == 0).ToList();

        var allPaths = new List<List<Point>>();
        foreach(var start in startLocs)
        {
            var paths = new List<List<Point>>();
            var path = new List<Point>(){start};
            discoverPaths(grid, path, paths);
            allPaths.AddRange(paths);
        }

        var set = new HashSet<(Point, Point)>();
        allPaths.ForEach(i => set.Add((i.First(), i.Last())));

        return set.Count;
    }

    public override object Part2()
    {
        var grid = this.GetGrid_Int().PadBorder(-1);
        var size = grid.GetSize();

        var startLocs = size.GetPointsInGrid().Where(p => grid.Get(p) == 0).ToList();

        var allPaths = new List<List<Point>>();
        foreach(var start in startLocs)
        {
            var paths = new List<List<Point>>();
            var path = new List<Point>(){start};
            discoverPaths(grid, path, paths);
            allPaths.AddRange(paths);
        }

        return allPaths.Count;
    }

    private void discoverPaths(int[,] grid, List<Point> path, List<List<Point>> validPaths)
    {
        var currLoc = path.Last();
        var currVal = grid.Get(currLoc);

        var dirs = new List<Direction>(){Direction.N, Direction.S, Direction.E, Direction.W};

        foreach(var dir in dirs)
        {
            var nextLoc = currLoc.GetNeighbor(dir);
            var nextVal = grid.Get(nextLoc);

            if(nextVal == currVal + 1)
            {
                var newPath = path.ToList();
                newPath.Add(nextLoc);
                if(nextVal == 9)
                {
                    validPaths.Add(newPath);
                }
                else
                {
                    discoverPaths(grid, newPath, validPaths);
                }
            }
        }
    }
}
