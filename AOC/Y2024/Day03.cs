using System;

namespace rsmith985.AOC.Y2024.Y2024;

public class Day03 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        return Regex.Matches(this.GetReader().ReadToEnd(), @"(mul\(\d{1,3}\,\d{1,3}\))").Sum(i => mult(i.Value));
    }

    public override object Part2()
    {
        var matches = Regex.Matches(this.GetReader().ReadToEnd(), @"(mul\(\d{1,3}\,\d{1,3}\)|do\(\)|don\'t\(\))");
        var enabled = true;
        long tot = 0;
        foreach(var match in matches)
        {
            if(match.ToString() == "do()")
            {
                enabled = true;
            }
            else if(match.ToString() == "don't()")
            {
                enabled = false;
            }
            else if(enabled)
            {
                tot += mult(match.ToString());
            }
        }
        return tot;
    }

    private long mult(string str)
    {
        (var a, var b) = str[4..^1].Split2(long.Parse, ",");
        return a*b;
    }
}
