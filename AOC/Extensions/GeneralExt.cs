using System.Reflection.Metadata.Ecma335;

namespace rsmith985.AOC.Y2023;

public static class Ext
{


    public static IEnumerable<T> Perform<T>(this IEnumerable<T> items, Action<T> action) 
    { 
        foreach(var item in items)  
            action(item);
        return items;
    }
    public static string Print<T>(this IEnumerable<T> objs, char sep = ',')
        => string.Join(sep, objs.ToArray());
    public static string PrintLines(this string[] input)
        => string.Join('\n', input);

    public static string GetCol(this string[] lines, int col)
        => new string(lines.Select(i => i[col]).ToArray());

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
}
