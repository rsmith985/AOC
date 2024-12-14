using System;

namespace rsmith985.AOC.Y2024;

public class Day12 : Day
{
    //protected override bool _useDefaultTestFile => true;
    public override object Part1()
    {
        var grid = this.GetGrid_Char().PadBorder('.');
        var copy = grid.Copy_();

        var regions = new List<HashSet<(int, int)>>();
        for(int x = 1; x < copy.GetLength(0) - 1; x++)
        {
            for(int y = 1; y < copy.GetLength(1) - 1; y++)
            {
                if(copy[x,y] == '.') continue;

                var region = new HashSet<(int, int)>();
                floodFind(copy, copy[x,y], new Point(x,y), region);
                regions.Add(region);
            }
        }

        return regions.Sum(r => r.Count * getPerimeter(r));
    }

    public override object Part2()
    {
        var grid = this.GetGrid_Char().PadBorder('.');
        var copy = grid.Copy_();

        var regions = new List<HashSet<(int, int)>>();
        for(int x = 1; x < copy.GetLength(0) - 1; x++)
        {
            for(int y = 1; y < copy.GetLength(1) - 1; y++)
            {
                if(copy[x,y] == '.') continue;

                var region = new HashSet<(int, int)>();
                floodFind(copy, copy[x,y], new Point(x,y), region);
                regions.Add(region);
            }
        }

        return regions.Sum(r => r.Count * getCorners(r));
    }

    private int getPerimeter(HashSet<(int, int)> points)
    {
        var sum = 0;
        foreach(var p in points)
        {
            if(!points.Contains(p.GetNeighbor(Direction.N))) sum++;
            if(!points.Contains(p.GetNeighbor(Direction.S))) sum++;
            if(!points.Contains(p.GetNeighbor(Direction.E))) sum++;
            if(!points.Contains(p.GetNeighbor(Direction.W))) sum++;
        }
        return sum;
    }

    private int getCorners(HashSet<(int, int)> points)
    {
        if(points.Count == 1) return 4;

        var corners = 0;
        foreach(var p in points)
        {
            // Counting outside corners
            var n4 = 0;
            if(points.Contains(p.GetNeighbor(Direction.N))) n4++;
            if(points.Contains(p.GetNeighbor(Direction.S))) n4++;
            if(points.Contains(p.GetNeighbor(Direction.E))) n4++;
            if(points.Contains(p.GetNeighbor(Direction.W))) n4++;

            if(n4 == 1) 
                corners +=2;    // end-point
            else if(n4 == 2) 
            {
                var hasNS = points.Contains(p.GetNeighbor(Direction.N)) || points.Contains(p.GetNeighbor(Direction.S));
                var hasEW = points.Contains(p.GetNeighbor(Direction.E)) || points.Contains(p.GetNeighbor(Direction.W));
                if(hasNS && hasEW)
                    corners++; // rect-corner
            }

            // Counting outside corners
            if(points.Contains(p.GetNeighbor(Direction.N)) && 
                points.Contains(p.GetNeighbor(Direction.E)) &&
                !points.Contains(p.GetNeighbor(Direction.NE)))
                corners++;
            if(points.Contains(p.GetNeighbor(Direction.S)) && 
                points.Contains(p.GetNeighbor(Direction.E)) &&
                !points.Contains(p.GetNeighbor(Direction.SE)))
                corners++;
            if(points.Contains(p.GetNeighbor(Direction.N)) && 
                points.Contains(p.GetNeighbor(Direction.W)) &&
                !points.Contains(p.GetNeighbor(Direction.NW)))
                corners++;
            if(points.Contains(p.GetNeighbor(Direction.S)) && 
                points.Contains(p.GetNeighbor(Direction.W)) &&
                !points.Contains(p.GetNeighbor(Direction.SW)))
                corners++;
        }
        //Console.WriteLine(corners);
        return corners;
    }

    private void floodFind(char[,] grid, char c, Point p, HashSet<(int, int)> locations)
    {
        var val = grid.Get(p);
        if(val != c)
            return;
        
        locations.Add(p.ToTuple());
        grid.Set(p, '.');
        floodFind(grid, c, p.GetNeighbor(Direction.N), locations);
        floodFind(grid, c, p.GetNeighbor(Direction.S), locations);
        floodFind(grid, c, p.GetNeighbor(Direction.E), locations);
        floodFind(grid, c, p.GetNeighbor(Direction.W), locations);
    }
}
