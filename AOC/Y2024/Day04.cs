using System;

namespace rsmith985.AOC.Y2024;

public class Day04 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var input = this.GetLines();
        
        var tot = 0;
        foreach(var line in input)
        {
            tot += Regex.Matches(line, "XMAS").Count;
            tot += Regex.Matches(line, "SAMX").Count;
        }
        for(int x = 0; x < input[0].Length; x++)
        {
            var col = input.GetCol(x);
            tot += Regex.Matches(col, "XMAS").Count;
            tot += Regex.Matches(col, "SAMX").Count;
        }
        foreach(var diag in input.GetAllBackwardsDiagonals())
        {
            tot += Regex.Matches(diag, "XMAS").Count;
            tot += Regex.Matches(diag, "SAMX").Count;
        }
        foreach(var diag in input.GetAllBackwardsDiagonals())
        {
            tot += Regex.Matches(diag, "XMAS").Count;
            tot += Regex.Matches(diag, "SAMX").Count;
        }
        return tot;
    }

    public override object Part2()
    {
        var input = this.GetLines();
        
        var tot = 0;
        for(int x = 1; x < input[0].Length - 1; x++)
        {
            for(int y = 1; y < input.Count() - 1; y++)
            {
                if(input[y][x] == 'A')
                {
                    var c1 = input[y-1][x-1];
                    var c2 = input[y+1][x+1];
                    var c3 = input[y+1][x-1];
                    var c4 = input[y-1][x+1];
                    if(((c1 == 'M' || c2 == 'M') && (c1 == 'S' || c2 == 'S')) && ((c3 == 'M' || c4 == 'M') && (c3 == 'S' || c4 == 'S')))
                        tot++;
                }
            }
        }
        return tot;
    }
}
