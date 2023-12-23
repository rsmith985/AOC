namespace rsmith985.AOC.Y2022;

public class Day04 : Day
{
    private List<((int, int), (int, int))> _input;

    public override void Init()
    {
        _input = new List<((int a, int b) r1, (int a, int b) r2)>();

        foreach(var line in this.GetLines())
        {
            var parts = line.Split(',');
            var item1 = parts[0].Split('-');
            var item2 = parts[1].Split('-');

            var range1 = (int.Parse(item1[0]), int.Parse(item1[1]));
            var range2 = (int.Parse(item2[0]), int.Parse(item2[1]));
            _input.Add((range1, range2));
        }
    }

    public override object Part1()
    {
        var count = 0;
        foreach(var input in _input)
        {
            var r1 = input.Item1;
            var r2 = input.Item2;
            if((r1.Item1 <= r2.Item1 && r1.Item2 >= r2.Item2) || (r2.Item1 <= r1.Item1 && r2.Item2 >= r1.Item2))
                count++;
        }
        return count;
    }

    public override object Part2()
    {
        var count = 0;
        foreach(var input in _input)
        {
            var r1 = input.Item1;
            var r2 = input.Item2;
            if((r1.Item2 >= r2.Item1 && r1.Item1 <= r2.Item2) || (r2.Item2 >= r1.Item1 && r2.Item1 <= r1.Item2))
                count++;
        }
        return count;
    }
}
