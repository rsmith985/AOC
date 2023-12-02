namespace rsmith985.AOC.Y2023;

public class Day02 : Day
{
    public override object Part1()
    {
        var max = new Dictionary<string, int>() { {"red", 12}, {"green", 13}, {"blue", 14}};
        var data = new Dictionary<int, List<Dictionary<string, int>>>();
        foreach(var line in this.GetLines())
            parseLine(line, data);

        var tot = 0;
        foreach(var game in data.Keys)
        {
            var valid = true;
            foreach(var set in data[game])
            {
                if(!possible(max, set))
                    valid = false;
            }
            if(valid)
                tot += game;
        }

        return tot;
    }

    public override object Part2()
    {
        var data = new Dictionary<int, List<Dictionary<string, int>>>();
        foreach(var line in this.GetLines())
            parseLine(line, data);

        var tot = 0;
        foreach(var game in data.Keys)
        {
            var max = new Dictionary<string, int>(){{"red", 0}, {"green", 0}, {"blue", 0}};
            foreach(var set in data[game])
            {
                foreach(var key in max.Keys)
                {
                    if(set.ContainsKey(key) && set[key] > max[key])
                        max[key] = set[key];
                }
            }
            tot += max["red"] * max["green"] * max["blue"];
        }
        return tot;
    }

    private bool possible(Dictionary<string, int> max, Dictionary<string, int> data)
    {
        foreach(var key in max.Keys)
        {
            if(data.ContainsKey(key) && data[key] > max[key])
                return false;
        }
        return true;
    }

    private void parseLine(string line, Dictionary<int, List<Dictionary<string, int>>> data)
    {
        var idx = line.IndexOf(":");
        var gameNum = int.Parse(line[4..idx]);

        var rest = line[(idx+1)..];

        var sets = rest.Split(';');

        var list = new List<Dictionary<string, int>>();
        foreach(var set in sets)
        {
            var parts = set.Split(',');

            var dict = new Dictionary<string, int>();
            foreach(var part in parts)
            {
                var kv = part.Trim().Split(' ');
                var num = int.Parse(kv[0]);
                var color = kv[1];
                dict.Add(color, num);
            }

            list.Add(dict);
        }

        data.Add(gameNum, list);
    }
}
