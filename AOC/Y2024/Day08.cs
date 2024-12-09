using System;

namespace rsmith985.AOC.Y2024;

public class Day08 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var lines = this.GetLines();
        var size = lines.GetSize();
        (var dict, var locs) = parseInput(lines);

        var uniqueLocs = new HashSet<(int x, int y)>();
        //var count = 0;
        foreach(var c in dict.Keys)
        {
            var items = dict[c];

            for(int i = 0; i < items.Count - 1; i++)
            {
                for(int j = i + 1; j < items.Count; j++)
                {
                    var deltaX = items[j].X - items[i].X;
                    var deltaY = items[j].Y - items[i].Y;

                    var x1 = items[i].X - deltaX;
                    var x2 = items[j].X + deltaX;
                    var y1 = items[i].Y - deltaY;
                    var y2 = items[j].Y + deltaY;

                    if(withinBounds(x1, y1, size))
                        uniqueLocs.Add((x1, y1));
                    if(withinBounds(x2, y2, size))
                        uniqueLocs.Add((x2, y2));
                }
            }
        }
        

        return uniqueLocs.Count;
    }


    public override object Part2()
    {
        var lines = this.GetLines();
        var size = lines.GetSize();
        (var dict, var locs) = parseInput(lines);

        var uniqueLocs = new HashSet<(int x, int y)>();
        foreach(var c in dict.Keys)
        {
            var items = dict[c];

            for(int i = 0; i < items.Count - 1; i++)
            {
                for(int j = i + 1; j < items.Count; j++)
                {
                    uniqueLocs.Add((items[i].X, items[i].Y));
                    uniqueLocs.Add((items[j].X, items[j].Y));

                    var foundValid = true;
                    var mult = 1;
                    while(foundValid)
                    {
                        var deltaX = (items[j].X - items[i].X)*mult;
                        var deltaY = (items[j].Y - items[i].Y)*mult;

                        var x1 = items[i].X - deltaX;
                        var x2 = items[j].X + deltaX;
                        var y1 = items[i].Y - deltaY;
                        var y2 = items[j].Y + deltaY;

                        foundValid = false;
                        if(withinBounds(x1, y1, size))
                        {
                            uniqueLocs.Add((x1, y1));
                            foundValid = true;
                        }
                        if(withinBounds(x2, y2, size))
                        {
                            uniqueLocs.Add((x2, y2));
                            foundValid = true;
                        }
                        mult++;
                    }
                }
            }
        }
        
        /*
        var copy = lines.ToList();
        foreach(var ans in uniqueLocs)
        {
            copy[ans.y] = copy[ans.y].ReplaceStringChar(ans.x, '#');
        }

        foreach(var line in copy)
            Console.WriteLine(line);
        */

        return uniqueLocs.Count;
    }

    private bool withinBounds(int x, int y, Size size)
    {
        return x >= 0 && x < size.Width && y >= 0 && y < size.Height;
    }
    private (Dictionary<char, List<Point>> dict, HashSet<(int, int)> locs) parseInput(string[] lines)
    {
        var dict = new Dictionary<char,List<Point>>();
        var locs = new HashSet<(int, int)> ();
        for(int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for(int x = 0; x < line.Length; x++)
            {
                var c = line[x];
                if(c == '.') continue;

                locs.Add((x,y));    
                if(!dict.ContainsKey(c))
                    dict.Add(c, new List<Point>());
                dict[c].Add(new Point(x, y));
            }
        }
        return (dict, locs);
    }
}
