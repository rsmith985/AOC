using System.Runtime.CompilerServices;

namespace rsmith985.AOC.Y2022;

public class Day02 : Day
{
    private List<(int, int)> _input;

    public override void Init()
    {
        _input = new List<(int, int)>();

        foreach(var line in this.GetLines())
        {
            var parts = line.Split(' ');
            var item2 = parts[1][0];
            var c1 = parts[0][0] - 65;
            var c2 = parts[1][0] - 88;

            _input.Add((c1, c2));
        }
    }

    public override object Part1() => score(_input);

    public override object Part2()
    {
        var modified = new List<(int, int)>();
        foreach(var item in _input)
        {
            if(item.Item2 == 0)
                modified.Add((item.Item1, (item.Item1 + 2)%3));
            else if(item.Item2 == 1)
                modified.Add((item.Item1, item.Item1));
            else
                modified.Add((item.Item1, (item.Item1 + 1)%3));
        }
        return score(modified);
    }

    private int score(List<(int, int)> data)
    {
        var tot = 0;
        foreach(var input in data)
        {
            if(input.Item1 == input.Item2)
                tot += (3 + input.Item2 + 1);
            else if(input.Item2 - input.Item1 == 1 || (input.Item2 == 0 && input.Item1 == 2))
                tot += (6 + input.Item2 + 1);
            else
                tot += input.Item2 + 1;
        }
        return tot;
    }
}
