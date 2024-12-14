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

    public static Direction Rotate4_CW(this Direction dir)
    {
        if(dir == Direction.N) return Direction.E;
        else if(dir == Direction.E) return Direction.S;
        else if(dir == Direction.S) return Direction.W;
        else if(dir == Direction.W) return Direction.N;
        throw new Exception();
    }
    public static Direction Rotate4_CCW(this Direction dir)
    {
        if(dir == Direction.N) return Direction.W;
        else if(dir == Direction.W) return Direction.S;
        else if(dir == Direction.S) return Direction.E;
        else if(dir == Direction.E) return Direction.N;
        throw new Exception();
    }
    public static Direction GetOpposite(this Direction dir)
    {
        if(dir == Direction.N) return Direction.S;
        else if(dir == Direction.W) return Direction.E;
        else if(dir == Direction.S) return Direction.N;
        else if(dir == Direction.E) return Direction.W;
        else if(dir == Direction.NE) return Direction.SW;
        else if(dir == Direction.NW) return Direction.SE;
        else if(dir == Direction.SE) return Direction.NW;
        else if(dir == Direction.SW) return Direction.NE;
        throw new Exception();
    }
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

    public static Point GetNeighbor(this Point p, Direction d, int dist = 1)
    {
        return d switch
        {
            Direction.N => p.Plus(0, -dist),
            Direction.S => p.Plus(0, dist),
            Direction.E => p.Plus(dist, 0),
            Direction.W => p.Plus(-dist, 0),
            Direction.NW => p.Plus(-dist, -dist),
            Direction.NE => p.Plus(dist, -dist),
            Direction.SW => p.Plus(-dist, dist),
            Direction.SE => p.Plus(dist, dist),
            _ => p
        };
    }
    public static (int, int) GetNeighbor(this (int x, int y) p, Direction d, int dist = 1)
    {
        return GetNeighbor(new Point(p.x, p.y), d, dist).ToTuple();
    }
    public static (long x, long y) GetNeighbor(this (long x, long y) p, Direction d, long dist = 1)
    {
        return d switch
        {
            Direction.N => (p.x, p.y-dist),
            Direction.S => (p.x, p.y+dist),
            Direction.E => (p.x+dist, p.y),
            Direction.W => (p.x-dist, p.y),
            _ => p
        };
    }

    public static IEnumerable<Point> GetNeighbors4(this Point p, Rectangle? bounds = null)
    {
        if(bounds == null)
        {
            yield return p.GetNeighbor(Direction.N);
            yield return p.GetNeighbor(Direction.S);
            yield return p.GetNeighbor(Direction.E);
            yield return p.GetNeighbor(Direction.W);
        }
        else
        {
            var rect = bounds.Value;

            var n = p.GetNeighbor(Direction.N);
            if(n.Y >= rect.Y) yield return n;

            var s = p.GetNeighbor(Direction.S);
            if(s.Y < rect.Y + rect.Height) yield return s;

            var e = p.GetNeighbor(Direction.E);
            if(e.X < rect.X + rect.Width) yield return e;

            var w = p.GetNeighbor(Direction.W);
            if(w.X >= rect.X) yield return w;
        }
    }

}

