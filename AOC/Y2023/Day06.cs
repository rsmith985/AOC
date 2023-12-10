namespace rsmith985.AOC.Y2023;

public class Day06 : Day
{
    public override object Part1_() =>
        new Func<List<long>, List<long>, long>((times, dists) =>
            Enumerable.Range(0, times.Count).Select(i => 
                ((times[i] / 2) - Enumerable.Range(1, int.MaxValue).First(t => (times[i] - t) * t > dists[i]) + 1) * 2 + ((times[i] % 2 == 0) ? -1 : 0))
                .Aggregate((long)1, (r, i) => r*i)
        )(File.ReadAllLines(_file)[0][5..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList(), 
          File.ReadAllLines(_file)[1][9..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList());

    public override object Part2_() =>
        new Func<long, long, long>((time, dist) =>
            ((time / 2) - Enumerable.Range(1, int.MaxValue).First(t => (time - t) * t > dist) + 1) * 2 + ((time % 2 == 0) ? -1 : 0)
        )(long.Parse(File.ReadAllLines(_file)[0].Replace(" ", "")[5..]), long.Parse(File.ReadAllLines(_file)[1].Replace(" ", "")[9..]));

    public override object Part1()
    {
        var lines = this.GetLines();
        var times = lines[0][6..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToList();
        var dist = lines[1][10..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToList();

        var tot = 1;
        for(int race = 0; race < times.Count; race++)
        {
            var mid = (int)(times[race] / 2.0);
            var even = times[race] % 2 == 0;
            for(int time = 1; time < times[race] - 1; time++)
            {
                var d = (times[race] - time) * time;
                if(d > dist[race])
                {
                    var val = (mid - time + 1) * 2;
                    if(even)
                        val--;
                    tot *= val;
                    break;
                }
            }
        }
        return tot;
    }

    public override object Part2()
    {
        var times = new List<long>() {62737565};
        var dist = new List<long>() {644102312401023};
        var tot = 1;
        for(int race = 0; race < times.Count; race++)
        {
            var mid = (int)(times[race] / 2.0);
            var even = times[race] % 2 == 0;
            for(int time = 1; time < times[race] - 1; time++)
            {
                var d = (times[race] - time) * time;
                if(d > dist[race])
                {
                    var val = (mid - time + 1) * 2;
                    if(even) val--;
                    tot *= val;
                    break;
                }
            }
        }
        return tot;
    }
}
