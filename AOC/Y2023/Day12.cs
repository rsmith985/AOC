namespace rsmith985.AOC.Y2023;

public class Day12 : Day
{
    public override object Part1()
    {
        var input = this.GetLines().Select(i => parseLine(i)).ToList();
        return input.Sum(i => solve(i.txt, i.nums));
    }

    public override object Part2()
    {
        var input = this.GetLines().Select(i => parseLine(i)).ToList();

        long tot = 0;
        foreach(var item in input)
        {
            var txt = item.txt;
            var nums = item.nums.ToList();
            for(int i = 0; i < 4; i++)
            {
                txt += "?" + item.txt;
                nums.AddRange(item.nums);
            }
            tot += (long)solve(txt, nums.ToArray());
        };
        return tot;
    }

    private static Dictionary<string, long> _cache = new();

    private long solve(string text, int[] nums)
    {
        var txt = simplify(text);
        var key = txt + "_" + nums.Print();
        if(_cache.ContainsKey(key)) return _cache[key];

        var ans = solve_(txt, nums);
        _cache.Add(key, ans);
        return ans;
    }
    private long solve_(string text, int[] nums)
    {
        if(nums.Length == 0)
            nums = new int[]{0};

        if(text.Length < nums.Sum() + (nums.Length - 1))
            return 0;

        var index = text.IndexOf('.');
        if(index < 0) index = text.Length;
        if(text[..index].All(i => i == '#'))
        {
            if(nums[0] != index)
                return 0;
            if(index == text.Length)
                return 1;
            
            return solve(text[index..], nums[1..]);
        }

        index = text.IndexOf('?');

        var txt1 = text[..index] + "." + text[(index+1)..];
        var txt2 = text[..index] + "#" + text[(index+1)..];
        return solve(txt1, nums) + solve(txt2, nums);
    }
    private (string txt, int[] nums) parseLine(string line)
    {
        var parts = line.Split(' ');
        var nums = parts[1].Split(',').Select(i => int.Parse(i)).ToArray();
        return (parts[0], nums);
    }
    private string simplify(string text)
    {
        var curr = text;
        while(true)
        {
            var next = curr.Replace("..", ".");
            if(curr == next) break;
            curr = next;
        }
        return curr.Trim('.');
    }
}