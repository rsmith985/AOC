namespace rsmith985.AOC;

public static class Const
{
    public const int YEAR = 2024;

    static Const()
    {
        var path = "../../../../session.txt";
        if(File.Exists(path))
            SESSION_ID = File.ReadAllText(path);
    }

    public static readonly string SESSION_ID;
}
