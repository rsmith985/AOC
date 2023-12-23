using System.ComponentModel;
using Emgu.CV.Structure;

namespace rsmith985.AOC.Y2023;

public class Day20 : Day
{
   // protected override bool _useDefaultTestFile => true;

    private Broadcast _start;
    private Dictionary<string, Module> _moduleDict;

    public override void Init()
    {
        _moduleDict = new Dictionary<string, Module>();
        var allFeeds = new HashSet<string>();
        foreach(var line in this.GetLines())
        {
            var idx = line.IndexOf("->");
            var key = line[1..idx].Trim();
            var feeds = line[(idx+3)..].Split(',').Select(i => i.Trim()).ToArray();

            if(line.StartsWith("broadcaster"))
            {
                _start = new Broadcast("broadcast", feeds);
                _moduleDict.Add("broadcast", _start);
            }
            else if(line.StartsWith("&"))
            {
                var conj = new Conjunction(key, feeds);
                _moduleDict.Add(key, conj);
            }
            else if(line.StartsWith("%"))
            {
                var ff = new FlipFlop(key, feeds);
                _moduleDict.Add(key, ff);
            }
        }

        foreach(var mod in _moduleDict.Values.ToList())
        {
            foreach(var feed in mod.FeedKeys)
            {
                if(!_moduleDict.ContainsKey(feed))
                    _moduleDict.Add(feed, new EndPoint(feed));
            }
            mod.AddConnections(_moduleDict);
        }
        foreach(var mod in _moduleDict.Values)
            mod.Init();

    }

    public override object Part1()
    {
        long highPulses = 0;
        long lowPulses = 0;
        for(int i = 0; i < 1000; i++)
        {
            //Console.WriteLine(".............New Button Press................");
            var queue = new Queue<Signal>();
            queue.Enqueue(new Signal(false, _start, null));

            while(queue.Any())
            {
                var signal = queue.Dequeue();
                if(signal.Pulse)
                    highPulses++;
                else
                    lowPulses++;

                var module = signal.To;
                var toSend = module.PulseReceived(signal.Pulse, signal.From);

                //Console.WriteLine($"{signal.From} {signal.Pulse} > {module.Key}");
                if(toSend == null) continue;

                foreach(var to in module.Outputs)
                    queue.Enqueue(new Signal(toSend.Value, to, module.Key));
            }
        }
        return highPulses * lowPulses;
    }

    public override object Part2()
    {
        //var drAll = new List<string>(){"sb", "lp", "sh", "kn", "jc", "zf", "lh", "kd", "jg", "bj", "fp", "bk" };
        //var tnAll = new List<string>(){"dc", "nx", "qr", "pz", "rv", "mp", "cj", "pg", "df", "rs", "gq", "vx" };
        //var bmAll = new List<string>(){"jk", "qs", "db", "qg", "ls", "ng", "ft", "dz", "fg", "xz", "sx", "lm" };
        //var clAll = new List<string>(){"mg", "fj", "tr", "bx", "qb", "qm", "ll", "zb", "gz", "dx", "bv", "bs" };

        // Need to reset state
        this.Init();

        // Output is fed by a NAND and each NAND get signal for 4 counters.
        var rxMod = _moduleDict["rx"];
        var rxIn = rxMod.Inputs.First();
        var counterKeys = rxIn.Inputs.ToDictionary(i => i.Key, i => (long)0);

        for(int i = 0; true; i++)
        {
            var queue = new Queue<Signal>();
            queue.Enqueue(new Signal(false, _start, null));

            while(queue.Any())
            {
                var signal = queue.Dequeue();

                if(signal.Pulse && counterKeys.ContainsKey(signal.From) && counterKeys[signal.From] == 0)
                    counterKeys[signal.From] = i+1;
                
                var module = signal.To;
                var toSend = module.PulseReceived(signal.Pulse, signal.From);

                if(toSend == null) continue;

                foreach(var to in module.Outputs)
                    queue.Enqueue(new Signal(toSend.Value, to, module.Key));
            }
                
            if(counterKeys.Values.All(i => i > 0))
                break;
        }

        return MyMath.LCM(counterKeys.Values.ToArray());
    }

    private string printNumber(List<string> keys)
    {
        var binary = "";
        foreach(var key in keys)
        {
            var module = _moduleDict[key] as FlipFlop;
            binary += module.State ? "1" : "0";
        }

        return binary + " > " + Convert.ToInt32(binary, 2);
    }
}

struct Signal(bool pulse, Module to, string from)
{
    public bool Pulse => pulse;
    public Module To => to;
    public string From => from;
}

abstract class Module
{
    public string Key{get;}

    public string[] FeedKeys;

    public HashSet<Module> Outputs{get;private set;}
    public HashSet<Module> Inputs{get;private set;}

    public Module(string key, string[] feeds)
    {
        this.Key = key;
        this.FeedKeys = feeds;
        this.Inputs = new HashSet<Module>();
        this.Outputs = new HashSet<Module>();
    }

    public void AddConnections(Dictionary<string, Module> dict)
    {
        foreach(var key in FeedKeys)
        {
            var to = dict[key];
            this.Outputs.Add(to);
            to.Inputs.Add(this);
        }
    }

    public virtual void Init(){}

    public abstract bool? PulseReceived(bool pulse, string from);
}

class FlipFlop : Module
{
    public bool State{get; private set;}

    public FlipFlop(string key, string[] feeds) : base(key, feeds){}

    public override bool? PulseReceived(bool pulse, string from)
    {
        if(pulse) return null;

        this.State = !this.State;
        return this.State;
    }
}

class Conjunction : Module
{
    public Dictionary<string, bool> State{get; private set;}

    public Conjunction(string key, string[] feeds) 
        : base(key, feeds){}

    public override void Init()
    {
        this.State = new Dictionary<string, bool>();
        foreach(var input in this.Inputs)
            this.State.Add(input.Key, false);
    }

    public override bool? PulseReceived(bool pulse, string from)
    {
        this.State[from] = pulse;

        return !this.State.Values.All(i => i == true);
    }
}

class Broadcast : Module
{
    public Broadcast(string key, string[] feeds)
        : base(key, feeds){}

    public override bool? PulseReceived(bool pulse, string from)
    {
        return pulse;
    }
}

class EndPoint : Module
{
    public EndPoint(string key) : base(key, new string[0]){}

    public override bool? PulseReceived(bool pulse, string from)
    {
        return null;
    }
}