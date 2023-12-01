namespace rsmith985.AOC._2023;

public abstract class Day
{
    public int Num { get; }

    private string _file => $"../../../inputs/input{this.Num}.txt";

    public Day()
    {
        var type = this.GetType();
        var name = type.Name;
        this.Num = int.Parse(name[3..]);
    }

    public abstract object Part1();
    public abstract object Part2();

    public void Run()
    {
        if(!File.Exists(_file))
        {
            Console.WriteLine($"Couldn't find inputs/input{this.Num}.txt");
            Environment.Exit(1);
        }

        var part1 = this.Part1();
        var part2 = this.Part2();
        Console.WriteLine($"Day {this.Num.ToString().PadLeft(2, '0')} | Part 1: {part1}");
        Console.WriteLine($"Day {this.Num.ToString().PadLeft(2, '0')} | Part 2: {part2}");
        Console.WriteLine();
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
