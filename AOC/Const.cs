namespace rsmith985.AOC;

public static class Const
{
    static Const()
    {
        var path = "../../../../session.txt";
        if(File.Exists(path))
            SESSION_ID = File.ReadAllText(path);
    }

    public const int YEAR = 2023;

    public static readonly string SESSION_ID;
}
