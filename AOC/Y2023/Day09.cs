using Microsoft.VisualBasic;

namespace rsmith985.AOC.Y2023;

public class Day09 : Day
{
    //protected override string _testFile => @"D:\Google Drive\_Ryan\Code\C#\AOC\AOC\test.txt";

    public override object Part1_() =>
        this.GetLines()
            .Select(i => i.Split(' ').Select(i => long.Parse(i)).ToList())
            .Sum(seq => new Func<List<long>, List<List<long>>, long>((seq, lists) =>
                    Enumerable.Range(1, lists.Count-1)
                        .Perform(i => lists[i] = lists[i-1].Zip(lists[i-1].Skip(1), (a, b) => b-a).ToList())
                        .Select(i => lists[i-1].Last())
                        .Reverse()
                        .Aggregate((long)0, (a, b) => a + b)
                )(seq, Enumerable.Range(0, seq.Count-1).Select(i => seq).ToList())
            );
    
    public override object Part2_() =>
        this.GetLines()
            .Select(i => i.Split(' ').Select(i => long.Parse(i)).Reverse().ToList())
            .Sum(seq => new Func<List<long>, List<List<long>>, long>((seq, lists) =>
                    Enumerable.Range(1, lists.Count-1)
                        .Perform(i => lists[i] = lists[i-1].Zip(lists[i-1].Skip(1), (a, b) => b-a).ToList())
                        .Select(i => lists[i-1].Last())
                        .Reverse()
                        .Aggregate((long)0, (a, b) => a + b)
                )(seq, Enumerable.Range(0, seq.Count-1).Select(i => seq).ToList())
            );

    public override object Part1()
    {
        var numbers = this.GetLines()
            .Select(i => i.Split(' ').Select(i => long.Parse(i)).ToList()).ToList();
    
        return numbers.Sum(i => calc(i));
    }
    public override object Part2()
    {
        var numbers = this.GetLines()
            .Select(i => i.Split(' ').Select(i => long.Parse(i)).Reverse().ToList()).ToList();
        
        return numbers.Sum(i => calc(i));
    }

    private long calc(IEnumerable<long> seq, bool debug = false)
    {
        if(seq.All(i => i == 0)) return 0;
        var last = seq.Last();
        var next = seq.Zip(seq.Skip(1), (a, b) => b-a).ToList();
        return last + calc(next, debug);
    }
}
