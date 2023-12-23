namespace rsmith985.AOC.Y2022;

public class Day06 : Day
{
    public override object Part1()
    {
        var line = this.GetLines()[0];

        var list = new List<char>();
        for(int i = 0; i < 4; i++)
            list.Add(line[i]);

        var idx = 4;
        for(; idx < line.Length; idx++)
        {  
            list.Add(line[idx]);
            list.RemoveAt(0);

            if(list.Distinct().Count() == 4)
                break;
        }
        return idx + 1;
    }

    public override object Part2()
    {
        var line = this.GetLines()[0];

        var list = new List<char>();
        for(int i = 0; i < 14; i++)
            list.Add(line[i]);

        var idx = 14;
        for(; idx < line.Length; idx++)
        {  
            list.Add(line[idx]);
            list.RemoveAt(0);

            if(list.Distinct().Count() == 14)
                break;
        }
        return idx + 1;
    }
}
