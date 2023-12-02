
namespace rsmith985.AOC.Y2023;

public class Day01 : Day
{
    public override object Part1()
    {
        var tot = 0;
        foreach(var line in this.GetLines())
        {
            var first = -1;
            var last = -1;
            foreach (var c in line)
            {
                if (int.TryParse(c.ToString(), out int val))
                {
                    if (first < 0)
                        first = last = val;
                    else
                        last = val;
                }
            }

            if (first > 0)
                tot += (first * 10 + last);
        }

        return tot;
    }

    public override object Part2()
    {
        var lookup = new Dictionary<string, int>(){
            { "one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5},
            {"six", 6}, {"seven", 7}, {"eight", 8}, {"nine", 9}
        };

        var tot = 0;        
        foreach(var line in this.GetLines())
        {
            var first = -1;
            var last = -1;
            var txt = "";
            foreach (var c in line)
            {
                int next = -1;
                if (int.TryParse(c.ToString(), out int val))
                    next = val;
                else
                {
                    txt += c.ToString();
                    foreach (var key in lookup.Keys)
                        if (txt.EndsWith(key))
                            next = lookup[key];
                }

                if (next > 0)
                {
                    if (first < 0)
                        first = last = next;
                    else
                        last = next;
                }
            }

            if (first > 0)
                tot += (first * 10 + last);
        }

        return tot;
    }
}
