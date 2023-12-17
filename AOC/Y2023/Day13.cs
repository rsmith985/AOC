using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace rsmith985.AOC.Y2023;

public class Day13 : Day
{

    //protected override string _testFile => @"D:\Google Drive\_Ryan\Code\C#\AOC\test1.txt";
    public override object Part1()
    {
        var lines = this.GetLines().Select(i => i.Replace('#', '0').Replace('.', '1'));
        var groups = getGroups(lines);

        return groups.Sum(i => processGroup(i));
    }

    public override object Part2()
    {
        var lines = this.GetLines().Select(i => i.Replace('#', '0').Replace('.', '1'));
        var groups = getGroups(lines);

        return groups.Sum(i => processGroup(i, 1));
    }

    private List<List<string>> getGroups(IEnumerable<string> lines)
    {
        var groups = new List<List<string>>();
        var curr = new List<string>();
        foreach(var line in lines)
        {
            if(string.IsNullOrEmpty(line))
            {
                groups.Add(curr);
                curr = new List<string>();
            }
            else
                curr.Add(line);
        }
        groups.Add(curr);
        return groups;
    }

    private int processGroup(List<string> lines, int expected = 0)
    {
        var rows = lines.Select(i => Convert.ToInt32(i, 2)).ToList();
        var cols = new List<int>();

        for(int i =0; i < lines[0].Length; i++)
        {
            var str = "";
            foreach(var line in lines)
                str += line[i];
            cols.Add(Convert.ToInt32(str, 2));
        }

        var c1 = getCount(cols, expected);
        var c2 = getCount(rows, expected) * 100;
        return c1 + c2;
    }

    private int getCount(List<int> list, int expected)
    {
        var answers = new HashSet<int>();
        var left1 = new List<int>(){list[0]};
        var right1 = new List<int>(){list[^1]};
        for(int i = 1; i < (list.Count + 1) / 2; i++)
        {
            var left2 = new List<int>();
            var right2 = new List<int>();
            for(int j = 0; j < i; j++)
            {
                left2.Add(list[i + j]);
                right2.Add(list[^(i+j+1)]);
            }
            
            var diffs = numOfDiffs(left1, left2);
            if(diffs == expected)
                return i;
            
            diffs = numOfDiffs(right1, right2);
            if(diffs == expected)
                return list.Count - i;
            
            left1.Add(list[i]);
            right1.Add(list[^(i+1)]);
        }
        return 0;
    }

    private int numOfDiffs(List<int> l1, List<int> l2)
    {
        var numDiffs = 0;
        for(int i = 0; i < l1.Count; i++)
        {
            if(l1[i] == l2[^(i+1)]) continue;

            var xor = l1[i]^l2[^(i+1)];
            var str = Convert.ToString(xor, 2);
            numDiffs += str.Count(i => i == '1');
        }
        return numDiffs;
    }
}
