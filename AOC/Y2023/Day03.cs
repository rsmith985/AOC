namespace rsmith985.AOC.Y2023;

public class Day03 : Day
{
    public override object Part1_() =>
        new Func<string, long>(str =>
            Regex.Matches(str, "[0-9]+")
                .Where(i => 
                    new List<int>(){i.Index-1, i.Index+i.Length}
                        .Concat(Enumerable.Range(i.Index-142, i.Length+2))
                        .Concat(Enumerable.Range(i.Index+140, i.Length+2))
                    .Select(i => (str[i], i))
                    .Any(i => !(i.Item1 == '.' || (i.Item1 >= 48 && i.Item1 <= 57)))
                ).Sum(i => int.Parse(i.Value))
        )(new string('.', 141) + string.Join('.', File.ReadAllLines(_file)) + new string('.', 141));

    public override object Part2_() =>
        new Func<string, Dictionary<int, string>, long>((str, matches) =>
            Enumerable.Range(0, str.Length)
                .Select(i => (i, str[i]))
                .Where(i => i.Item2 == '*')
                .Select(i => new HashSet<int>(
                    new List<int>(){i.i - 1, i.i + 1, i.i - 142, i.i - 141, i.i - 140, i.i + 140, i.i + 141, i.i + 142}
                        .Where(i => matches.ContainsKey(i))
                        .Select(i => int.Parse(matches[i]))
                    ))
                .Where(i => i.Count == 2)
                .Sum(i => (long)i.First() * (long)i.Last())
        )(new string('.', 141) + string.Join('.', File.ReadAllLines(_file)) + new string('.', 141),
            Regex.Matches(new string('.', 141) + string.Join('.', File.ReadAllLines(_file)) + new string('.', 141), "[0-9]+")
            .SelectMany(i => Enumerable.Range(i.Index, i.Length).Select(j => (j, i.Value)))
            .ToDictionary(i => i.j, i => i.Value));

    public override object Part1()
    {
        var init = this.GetLines();
        var lines = init.PadBorder('.');

        var w = lines[0].Length;
        var h = lines.Count;

        var tot = 0;
        var allNums = new HashSet<int>();
        for(int y = 1; y < h - 1; y++)
        {
            var currNum = "";
            for(int x = 1; x < w; x++)
            {
                var c = lines[y][x];
                var isNum = c >= 48 && c <= 57;

                if(isNum)
                {
                    currNum += c;
                }
                else if(currNum != "")
                {
                    var num = int.Parse(currNum);

                    var foundSymbol = isSymbol(lines, x, y) || isSymbol(lines, x - currNum.Length - 1, y);

                    for(int xx = -1; xx < currNum.Length + 1; xx++)
                    {
                        var xxx = x + xx - currNum.Length;
                        if(isSymbol(lines, xxx, y-1) || isSymbol(lines, xxx, y+1))
                            foundSymbol = true;
                    }
                    
                    if(foundSymbol)
                        tot += num;
                    
                    currNum = "";
                }
            }
        }
        return tot;
    }

    public override object Part2()
    {
        var init = this.GetLines();
        var lines = init.PadBorder('.');

        var w = lines[0].Length;
        var h = lines.Count;

        var nums = new int[w,h];

        for(int y = 1; y < h - 1; y++)
        {
            var currNum = "";
            for(int x = 1; x < w; x++)
            {
                var c = lines[y][x];
                var isNum = c >= 48 && c <= 57;

                if(isNum)
                {
                    currNum += c;
                }
                else if(currNum != "")
                {
                    var num = int.Parse(currNum);

                    for(int i = 1; i <= currNum.Length; i++)
                    {
                        var xxx = x - i;
                        nums[xxx,y] = num;
                    }
                    currNum = "";
                }
            }
        }

        var tot = 0.0;
        for(int y = 1; y < h - 1; y++)
        {
            for(int x = 1; x < w; x++)
            {
                var c = lines[y][x];
                if(c == '*')
                {
                    var set = new HashSet<int>();
                    set.Add(nums[x-1, y-1]);
                    set.Add(nums[x+0, y-1]);
                    set.Add(nums[x+1, y-1]);
                    set.Add(nums[x-1, y+0]);
                    set.Add(nums[x+0, y+0]);
                    set.Add(nums[x+1, y+0]);
                    set.Add(nums[x-1, y+1]);
                    set.Add(nums[x+0, y+1]);
                    set.Add(nums[x+1, y+1]);
                    set.Remove(0);

                    if(set.Count() == 2)
                    {
                        var list = set.ToList();
                        tot += list[0] * list[1];
                    }

                }
            }
        }
        
        return tot;
    }

    private bool isSymbol(List<string> lines, int x, int y)
    {
        var c = lines[y][x];
        var isPeriod = c == '.';
        var isNum = c >= 0 && c <= 9;
        return !isPeriod && !isNum;
    }
}
