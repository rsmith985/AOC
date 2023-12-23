namespace rsmith985.AOC.Y2022;

public class Day05 : Day
{
    private List<(int num, int from, int to)> _ops;
    private Stack<Char>[] _stacks;

    public override void Init()
    {
        _ops = new List<(int num, int from, int to)>();
        _stacks = new Stack<char>[9];
        Enumerable.Range(0, _stacks.Length).Perform(i => _stacks[i] = new Stack<char>());

        var readingStacks = true;
        var stackStrs = new List<string>();
        foreach(var line in this.GetLines())
        {
            if(line.StartsWith(" 1"))
            {
                readingStacks = false;
                continue;
            }

            if(readingStacks)
            {
                stackStrs.Add(line.PadRight(50, ' '));
            }
            else
            {
                if(!line.StartsWith("move")) continue;

                var fromIndex = line.IndexOf("from");
                var toIndex = line.IndexOf("to");
                var num1 = int.Parse(line[5..fromIndex].Trim());
                var num2 = int.Parse(line[(fromIndex+5)..toIndex].Trim());
                var num3 = int.Parse(line[(toIndex+3)..].Trim());
                _ops.Add((num1, num2-1, num3-1));
            }
        }

        stackStrs.Reverse();
        foreach(var str in stackStrs)
        {
            for(int i = 0; i < 9; i++)
            {
                var idx = i*4 + 1;
                if(str[idx] == ' ') continue;
                _stacks[i].Push(str[idx]);
            }
        }
    }

    public override object Part1()
    {
        foreach(var op in _ops)
        {
            for(int i = 0; i < op.num; i++)
            {
                var item = _stacks[op.from].Pop();
                _stacks[op.to].Push(item);
            }
        }

        var str = "";
        for(int i = 0; i < _stacks.Length; i++)
        {
            str += _stacks[i].Peek();
        }
        
        return str;
    }

    public override object Part2()
    {
        this.Init();

        foreach(var op in _ops)
        {
            var temp = new List<char>();
            for(int i = 0; i < op.num; i++)
            {
                var item = _stacks[op.from].Pop();
                temp.Add(item);
            }
            temp.Reverse();
            foreach(var item in temp)
                _stacks[op.to].Push(item);
        }

        var str = "";
        for(int i = 0; i < _stacks.Length; i++)
        {
            str += _stacks[i].Peek();
        }
        
        return str;
    }
}
