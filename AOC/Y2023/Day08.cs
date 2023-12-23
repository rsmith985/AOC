using System.Numerics;

namespace rsmith985.AOC.Y2023;

public class Day08 : Day
{
    public override object Part1_() => 
        new Func<List<int>, Dictionary<string, string[]>, List<string>, int>((path, lookup, route) =>
            Enumerable.Range(1, route.Count - 1)
                .Perform(i => route[i] = lookup[route[i-1]][path[(i - 1) % path.Count]])
                .First(i => route[i] == "ZZZ") )
            (File.ReadAllLines(_file)[0].Select(c => c == 'L' ? 0 : 1).ToList(), 
             File.ReadAllLines(_file).Skip(2).ToDictionary(line => line[..3], line => new string[]{line[7..10], line[12..15]}),
             new string[1000000].Select(i => "AAA").ToList());

    public override object Part2_()  => 
        new Func<List<int>, Dictionary<string, string[]>, ulong>((path, lookup) =>
            lookup.Keys.Where(i => i.EndsWith("A"))
                .Select(start => new Func<List<int>, Dictionary<string, string[]>, List<string>, int>((path, lookup, route) =>
                    Enumerable.Range(1, route.Count - 1)
                        .Perform(i => route[i] = lookup[route[i-1]][path[(i - 1) % path.Count]])
                        .First(i => route[i].EndsWith("Z")) )
                    (path, lookup, new string[1000000].Select(i => start).ToList()) )
                .Select(i => (ulong)i)
                .Aggregate((ulong)1, (a, b) => a > b ? 
                    (a / (ulong)BigInteger.GreatestCommonDivisor(a, b)) * b : 
                    (b / (ulong)BigInteger.GreatestCommonDivisor(a, b)) * a))
            (File.ReadAllLines(_file)[0].Select(c => c == 'L' ? 0 : 1).ToList(), 
             File.ReadAllLines(_file).Skip(2).ToDictionary(line => line[..3], line => new string[]{line[7..10], line[12..15]}) );

    public override object Part1()
    {
        var lines = this.GetLines();
        
        var path = new int[lines[0].Length];
        for(int i = 0; i < lines[0].Length; i++)
            path[i] = lines[0][i] == 'L' ? 0 : 1;

        var lookup = new Dictionary<string, string[]>();
        foreach(var line in this.GetLines().Skip(2))
            lookup.Add(line[..3], [line[7..10], line[12..15]]);

        var pathIndex = 0;
        var loc = "AAA";
        long count = 0;
        while(true)
        {
            count++;
            loc = lookup[loc][path[pathIndex]];
            
            if(loc == "ZZZ") break;

            pathIndex = (pathIndex + 1) % path.Length;
        }
        return count;
    }

    public override object Part2()
    {
        var lines = this.GetLines();
        
        var path = new int[lines[0].Length];
        for(int i = 0; i < lines[0].Length; i++)
            path[i] = lines[0][i] == 'L' ? 0 : 1;

        var lookup = new Dictionary<string, string[]>();
        foreach(var line in this.GetLines().Skip(2))
            lookup.Add(line[..3], [line[7..10], line[12..15]]);

        var locs = new List<string>();
        foreach(var key in lookup.Keys)
        {
            if(key.EndsWith("A"))
                locs.Add(key);
        }

        var counts = new int[locs.Count];
        for(int i = 0; i < locs.Count; i++)
        {
            var pathIndex = 0;
            var count = 0;
            while(true)
            {
                count++;

                locs[i] = lookup[locs[i]][path[pathIndex]];

                if(locs[i].EndsWith("Z")) break;

                pathIndex = (pathIndex + 1) % path.Length;
            }
            counts[i] = count;
        }

        var rv = (ulong)1;//LCM((ulong)counts[0], (ulong)counts[1]);
        for(int i = 0; i < counts.Length; i++)
        {
            rv = LCM(rv, (ulong)counts[i]);
        }
        return rv;
    }

    private ulong LCM(ulong a, ulong b)
    {
        var c = (ulong)BigInteger.GreatestCommonDivisor(a, b);
        var d = (ulong)BigInteger.GreatestCommonDivisor(a, b);
        return a > b ? (a / GCD(a,b))*b : (b / GCD(a,b))*a;
    }
    private ulong GCD(ulong a, ulong b)
    {
        while(a != 0 && b != 0)
        {
            if(a > b) a%=b;
            else b%=a;
        }
        return a|b;
    }
    
}
