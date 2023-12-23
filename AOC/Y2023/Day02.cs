namespace rsmith985.AOC.Y2023;

public class Day02 : Day
{
    public Day02()
    {
    }

    public override object Part1_() =>
        File.ReadAllLines(_file)
            .Where(line =>
                Regex.Matches(line, "\\d+\\sblue").All(i => int.Parse(i.Value[0..^5]) <= 14) &&
                Regex.Matches(line, "\\d+\\sred").All(i => int.Parse(i.Value[0..^4]) <= 12) &&
                Regex.Matches(line, "\\d+\\sgreen").All(i => int.Parse(i.Value[0..^6]) <= 13))
            .Sum(line => int.Parse(line[5..line.IndexOf(':')]));  

    public override object Part2_() =>
        File.ReadAllLines(_file)
            .Sum(line =>
                Regex.Matches(line, "\\d+\\sblue").Max(i => int.Parse(i.Value[0..^5])) *
                Regex.Matches(line, "\\d+\\sred").Max(i => int.Parse(i.Value[0..^4])) *
                Regex.Matches(line, "\\d+\\sgreen").Max(i => int.Parse(i.Value[0..^6]))); 

    public override object Part1()
    {
        var max = Utils.CreateDict<string, int>("red", 12, "green", 13, "blue", 14);
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
            var max = Utils.CreateDict<string, int>("red", 0, "green", 0, "blue", 0);
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
        var matches = Regex.Matches(line, "\\d\\sblue").Select(i => i.Value[0..^5]).ToList();

        var idx = line.IndexOf(":");
        var gameNum = int.Parse(line[4..idx]);

        var rest = line[(idx+1)..];
        var sets = rest.Split(';');

        var list = new List<Dictionary<string, int>>();
        foreach(var set in sets)
        {
            var items = Utils.GetKVList(set, ',', ' ');
            var dict = items.ToDictionary(i => i.v, i => int.Parse(i.k));
            list.Add(dict);
        }

        data.Add(gameNum, list);
    }
}
