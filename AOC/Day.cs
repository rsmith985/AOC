using System.Net;

namespace rsmith985.AOC;

public abstract class Day
{
    public int Num { get; }

    public int Year{ get; }

    private string _file => $"../../../{this.Year}/inputs/input{this.Num}.txt";

    public Day()
    {
        var type = this.GetType();

        var name = type.Name;
        this.Num = int.Parse(name[3..]);

        var space = type.Namespace;
        this.Year = int.Parse(space[^4..]);
    }

    public abstract object Part1();
    public abstract object Part2();

    public bool Run()
    {
        if(!File.Exists(_file))
        {
            Utils.DownloadInput(this.Num);
            if(!File.Exists(_file))
            {
                Console.WriteLine($"Couldn't find inputs/input{this.Num}.txt");
                return false;
            }
        }

        var dayStr = $"Day {this.Num.ToString().PadLeft(2, '0')}";
        try
        {
            var part1 = this.Part1();
            Console.WriteLine($"{dayStr} | Part 1: {part1}");
            try
            {
                var part2 = this.Part2();
                Console.WriteLine($"{dayStr} | Part 2: {part2}");
            } 
            catch(NotImplementedException) { }
        }
        catch(NotImplementedException) 
        { 
            Console.WriteLine($"{dayStr}: Not Implemented Yet");
            return false;
        }
        Console.WriteLine();
        return true;
    }

    protected StreamReader GetReader() => new StreamReader(_file);
    protected string[] GetLines() => File.ReadAllLines(_file);

    protected List<string[]> GetWordsPerLine()
    {
        var rv = new List<string[]>();
        foreach(var line in GetLines())
        {
            var words = line.Split(' ');
            rv.Add(words);
        }
        return rv;
    }

    protected List<int> GetIntList()
    {
        var rv = new List<int>();
        foreach(var line in GetLines())
            rv.Add(int.Parse(line));
        return rv;
    }
}
