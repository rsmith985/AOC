using System;
using System.Net.Http.Headers;
using Microsoft.Z3;

namespace rsmith985.AOC.Y2024;

public class Day19 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        (var towels, var patterns) = parseInput();

        var cache = new Dictionary<string, long>();

        var count = 0;
        foreach(var pattern in patterns)
        {
            //Console.WriteLine("Checking: " + pattern);
            var num = discoverPossible(pattern, towels, cache);
            if(num > 0)
                count++;
        }
        return count;
    }

    public override object Part2()
    {
        (var towels, var patterns) = parseInput();

        var cache = new Dictionary<string, long>();

        var tot = 0L;
        foreach(var pattern in patterns)
        {
            //Console.WriteLine("Checking: " + pattern);
            var num = discoverPossible(pattern, towels, cache);
            tot += num;
        }
        return tot;
    }

    private long discoverPossible(string pattern, HashSet<string> towels, Dictionary<string, long> cache)
    {
        if(pattern.Length == 0) return 1;

        if(cache.TryGetValue(pattern, out long val))
            return val;
        
        var possible = towels.Where(t => pattern.StartsWith(t));
        var sum = possible.Sum(t => discoverPossible(pattern[t.Length..], towels, cache));

        cache.Add(pattern, sum);
        return sum;
    }

    private (HashSet<string> towels, List<string> patterns) parseInput()
    {
        var lines = this.GetLines();
        var towels = lines[0].Split<string>(i => i, ", ").ToList();

        var patterns = new List<string>();
        for(int i = 2; i < lines.Length; i++)
            patterns.Add(lines[i]);
        
        return (new HashSet<string>(towels), patterns);
    }
}
