namespace rsmith985.AOC.Y2023;

public class Day04 : Day
{
    public override object Part1_()
    {
        return File.ReadAllLines(_file)
            .Select(line => 
                (new HashSet<int>(line[(line.IndexOf(':') + 1)..line.IndexOf('|')].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i))),
                 new HashSet<int>(line[(line.IndexOf('|') + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i))))
                )
            .Select(item => item.Item2.Count(i => item.Item1.Contains(i)))
            .Where(i => i != 0)
            .Sum(i => Math.Pow(2, i - 1));
    }

    public override object Part1()
    {
        var tot = 0.0;
        foreach(var line in this.GetLines())
        {
            var index1 = line.IndexOf(':');
            var index2 = line.IndexOf('|');

            var str1 = line[(index1+1)..index2].Trim();
            var str2 = line[(index2+1)..].Trim();

            var nums1 = str1.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var nums2 = str2.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var set1 = new HashSet<int>(nums1.Select(i => int.Parse(i)));
            var set2 = new HashSet<int>(nums2.Select(i => int.Parse(i)));

            var correct = 0;
            foreach(var num in set2)
            {
                if(set1.Contains(num))
                    correct++;
            }

            var score = correct == 0 ? 0.0 : Math.Pow(2, correct - 1);
            tot += score;
        }
        return tot;
    }

    public override object Part2()
    {
        var lines = this.GetLines();
        var scores = new int[lines.Length];
        var copies = new int[lines.Length];
        for(int i = 0; i < lines.Length; i++)
        {
            copies[i]++;

            var line = lines[i];

            var index1 = line.IndexOf(':');
            var index2 = line.IndexOf('|');

            var str1 = line[(index1+1)..index2].Trim();
            var str2 = line[(index2+1)..].Trim();

            var nums1 = str1.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var nums2 = str2.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var set1 = new HashSet<int>(nums1.Select(i => int.Parse(i)));
            var set2 = new HashSet<int>(nums2.Select(i => int.Parse(i)));

            var correct = 0;
            foreach(var num in set2)
            {
                if(set1.Contains(num))
                    correct++;
            }

            var numCopies = copies[i];

            for(int j = 1; j <= correct; j++)
                copies[i+j] += numCopies;
        }
        
        return copies.Select(i => (double)i).Sum();
    }
}
