using System;

namespace rsmith985.AOC.Y2024;

public class Day11 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var maxIterations = 25;
        var initStones = this.GetReader().ReadToEnd().Split<long>(long.Parse);

        var dict = new Dictionary<long, Stone>();
        var toDefine = new List<Stone>();
        foreach(var num in initStones)
        {
            var stone = new Stone(num);
            dict.Add(num, stone);
            toDefine.Add(stone);
        }

        for(int i = 0; i < maxIterations; i++)
        {
            var list = toDefine.ToList();
            toDefine.Clear();
            foreach(var stone in list)
            {
                var num = stone.Num;
                var nextNums = getNextNums(stone.Num);

                var nextStones = new List<Stone>();
                foreach(var n in nextNums)
                {
                    if(!dict.ContainsKey(n))
                    {
                        var s = new Stone(n);
                        dict.Add(n, s);
                        toDefine.Add(s);
                    }
                    nextStones.Add(dict[n]);
                }
                stone.SetNext(nextStones);
            }
            dict.Values.Where(i => i.NextSet != null).OrderBy(i => i.Depth).Perform(i => i.ComputeNext());
        }

        return initStones.Sum(i => dict[i].NumberAfterEachIteration[maxIterations]);
    }

    public override object Part2()
    {
        var maxIterations = 75;
        var initStones = this.GetReader().ReadToEnd().Split<long>(long.Parse);

        var dict = new Dictionary<long, Stone>();
        var toDefine = new List<Stone>();
        foreach(var num in initStones)
        {
            var stone = new Stone(num);
            dict.Add(num, stone);
            toDefine.Add(stone);
        }

        for(int i = 0; i < maxIterations; i++)
        {
            var list = toDefine.ToList();
            toDefine.Clear();
            foreach(var stone in list)
            {
                var num = stone.Num;
                var nextNums = getNextNums(stone.Num);

                var nextStones = new List<Stone>();
                foreach(var n in nextNums)
                {
                    if(!dict.ContainsKey(n))
                    {
                        var s = new Stone(n);
                        dict.Add(n, s);
                        toDefine.Add(s);
                    }
                    nextStones.Add(dict[n]);
                }
                stone.SetNext(nextStones);
            }
            dict.Values.Where(i => i.NextSet != null).OrderBy(i => i.Depth).Perform(i => i.ComputeNext());
        }

        return initStones.Sum(i => dict[i].NumberAfterEachIteration[maxIterations]);
    }


    private List<long> getNextNums(long num)
    {
        if(num == 0)
            return new List<long>(){1};
        
        var str = num.ToString();
        if(str.Length%2 == 0)
        {
            var num1 = long.Parse(str.Substring(0,str.Length/2));
            var num2 = long.Parse(str.Substring(str.Length/2));
            return new List<long>(){num1,num2};
        }

        return new List<long>(){num*2024};
    }

    class Stone
    {
        public long Num{get;set;}
        public List<Stone> NextSet{get;private set;}
        public List<long> NumberAfterEachIteration = new List<long>();
        public int Depth = 0;

        public Stone(long num)
        {
            this.Num = num;
            this.NumberAfterEachIteration.Add(1);
        }

        public void SetNext(List<Stone> next)
        {
            this.NextSet = next;
        }

        public void ComputeNext()
        {
            var sum = this.NextSet.Sum(i => i.NumberAfterEachIteration[this.Depth]);
            //Console.WriteLine("Computing Next: " + this.Num + " " + this.Depth + " " + sum);
            this.NumberAfterEachIteration.Add(sum);
            this.Depth++;
        }
    }
}
