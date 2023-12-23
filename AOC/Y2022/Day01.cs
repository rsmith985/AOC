namespace rsmith985.AOC.Y2022;

public class Day01 : Day
{
    private List<List<int>> _items;

    public override void Init()
    {
        _items = new List<List<int>>();
        var curr = new List<int>();
        foreach(var line in this.GetLines())
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                _items.Add(curr);
                curr = new List<int>();
            }
            else
            {
                curr.Add(int.Parse(line));
            }
        }
        _items.Add(curr);
    }

    public override object Part1()
    {
        return _items.Select(i => i.Sum()).Max();
    }

    public override object Part2()
    {
        return _items.Select(i => i.Sum()).OrderBy(i => i).Reverse().Take(3).Sum();
    }
}
