namespace rsmith985.AOC.Y2024;

public class Day01 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var list1 = new List<int>();
        var list2 = new List<int>();
        foreach(var line in this.GetLines())
        {
            (var i1, var i2) = line.Split2();
            list1.Add(int.Parse(i1));
            list2.Add(int.Parse(i2));
        }
        list1 = list1.OrderBy(i => i).ToList();
        list2 = list2.OrderBy(i => i).ToList();

        return Enumerable.Range(0, list1.Count).Sum(i => Math.Abs(list1[i] - list2[i]));
    }

    public override object Part2()
    {
        var list1 = new List<int>();
        var set2 = new Dictionary<int, int>();
        foreach(var line in this.GetLines())
        {
            (var i1, var i2) = line.Split2();
            list1.Add(int.Parse(i1));

            var val2 = int.Parse(i2);
            set2.TryAdd(val2, 0);
            set2[val2]++;
        }

        return list1.Where(i => set2.ContainsKey(i)).Sum(i => i * set2[i]);
    }
}
