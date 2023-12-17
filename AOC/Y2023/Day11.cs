using System.Drawing;

namespace rsmith985.AOC.Y2023;

public class Day11 : Day
{
    public override object Part1_() =>
        new Func<(HashSet<int>, HashSet<int>, List<Point>), long>(input =>
            Enumerable.Range(0, input.Item3.Count-1)
                .SelectMany(i1 => 
                    Enumerable.Range(i1+1, input.Item3.Count-i1-1)
                        .Select(i2 => (
                            new Point(input.Item3[i1].X < input.Item3[i2].X ? input.Item3[i1].X : input.Item3[i2].X, 
                                      input.Item3[i1].Y < input.Item3[i2].Y ? input.Item3[i1].Y : input.Item3[i2].Y),
                            new Point(input.Item3[i1].X > input.Item3[i2].X ? input.Item3[i1].X : input.Item3[i2].X, 
                                      input.Item3[i1].Y > input.Item3[i2].Y ? input.Item3[i1].Y : input.Item3[i2].Y)
                            )) )
                .Sum(pair => 
                    (long)pair.Item2.X - (long)pair.Item1.X + 
                        ((pair.Item2.X - pair.Item1.X < 1) ? 0 : 
                            (long)Enumerable.Range(pair.Item1.X + 1, (pair.Item2.X - pair.Item1.X - 1))
                                .Where(i => input.Item2.Contains(i)).Count()) +
                    (long)pair.Item2.Y - (long)pair.Item1.Y + 
                        ((pair.Item2.Y - pair.Item1.Y < 1) ? 0 : 
                            (long)Enumerable.Range(pair.Item1.Y + 1, (pair.Item2.Y - pair.Item1.Y - 1))
                                .Where(i => input.Item1.Contains(i)).Count())
            ))(new Func<string[], (HashSet<int>, HashSet<int>, List<Point>)>(input =>
                (   new HashSet<int>(Enumerable.Range(0, input.Length).Where(i => input[i].All(c => c == '.'))),
                    new HashSet<int>(Enumerable.Range(0, input[0].Length).Where(i => input.All(line => line[i] == '.'))),
                    Enumerable.Range(0, input.Length * input[0].Length)
                        .Select(i => new Point(i % input[0].Length, i/input[0].Length))
                        .Where(p => input[p.Y][p.X] == '#')
                        .ToList()
                ))(this.GetLines())
            );    
    public override object Part2_() =>
        new Func<(HashSet<int>, HashSet<int>, List<Point>), long>(input =>
            Enumerable.Range(0, input.Item3.Count-1)
                .SelectMany(i1 => 
                    Enumerable.Range(i1+1, input.Item3.Count-i1-1)
                        .Select(i2 => (
                            new Point(input.Item3[i1].X < input.Item3[i2].X ? input.Item3[i1].X : input.Item3[i2].X, 
                                      input.Item3[i1].Y < input.Item3[i2].Y ? input.Item3[i1].Y : input.Item3[i2].Y),
                            new Point(input.Item3[i1].X > input.Item3[i2].X ? input.Item3[i1].X : input.Item3[i2].X, 
                                      input.Item3[i1].Y > input.Item3[i2].Y ? input.Item3[i1].Y : input.Item3[i2].Y)
                            )) )
                .Sum(pair => 
                    (long)pair.Item2.X - (long)pair.Item1.X + 
                        ((pair.Item2.X - pair.Item1.X < 1) ? 0 : 
                            (long)Enumerable.Range(pair.Item1.X + 1, (pair.Item2.X - pair.Item1.X - 1))
                                .Where(i => input.Item2.Contains(i)).Sum(i => 999999)) +
                    (long)pair.Item2.Y - (long)pair.Item1.Y + 
                        ((pair.Item2.Y - pair.Item1.Y < 1) ? 0 : 
                            (long)Enumerable.Range(pair.Item1.Y + 1, (pair.Item2.Y - pair.Item1.Y - 1))
                                .Where(i => input.Item1.Contains(i)).Sum(i => 999999))
            ))(new Func<string[], (HashSet<int>, HashSet<int>, List<Point>)>(input =>
                (   new HashSet<int>(Enumerable.Range(0, input.Length).Where(i => input[i].All(c => c == '.'))),
                    new HashSet<int>(Enumerable.Range(0, input[0].Length).Where(i => input.All(line => line[i] == '.'))),
                    Enumerable.Range(0, input.Length * input[0].Length)
                        .Select(i => new Point(i % input[0].Length, i/input[0].Length))
                        .Where(p => input[p.Y][p.X] == '#')
                        .ToList()
                ))(this.GetLines())
            );  

    public override object Part1()
    {
        var input = this.GetLines();
        
        var rows = emptyRows(input);
        var cols = emptyCols(input);
        var locs = getLocs(input);

        long tot = 0;
        for(int i = 0; i < locs.Count - 1; i++)
        {
            for(int j = i+1; j < locs.Count; j++)
            {
                var p1 = locs[i];
                var p2 = locs[j];
                var min = p1.Min(p2);
                var max = p1.Max(p2);
                var x = getDist(min.X, max.X, cols);
                var y = getDist(min.Y, max.Y, rows);
                tot += (x+y);
            }
        }
        return tot;
    }

    public override object Part2()
    {
        var input = this.GetLines();
        
        var rows = emptyRows(input);
        var cols = emptyCols(input);
        var locs = getLocs(input);

        long tot = 0;
        for(int i = 0; i < locs.Count - 1; i++)
        {
            for(int j = i+1; j < locs.Count; j++)
            {
                var p1 = locs[i];
                var p2 = locs[j];
                var min = p1.Min(p2);
                var max = p1.Max(p2);
                var x = getDist(min.X, max.X, cols, 999999);
                var y = getDist(min.Y, max.Y, rows, 999999);
                tot += (x+y);
            }
        }
        return tot;
    }

    private long getDist(int start, int end, HashSet<int> empty, int factor = 1)
    {
        long xtra = 0;
        for(int i = start + 1; i < end; i++)
        {
            if(empty.Contains(i))
                xtra += factor;
        }
        return (long)(end - start) + xtra;
    }

    private List<Point> getLocs(string[] input)
    {
        var rv = new List<Point>();
        for(var y = 0; y < input.Length; y++)
        {
            for(int x =0; x < input.Length; x++)
            {
                if(input[y][x] == '#')
                    rv.Add(new Point(x, y));
            }
        }
        return rv;
    }
    private HashSet<int> emptyCols(string[] lines)
    {
        var rv = new HashSet<int>();
        for(int i = 0; i < lines[0].Length; i++)
        {
            if(lines.All(line => line[i] == '.'))
                rv.Add(i);
        }
        return rv;
    }
    private HashSet<int> emptyRows(string[] lines)
    {
        var rv = new HashSet<int>();
        for(int i = 0; i < lines.Length; i++)
        {
            if(lines[i].All(i => i == '.'))
                rv.Add(i);
        }
        return rv;
    }
}
