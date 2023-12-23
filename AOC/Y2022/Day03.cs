using rsmith985.AOC.Y2023;

namespace rsmith985.AOC.Y2022;

public class Day03 : Day
{
    private List<(string l, string r)> _input;
    private List<(HashSet<int>, HashSet<int>)> _intSets;

    private List<HashSet<int>> _set2;

    public override void Init()
    {
        _input = new List<(string l, string r)>();
        _intSets = new List<(HashSet<int>, HashSet<int>)>();
        _set2 = new List<HashSet<int>>();

        foreach(var line in this.GetLines())
        {
            if(line.Length %2 == 1) throw new Exception();
            var half = line.Length /2;
            var str1 = line[..half];
            var str2 = line[half..];
            _input.Add((str1, str2));

            var set1 = new HashSet<int>();
            var set2 = new HashSet<int>();
            for(int i = 0; i < half; i++)
            {
                set1.Add(toNum(str1[i]));
                set2.Add(toNum(str2[i]));
            }
            _intSets.Add((set1, set2));

            var set = new HashSet<int>();
            for(int i = 0; i < line.Length; i++)
                set.Add(toNum(line[i]));
            _set2.Add(set);
        }
    }

    public override object Part1()
    {
        var tot = 0;
        for(int i = 0; i < _intSets.Count; i++)
        {
            var item = _intSets[i];
            tot += item.Item1.Intersect(item.Item2).Sum();
        }
        return tot;
    }

    public override object Part2()
    {
        var tot = 0;
        for(int i = 0; i < _set2.Count; i = i + 3)
        {
            var set = _set2[i].Intersect(_set2[i+1]);
            set = set.Intersect(_set2[i+2]);
            tot += set.Sum();
        }
        return tot;
    }


    private int toNum(char c)
    {
        if(c > 96)
            return c - 96;
        return c - 64 + 26;
    }
}
