using System;

namespace rsmith985.AOC.Y2024;

public class Day07 : Day
{
    //protected override bool _useDefaultTestFile => true;
    public override object Part1()
    {
        var input = parseInput();
        var funcs = new List<Func<long, long, long>>(){add, mult};

        long tot = 0;
        foreach(var line in input)
        {
            if(evaluate(line.ans, line.nums, funcs))
                tot += line.ans;
        }

        return tot;
    }

    public override object Part2()
    {
        var input = parseInput();
        var funcs = new List<Func<long, long, long>>(){add, mult, concat};

        long tot = 0;
        foreach(var line in input)
        {
            if(evaluate(line.ans, line.nums, funcs))
                tot += line.ans;
        }

        return tot;
    }

    private List<(long ans, Stack<long> nums)> parseInput()
    {
        var rv = new List<(long ans, Stack<long> nums)>();
        foreach(var line in this.GetLines())
        {
            var idx = line.IndexOf(':');
            var ans = long.Parse(line[0..idx]);
            var numList = line[(idx+1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries).Select(i => long.Parse(i)).ToList();
            var nums = new Stack<long>(numList.Reverse<long>());
            rv.Add((ans, nums));
        }
        return rv;
    }

    private bool evaluate(long goal, Stack<long> nums, List<Func<long, long, long>> funcs)
    {
        if(nums.Count == 1)
            return nums.Peek() == goal;

        var a = nums.Pop();
        var b = nums.Pop();
        foreach(var f in funcs)
        {
            var ans = f(a, b);
            var newNums = nums.Clone();
            newNums.Push(ans);
            if(evaluate(goal, newNums, funcs))
                return true;
        }
        return false;
    }
    
    private long add(long a, long b)
    {
        return a + b;
    }

    private long mult(long a, long b)
    {
        return a * b;
    }

    private long concat(long a, long b)
    {
        return long.Parse($"{a}{b}");
    }
}
