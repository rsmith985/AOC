using System;

namespace rsmith985.AOC.Y2024.Y2024;

public class Day02 : Day
{
    //protected override bool _useDefaultTestFile => true;

    protected override string _testFile => @"D:\Google Drive\_Ryan\Code\C#\AOC\debug.txt";

    public override object Part1()
    {
        var lists = this.GetLines().SplitEachLine(long.Parse);

        var safe = 0;
        foreach(var list in lists) 
        {
            if(isListValid(list))
                safe++;
        }
        return safe;
    }

    public override object Part2()
    {
        var lists = this.GetLines().SplitEachLine(long.Parse);

        var invalid = new List<List<long>>();
        var safe = 0;
        foreach(var list in lists) 
        {
            if(isListValid(list))
                safe++;
            else
                invalid.Add(list);
        }

        foreach(var list in invalid)
        {
            if(isListValid2(list))
                safe++;
        }

        return safe;
    }

    private bool isListValid(List<long> list)
    {
        var decreasing = list[0] > list[1];
        for(int i = 1; i < list.Count; i++)
        {
            var delta = list[i] - list[i-1];
            if(decreasing && (delta < -3 || delta >=0))
                return false;
            else if(!decreasing && (delta <= 0 || delta > 3))
                return false;
        }
        return true;
    }
    private bool isListValid2(List<long> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            var copy = list.ToList();
            copy.RemoveAt(i);
            if(isListValid(copy))
                return true;
        }
        return false;
    }
}
