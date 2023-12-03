using rsmith985.AOC;

if(args.Length == 0)
{
    Console.WriteLine($"Running Latest for Year {Const.YEAR}");

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
    var input = args;
    while(true)
    {
        if(input.Length < 1 || input.Length > 2)
            goto Usage;

        if(input[0] == "exit")
            break;

        var year = -1;
        var day = -1;

        if(args[0] != "all")
        {
            if(!int.TryParse(input[0], out year))
                goto Usage;
        }

        if(year >= 0 && args.Length == 2)
        {
            if(!int.TryParse(input[1], out day))
                goto Usage;
        }

        var days = 
            year < 0 ? Utils.GetAllSolutions() : 
            day < 0 ? Utils.GetSolutions(year) :
            new List<Day>(){Utils.GetSolution(year, day)};

        foreach(var item in days.OrderByDescending(i => i.Year).ThenBy(i => i.Num))
            item.Run();

        Usage:
            Console.WriteLine("Usage: AOC.exe [year [day]] | all | exit");
            var data = Console.ReadLine();
            input = data.Split(' ');
    }
}