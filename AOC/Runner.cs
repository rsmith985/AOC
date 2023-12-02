using System.Security.Cryptography;
using rsmith985.AOC;

if(args.Length == 0)
{
    Console.WriteLine($"Running Latest for {Const.YEAR}");

    var days = Utils.GetSolutions();
    days = days.OrderBy(i => i.Num).Reverse().ToList();

    foreach(var day in days)
    {
        if(day.Num > DateTime.Now.Day)
            continue;
        if(day.Run())
            break;
    }
}
else
{
    while(true)
    {
        var fail = int.TryParse(args[0], out int year);
        if(fail)
            goto Usage;


        Usage:
            Console.WriteLine("");
    }
}