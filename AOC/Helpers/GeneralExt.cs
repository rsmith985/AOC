using System.Drawing;

namespace rsmith985.AOC;

public static class Ext
{
    public static IEnumerable<T> Perform<T>(this IEnumerable<T> items, Action<T> action) 
    { 
        foreach(var item in items)  
            action(item);
        return items;
    }

    #region Min/Max Object
    public static T MinObject<T>(this IEnumerable<T> items, Func<T, double> func)
    {
        T rv = default;
        var min = double.MaxValue;
        foreach(var item in items)
        {
            var val = func(item);
            if(val < min)
            {
                rv = item;
                min = val;
            }
        }
        return rv;
    }
    public static T MaxObject<T>(this IEnumerable<T> items, Func<T, double> func)
    {
        T rv = default;
        var max = double.MinValue;
        foreach(var item in items)
        {
            var val = func(item);
            if(val > max)
            {
                rv = item;
                max = val;
            }
        }
        return rv;
    }
    #endregion

    #region Input Lines Operations
    /// <summary>
    /// Gets all characters in a column returned as a string
    /// </summary>
    public static string GetCol(this string[] lines, int col)
        => new string(lines.Select(i => i[col]).ToArray());

    public static void SetCol(this string[] lines, int col, string val)
    {
        for(int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            lines[y] = line[..col] + val[y] + line[(col+1)..];
        }
    }
    
    public static void SetCol(this string[] lines, int col, char c)
    {
        for(int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            lines[y] = line[..col] + c + line[(col+1)..];
        }
    }
    public static List<string> PadBorder(this string[] lines, char pad = ' ')
    {
        var w = lines[0].Length;
        var rv = new List<string>();

        var first = "";
        var last = "";
        for(int i = 0; i < w + 2; i++)
        {
            first += pad;
            last += pad;
        }

        rv.Add(first);
        foreach(var line in lines)
            rv.Add(pad + line + pad);
        rv.Add(last);

        return rv;
    }
    public static List<string> GetAllForwardDiagonals(this string[] lines)
    {
        var rv = new List<string>();
        var w = lines[0].Length;
        for(int x = 0; x < w; x++)
        {
            var str = "";
            for(int y = 0; y <= x; y++)
            {
                str += lines[y][x - y];
            }
            rv.Add(str);
        }
        for(int y = 1; y < lines.Length; y++)
        {
            var str = "";
            var yy = 0;
            for(int x = w-1; x >= y; x--)
            {
                str += lines[y + yy][x];
                yy++;
            }
            rv.Add(str);
        }
        return rv;
    }
    public static List<string> GetAllBackwardsDiagonals(this string[] lines)
    {
        var rv = new List<string>();
        var w = lines[0].Length;
        var h = lines.Length;
        for(int x = w-1; x >= 0; x--)
        {
            var str = "";
            var num = w-x;
            for(int y = 0; y < num; y++)
            {
                str += lines[y][x + y];
            }
            rv.Add(str);
        }
        for(int y = 1; y < h; y++)
        {
            var str = "";
            var yy = 0;
            for(int x = 0; x < w-y; x++)
            {
                str += lines[y + yy][x];
                yy++;
            }
            rv.Add(str);
        }
        return rv;
    }
    public static List<List<string>> SplitEachLine(this IList<string> lines, string splitOn = " ", StringSplitOptions splitOps = StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries)
    {
        var rv = new List<List<string>>();
        foreach(var line in lines)
        {
            var items = line.Split(splitOn, splitOps);
            rv.Add(items.ToList());
        }
        return rv;
    }
    public static List<List<T>> SplitEachLine<T>(this IList<string> lines, Func<string, T> op, string splitOn = " ", StringSplitOptions splitOps = StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries)
    {
        var rv = new List<List<T>>();
        foreach(var line in lines)
        {
            var items = line.Split(splitOn, splitOps);
            rv.Add(items.Select(i => op(i)).ToList());
        }
        return rv;
    }
    #endregion

    #region 2D Char Array
    public static string[] ToStringArray(this char[,] array)
    {
        var rv = new string[array.GetLength(1)];

        for(int y = 0;y < array.GetLength(1); y++)
        {
            var str = "";
            for(int x = 0; x < array.GetLength(0); x++)
                str += array[x,y];
            rv[y] = str;
        }
        return rv;
    }
    
    public static char[,] To2DArray(this IList<string> lines)
    {
        return To2DArray(lines, i => i);
    }
    public static T[,] To2DArray<T>(this IList<string> lines, Func<char, T> op)
    {
        var rv = new T[lines[0].Length, lines.Count];
        for(int y = 0; y < rv.GetLength(1); y++)
        {
            for(int x = 0; x < rv.GetLength(0); x++)
            {
                rv[x,y] = op(lines[y][x]);
            }
        }
        return rv;
    }
    #endregion

    #region Grid
    public static IEnumerable<Point> GetPointsInGrid(this Size s)
    {
        for(int x = 0; x < s.Width; x++)
        {
            for(int y = 0; y < s.Height; y++)
            {
                yield return new Point(x,y);
            }
        }
    }

    public static Size GetSize<T>(this T[,] array) => new Size(array.GetLength(0), array.GetLength(1));
    public static Size GetSize(this IList<string> input) => new Size(input[0].Length, input.Count);
    public static char GetValueAt(this IList<string> array, Point p) => array[p.Y][p.X];
    public static T Get<T>(this T[,] array, Point p) => array[p.X, p.Y];
    public static void Set<T>(this T[,] array, Point p, T val) => array[p.X, p.Y] = val;
    public static IEnumerable<T> LoopAll<T>(this T[,] array)
    {
        for(int x = 0; x < array.GetLength(0); x++)
        {
            for(int y = 0; y < array.GetLength(1); y++)
                yield return array[x,y];   
        }
    }
    #endregion

    #region To Tuple
    public static (T, T) ToTuple2<T>(this IEnumerable<T> items)
    {
        var list = items.ToList();
        return (list[0], list[1]);
    }
    public static (T, T, T) ToTuple3<T>(this IEnumerable<T> items)
    {
        var list = items.ToList();
        return (list[0], list[1], list[2]);
    }
    public static (T, T, T, T) ToTuple4<T>(this IEnumerable<T> items)
    {
        var list = items.ToList();
        return (list[0], list[1], list[2], list[3]);
    }

    public static IEnumerable<T> Split<T>(this string str, Func<string, T> func, string splitStr = " ")
    {
        var vals = str.Split(splitStr, StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);
        foreach(var val in vals)
            yield return func(val);
    }
    public static (string a, string b) Split2(this string str, string splitStr = " ")
    {
        var vals = str.Split(splitStr, StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);
        return (vals[0], vals[1]);
    }
    public static (T a, T b) Split2<T>(this string str, Func<string, T> func, string splitStr = " ")
    {
        var vals = str.Split(splitStr, StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);
        return (func(vals[0]), func(vals[1]));
    }
    #endregion

    #region Debugging
    public static string Print<T>(this IEnumerable<T> objs, char sep = ',')
        => string.Join(sep, objs.ToArray());
    public static string PrintLines(this string[] input)
        => string.Join('\n', input);
    #endregion

    public static Stack<T> Clone<T>(this Stack<T> input)
    {
        var array = new T[input.Count];
        input.CopyTo(array, 0);
        Array.Reverse(array);
        return new Stack<T>(array);
    }
}
