
namespace rsmith985.AOC.Y2023;

public class Day05 : Day
{
    public override object Part1_() =>
         new Func<List<List<(long, long, long)>>, long>(maps =>
            File.ReadLines(_file).First()[7..].Split(' ').Select(i => long.Parse(i))
                .Select(i => maps[0].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - maps[0].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
                .Select(i => maps[1].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - maps[1].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
                .Select(i => maps[2].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - maps[2].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
                .Select(i => maps[3].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - maps[3].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
                .Select(i => maps[4].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - maps[4].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
                .Select(i => maps[5].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - maps[5].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
                .Select(i => maps[6].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - maps[6].First(m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
                .Min()
        )(File.ReadAllLines(_file)
            .Skip(1)
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .Select(i => i.Contains(':') ? ":" : i)
            .Aggregate("", (i1, i2) => i1 + i2 + "|")
            .Split(":|", StringSplitOptions.RemoveEmptyEntries)
            .Select(i => i.Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select(j => j.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(k => (long.Parse(k[0]), long.Parse(k[1]), long.Parse(k[2])))
                .Concat(new List<(long, long, long)>(){(0, 0, long.MaxValue)})
                .ToList()
            ).ToList());

    public override object Part2_() =>
        new Func<List<List<(long, long, long)>>, long>(maps =>
            File.ReadLines(_file)
                .First()[7..]
                .Split(' ')
                .Select(i => long.Parse(i))
                .Chunk(2)
                .Select(i => (i[0], i[0] + i[1]))
                .Select(seedRange =>
                    Enumerable.Range(1, 1000).Aggregate((seedRange.Item1, seedRange.Item2, long.MaxValue, long.MaxValue), (a, b) =>
                        a.Item1 >= a.Item2 ? a :
                            new Func<(long, long), (long, long, long, long)>(item =>
                                (a.Item1 + item.Item2, a.Item2, item.Item1, item.Item1 < a.Item4 ? item.Item1 : a.Item4)
                            )(maps.Aggregate((a.Item1, long.MaxValue), (x, m) =>
                                    new Func<(long dst, long src, long len), (long, long)>(range =>
                                        (range.dst + (x.Item1 - range.src),
                                         Math.Min(x.Item2, 
                                            range.len == long.MaxValue ? 
                                                (m.Where(i => i.Item2 > x.Item1).Any() ? m.Where(i => i.Item2 > x.Item1).Min(i => i.Item2) : 
                                                long.MaxValue) : 
                                                range.len - (x.Item1 - range.src)
                                                )
                                        )
                                    ) (m.First(m => x.Item1 >= m.Item2 && x.Item1 < (m.Item2 + m.Item3))) 
                                )
                            )
                        )
                    ).Min(i => i.Item4)
            )
            (File.ReadAllLines(_file)
                .Skip(1)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Contains(':') ? ":" : i)
                .Aggregate("", (i1, i2) => i1 + i2 + "|")
                .Split(":|", StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Split('|', StringSplitOptions.RemoveEmptyEntries)
                    .Select(j => j.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    .Select(k => (long.Parse(k[0]), long.Parse(k[1]), long.Parse(k[2])))
                    .Concat(new List<(long, long, long)>(){(0, 0, long.MaxValue)})
                    .ToList()
                ).ToList());

    public override object Part1()
    {
        var lines = this.GetLines();
        var initSeeds = getInitSeeds(lines[0]);
        var maps = parseMaps(lines);

        var min = long.MaxValue;
        foreach(var seed in initSeeds)
        {
            var loc = getLoc(seed, maps);
            if(loc < min)
                min = loc;
        }

        return min;
    }

    public override object Part2()
    {
        var lines = this.GetLines();
        var initSeeds = getInitSeeds(lines[0]);
        var maps = parseMaps(lines);

        var min = long.MaxValue;
        for(int i = 0 ; i < initSeeds.Count; i += 2)
        {
            var count = 0;
            var start = initSeeds[i];
            var len = initSeeds[i+1];
            long currIncrement = 1;
            for(var j = start; j < start + len; j += currIncrement)
            {
                (var loc, currIncrement) = getLoc2(j, maps);
                if(loc < min)
                    min = loc;
                //Console.WriteLine("    start:" + j + " loc:" + loc + " inc:" + currIncrement + " min:" + min);
                count++;
            }
            //Console.WriteLine((i/2) + " " + min + " " + count);
        }

        return min;
    }

    private List<List<(long dst, long src, long len)>> parseMaps(string[] lines)
    {
        var maps = new List<List<(long dst, long src, long len)>>();
        List<(long, long, long)> currRanges = null;
        foreach(var line in lines)
        {
            if(line.Contains("map"))
            {
                if(currRanges != null)
                    maps.Add(currRanges);
                currRanges = new List<(long, long, long)>();
            }
            else
            {
                var parts = line.Split(' ');
                if(parts.Length == 3)
                {
                    var dst = long.Parse(parts[0]);
                    var src = long.Parse(parts[1]);
                    var len = long.Parse(parts[2]);
                    currRanges.Add((dst, src, len));
                }
            }
        }
        maps.Add(currRanges);
        return maps;
    }

    private List<long> getInitSeeds(string line)
    {
        var index = line.IndexOf(':');

        var rv = new List<long>();
        var txt = line[(index+1)..];
        foreach (var item in txt.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            rv.Add(long.Parse(item));
        }
        return rv;
    }

    private long getLoc(long seed, List<List<(long dst, long src, long len)>> maps)
    {
        var curr = seed;
        foreach(var map in maps)
        {
            curr = getNext(curr, map);
        }
        return curr;
    }

    private long getNext(long src, List<(long dst, long src, long len)> ranges)
    {
        foreach(var range in ranges)
        {
            if(src >= range.src && src < (range.src + range.len))
                return range.dst + (src - range.src);
        }
        return src;
    }
    private (long loc, long exit) getLoc2(long seed, List<List<(long dst, long src, long len)>> maps)
    {
        var curr = seed;
        var minExit = long.MaxValue;
        foreach(var map in maps)
        {
            (curr, var exit) = getNext2(curr, map);
            if(exit < minExit)
                minExit = exit;

        }
        return (curr, minExit);
    }

    private (long loc, long exit) getNext2(long src, List<(long dst, long src, long len)> ranges)
    {
        long minExit = long.MaxValue;
        foreach(var range in ranges)
        {
            if(src >= range.src && src < (range.src + range.len))
            {
                var exit = range.len - (src - range.src);
                return (range.dst + (src - range.src), exit);
            }

            if(range.src > src && range.src < minExit)
                minExit = range.src;
        }

        return (src, minExit);
    }
}
