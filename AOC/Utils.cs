using System.Net;
using System.Reflection;

namespace rsmith985.AOC;

public class Utils
{
    public static void DownloadInput(int day)
    {
        #pragma warning disable
        var client = new WebClient();
        client.Headers.Add(HttpRequestHeader.Cookie, $"session={Const.SESSION_ID}");
        client.DownloadFile(
            $"https://adventofcode.com/{Const.YEAR}/day/{day}/input", 
            $"../../../{Const.YEAR}/inputs/input{day}.txt");
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
}
