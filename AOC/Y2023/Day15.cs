namespace rsmith985.AOC.Y2023;

public class Day15 : Day
{
    public override object Part1_() =>
        File.ReadAllLines(_file)[0].Split(',').Sum(i => i.ToArray().Aggregate(0, (a, b) => (a+b)*17%256));
    
    public override object Part1()
    {
        var input = File.ReadAllLines(_file)[0];
        
        var tot = 0;
        var curr = 0;
        for(int i = 0; i < input.Length; i++)
        {
            if(input[i] == ',')
            {
                tot += curr;
                curr = 0;   
            }
            else
            {
                curr = ((curr + input[i]) * 17) % 256;
            }
        }
        tot += curr;

        return tot;
    }


    public override object Part2()
    {
        var input = this.GetLines()[0];
        
        var parts = input.Split(',');

        var data = new LinkedList<(string, int)>[256];
        for(int i = 0; i < 256; i++)
            data[i] = new LinkedList<(string, int)>(); 

        foreach(var part in parts)
        {
            if(part.EndsWith('-'))
            {
                var key = part[..^1];
                var bin = hashCode(key);
                remove(data[bin], key);
            }
            else
            {
                var kv = part.Split('=');
                var num = int.Parse(kv[1]);
                var bin = hashCode(kv[0]);
                insert(data[bin], kv[0], num);
            }
        }
        
        long tot = 0;
        for(int i = 0; i < 256; i++)
        {
            var list = data[i];
            if(list.Any())
            {
                long num = 0;
                var slot = 1;
                var curr = list.First;
                while(curr != null)
                {
                    num += ((i+1)*(long)slot*(long)curr.Value.Item2);
                    curr = curr.Next;
                    slot++;
                }
                tot += num;
            }
        }
        return tot;
    }

    private void insert(LinkedList<(string, int)> list, string key, int val)
    {
        var curr = list.First;
        while(curr != null)
        {
            var data = curr.Value;
            if(data.Item1 == key)
            {
                list.AddAfter(curr, (key, val));
                list.Remove(curr);
                return;
            }
            curr = curr.Next;
        }
        list.AddLast((key, val));
    }

    private void remove(LinkedList<(string, int)> list, string key)
    {
        var curr = list.First;
        while(curr != null)
        {
            var data = curr.Value;
            if(data.Item1 == key)
            {
                list.Remove(curr);
                break;
            }
            curr = curr.Next;
        }
    }

    private int hashCode(string str)
    {
        int curr = 0;
        for(int i = 0; i < str.Length; i++)
            curr = ((curr + str[i]) * 17) % 256;
        return curr;
    }
}
