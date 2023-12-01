using System.Reflection;
using rsmith985.AOC._2023;

Console.WriteLine("Running");

var type = typeof(Day);
var assembly = Assembly.GetExecutingAssembly();
var days = new List<Day>();
foreach(var t in assembly.GetTypes())
{
    if(t.IsSubclassOf(type) && !t.IsAbstract)
    {
        var day = (Day)Activator.CreateInstance(t);
        days.Add(day);
    }
}

days = days.OrderBy(i => i.Num).ToList();

var last = days.Last();
last.Run();

Console.WriteLine("Run All? Y/N");
var input = Console.ReadLine();
if(input.ToLower() == "y")
{
    foreach(var day in days)
        day.Run();
}
