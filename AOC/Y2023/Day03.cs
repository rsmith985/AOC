using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace rsmith985.AOC.Y2023;

public class Day03 : Day
{
    public override object Part1()
    {
        var init = this.GetLines();
        var lines = Utils.PadBorder(init, '.');

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

    private bool isSymbol(List<string> lines, int x, int y)
    {
        var c = lines[y][x];
        var isPeriod = c == '.';
        var isNum = c >= 0 && c <= 9;
        return !isPeriod && !isNum;
    }

    public override object Part2()
    {
        var init = this.GetLines();
        var lines = Utils.PadBorder(init, '.');

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
                        Console.WriteLine(list[0] + " " + list[1]);
                        Console.WriteLine();
                        tot += list[0]*list[1];
                    }
                    else{
                        Console.WriteLine("Failed");
                    }

                }
            }
        }
        
        return tot;
    }
}
