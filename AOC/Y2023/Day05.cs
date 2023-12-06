namespace rsmith985.AOC.Y2023;

public class Day05 : Day
{
    public override object Part1_()
    {
        return new Func<List<List<(long, long, long)>>, long>(maps =>
            File.ReadLines(_file).First()[7..].Split(' ').Select(i => long.Parse(i))
            .Select(i => maps[0].FirstOrDefault(
                m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - 
                    maps[0].FirstOrDefault(m =>i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
            .Select(i => maps[1].FirstOrDefault(
                m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - 
                    maps[1].FirstOrDefault(m =>i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
            .Select(i => maps[2].FirstOrDefault(
                m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - 
                    maps[2].FirstOrDefault(m =>i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
            .Select(i => maps[3].FirstOrDefault(
                m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - 
                    maps[3].FirstOrDefault(m =>i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
            .Select(i => maps[4].FirstOrDefault(
                m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - 
                    maps[4].FirstOrDefault(m =>i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
            .Select(i => maps[5].FirstOrDefault(
                m => i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - 
                    maps[5].FirstOrDefault(m =>i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
            .Select(i => maps[6].FirstOrDefault(
                m =>i >= m.Item2 && i < (m.Item2 + m.Item3)).Item1 + i - 
                    maps[6].FirstOrDefault(m =>i >= m.Item2 && i < (m.Item2 + m.Item3)).Item2)
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
    }

    public override object Part1()
    {
        var lines = this.GetLines();
        var initSeeds = getInitSeeds(lines[0]);
        var maps = parseMaps(lines);

        var min = ulong.MaxValue;
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

        var min = ulong.MaxValue;
        for(int i = 0 ; i < initSeeds.Count; i += 2)
        {
            var start = initSeeds[i];
            var len = initSeeds[i+1];
            ulong currIncrement = 1;
            for(var j = start; j < start + len; j += currIncrement)
            {
                (var loc, currIncrement) = getLoc2(j, maps);
                if(loc < min)
                    min = loc;
            }
        }

        return min;
    }

    private List<List<(ulong dst, ulong src, ulong len)>> parseMaps(string[] lines)
    {
        var maps = new List<List<(ulong dst, ulong src, ulong len)>>();
        List<(ulong, ulong, ulong)> currRanges = null;
        foreach(var line in lines)
        {
            if(line.Contains("map"))
            {
                if(currRanges != null)
                    maps.Add(currRanges);
                currRanges = new List<(ulong, ulong, ulong)>();
            }
            else
            {
                var parts = line.Split(' ');
                if(parts.Length == 3)
                {
                    var dst = ulong.Parse(parts[0]);
                    var src = ulong.Parse(parts[1]);
                    var len = ulong.Parse(parts[2]);
                    currRanges.Add((dst, src, len));
                }
            }
        }
        maps.Add(currRanges);
        return maps;
    }

    private List<ulong> getInitSeeds(string line)
    {
        var index = line.IndexOf(':');

        var rv = new List<ulong>();
        var txt = line[(index+1)..];
        foreach (var item in txt.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            rv.Add(ulong.Parse(item));
        }
        return rv;
    }

    private ulong getLoc(ulong seed, List<List<(ulong dst, ulong src, ulong len)>> maps)
    {
        var curr = seed;
        foreach(var map in maps)
        {
            curr = getNext(curr, map);
        }
        return curr;
    }

    private ulong getNext(ulong src, List<(ulong dst, ulong src, ulong len)> ranges)
    {
        foreach(var range in ranges)
        {
            if(src >= range.src && src < (range.src + range.len))
                return range.dst + (src - range.src);
        }
        return src;
    }
    private (ulong loc, ulong exit) getLoc2(ulong seed, List<List<(ulong dst, ulong src, ulong len)>> maps)
    {
        var curr = seed;
        var minExit = ulong.MaxValue;
        foreach(var map in maps)
        {
            (curr, var exit) = getNext2(curr, map);
            if(exit < minExit)
                minExit = exit;

        }
        return (curr, minExit);
    }

    private (ulong loc, ulong exit) getNext2(ulong src, List<(ulong dst, ulong src, ulong len)> ranges)
    {
        ulong minExit = ulong.MaxValue;
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
