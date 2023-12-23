using System.Runtime.CompilerServices;
using System.Transactions;
using Emgu.CV.Bioinspired;

namespace rsmith985.AOC.Y2023;

public class Day19 : Day
{
    //protected override bool _useDefaultTestFile => true;

    private Dictionary<string, Condition> _lookup;

    private List<Dictionary<char, int>> _input;

    public override void Init()
    {
        _lookup = new Dictionary<string, Condition>();
        _input = new List<Dictionary<char, int>>();

        var readingConditions = true;
        foreach(var line in this.GetLines())
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                readingConditions = false;
                continue;
            }

            if(readingConditions)
            {
                foreach(var condition in lineToConditions(line))
                    _lookup.Add(condition.Key, condition);
            }
            else
            {
                _input.Add(lineToInputValue(line));
            }
        }
    }

    public override object Part1()
    {
        var tot = 0;
        foreach(var input in _input)
        {
            if(isAccepted(input))
                tot += input.Values.Sum();
        }

        return tot;
    }

    public override object Part2()
    {
        var initRanges = new Dictionary<char, (int min, int max)>()
        {
            {'x', (1, 4000)},
            {'m', (1, 4000)},
            {'a', (1, 4000)},
            {'s', (1, 4000)}
        };

        var root = new ConditionRange(_lookup["in"], initRanges);

        var queue = new Queue<ConditionRange>();
        queue.Enqueue(root);

        var acceptedRanges = new List<Dictionary<char, (int min, int max)>>();
        while(queue.Any())
        {
            var curr = queue.Dequeue();
            var condition = curr.Condition;
            (var tRange, var fRange) = curr.SplitRange();

            if(condition.TrueKey == "A")
            {
                acceptedRanges.Add(tRange);   
            }
            else if(condition.TrueKey != "R")
            {
                var item = new ConditionRange(_lookup[curr.Condition.TrueKey], tRange);
                queue.Enqueue(item);
            }

            if(condition.FalseKey == "A")
            {
                acceptedRanges.Add(fRange);
            }
            else if(condition.FalseKey != "R")
            {
                var item = new ConditionRange(_lookup[curr.Condition.FalseKey], fRange);
                queue.Enqueue(item);
            }
        }

        long tot = 0;
        foreach(var range in acceptedRanges)
        {
            var x = range['x'].max - range['x'].min + 1;
            var m = range['m'].max - range['m'].min + 1;
            var a = range['a'].max - range['a'].min + 1;
            var s = range['s'].max - range['s'].min + 1;

            tot += ((long)x * (long)m * (long)a * (long)s);
        }

        return tot;
    }

    private bool isAccepted(Dictionary<char, int> input)
    {
        var curr = _lookup["in"];
        while(true)
        {
            var nextKey = curr.nextCondition(input);
            if(nextKey == "A")
                return true;
            if(nextKey == "R")
                return false;
            
            curr = _lookup[nextKey];
        }
    }

    private IEnumerable<Condition> lineToConditions(string line)
    {
        var idx = line.IndexOf("{");
        var key = line[..idx];
        var txt = line[(idx+1)..^1];

        var parts = txt.Split(',');

        string falseKey = parts[^1];
        for(int i = parts.Length - 2; i >= 0; i--)
        {
            var name = i == 0 ? key : $"{key}_{i+1}";
            var item = parts[i];
            var varName = item[0];
            var lessThan = item[1] == '<';
            var number = int.Parse(item[2..item.IndexOf(':')]);
            var trueKey = item[(item.IndexOf(':')+1)..];

            yield return new Condition(name, varName, lessThan, number, trueKey, falseKey);

            falseKey = name;
        }
    }

    private Dictionary<char, int> lineToInputValue(string line)
    {
        var txt = line.Trim('{', '}');
        var parts = txt.Split(',');

        var dict = new Dictionary<char, int>();
        foreach(var part in parts)
        {
            var key = part[0];
            var value = int.Parse(part[2..]);
            dict.Add(key, value);
        }
        return dict;
    }
}

class Condition
{
    public string Key{get;}
    public char Variable{get;}
    public bool LessThan{get;}
    public int Value{get;}
    public string TrueKey{get;}
    public string FalseKey{get;}

    public Condition(string key, char variable, bool lessThan, int value, string tKey, string fKey)
    {
        this.Key = key;
        this.Variable = variable;
        this.LessThan = lessThan;
        this.Value = value;
        this.TrueKey = tKey;
        this.FalseKey = fKey;
    }

    public string nextCondition(Dictionary<char, int> input)
    {
        var val = input[this.Variable];
        if(this.LessThan)
            return val < this.Value ? this.TrueKey : this.FalseKey;
        else
            return val > this.Value ? this.TrueKey : this.FalseKey;
    }

    public override string ToString() => $"{this.Key}: {this.Variable} {(this.LessThan ? "<" : ">")} {this.Value} ? {this.TrueKey} : {this.FalseKey}";
}

class ConditionRange
{
    public Dictionary<char, (int min, int max)> Ranges{get;}
    public Condition Condition{get;}

    public ConditionRange TruePath{get;}
    public ConditionRange FalsePath{get;}
    public ConditionRange(Condition condition, Dictionary<char, (int, int)> dict)
    {
        this.Condition = condition;
        this.Ranges = dict;
    }

    public (Dictionary<char, (int min, int max)> t, Dictionary<char, (int min, int max)> f) SplitRange()
    {
        var currRange = this.Ranges[this.Condition.Variable];

        if(this.Condition.LessThan)
        {
            var min1 = currRange.min;
            var max1 = this.Condition.Value - 1;
            var min2 = this.Condition.Value;
            var max2 = currRange.max;

            var range1 = copyRangeAndReplace(this.Condition.Variable, (min1, max1));
            var range2 = copyRangeAndReplace(this.Condition.Variable, (min2, max2));

            return (range1, range2);            
        }
        else
        {
            var min1 = this.Condition.Value + 1;
            var max1 = currRange.max;
            var min2 = currRange.min;
            var max2 = this.Condition.Value;

            var range1 = copyRangeAndReplace(this.Condition.Variable, (min1, max1));
            var range2 = copyRangeAndReplace(this.Condition.Variable, (min2, max2));

            return (range1, range2); 
        }
        
    }
    Dictionary<char, (int min, int max)> copyRangeAndReplace(char c, (int, int) r)
    {
        var dict = new Dictionary<char, (int, int)>();
        foreach(var key in this.Ranges.Keys)
            dict.Add(key, this.Ranges[key]);
        dict[c] = r;
        return dict;
    }
}