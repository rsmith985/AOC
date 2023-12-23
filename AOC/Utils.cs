using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using Emgu.CV.ML;

namespace rsmith985.AOC;

public static class Utils
{
    public static void DownloadInput(int day)
    {
        #pragma warning disable
        var client = new WebClient();
        client.Headers.Add(HttpRequestHeader.Cookie, $"session={Const.SESSION_ID}");
        client.DownloadFile(
            $"https://adventofcode.com/{Const.YEAR}/day/{day}/input", 
            $"../../../Y{Const.YEAR}/inputs/input{day}.txt");
        #pragma warning restore
    }

    public static Day GetSolution(int year, int num)
    {
        foreach(var day in getDays())
        {
            if(day.Year == year && day.Num == num)
                return day;
        }
        return null;
    }

    public static List<Day> GetSolutions(int year = 0)
    {
        year = year == 0 ? Const.YEAR : year;

        var days = new List<Day>();
        foreach(var day in getDays())
        {
            if(day.Year == year)
                days.Add(day);
        }

        return days;
    }
    public static List<Day> GetAllSolutions() => getDays().ToList();

    private static IEnumerable<Day> getDays()
    {
        var type = typeof(Day);
        var assembly = Assembly.GetExecutingAssembly();

        foreach(var t in assembly.GetTypes())
        {
            if(t.IsSubclassOf(type) && !t.IsAbstract)
                yield return (Day)Activator.CreateInstance(t);
        }
    }

    public static IEnumerable<(string k, string v)> GetKVList(string txt, char itemSep = ',', char kvSep = ' ')
    {
        var items = txt.Split(itemSep);
        foreach(var item in items)
        {
            var kv = item.Trim().Split(kvSep);
            yield return (kv[0], kv[1]);
        }
    }

    public static Dictionary<K, V> CreateDict<K, V>(params object[] items)
    {
        var dict = new Dictionary<K, V>();
        for(int i = 0; i < items.Length; i+=2)
            dict.Add((K)items[i], (V)items[i+1]);
        return dict;
    }


    public static IEnumerable<long> Range(long start, long len)
    {
        for(var i = start; i < start + len; i++)
            yield return i;
    }

}

public enum Direction
{
    N,S,E,W,NW,NE,SW,SE
}
public static class DirExt
{
    public static bool IsVert(this Direction dir) => dir == Direction.N || dir == Direction.S;
    public static bool IsHorz(this Direction dir) => dir == Direction.E || dir == Direction.W;

    public static Direction ToDirection(this char c)
    {
        return c switch
        {
            'N' or 'U' => Direction.N,
            'S' or 'D' => Direction.S,
            'E' or 'R' => Direction.E,
            'W' or 'L' => Direction.W,
            _ => throw new Exception()
        };
    }

}

