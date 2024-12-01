namespace rsmith985.AOC.Y2022;

public class Day07 : Day
{

    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var dict = new Dictionary<string, Directory>();

        string currName = null;
        Directory curr = null;
        foreach(var line in this.GetLines())
        {
            if(line.StartsWith("$ cd"))
            {
                var nextName = line.Substring(5).Trim();

                if(nextName == "..")
                {
                    if(curr.Parent == null) continue;
                    currName = curr.Parent;
                    curr = dict[currName];
                }
                else
                {
                    if(!dict.ContainsKey(nextName))
                        dict.Add(nextName, new Directory(nextName, currName));

                    curr?.Dirs?.Add(nextName);

                    curr = dict[nextName];
                    currName = nextName;
                }
            }
            else if(line.StartsWith("$ ls")){}
            else
            {
                if(line.StartsWith("dir"))
                {
                    var subDirName = line.Substring(4).Trim();
                    curr.Dirs.Add(subDirName);
                }
                else
                {
                    (var size, var name) = line.Split2();
                    if(!curr.Files.ContainsKey(name))
                        curr.Files.Add(name, long.Parse(size));
                }
            }
        }

        var toCalc = dict.Keys.ToList();
        foreach(var dir in dict.Values)
        {
            if(dir.Dirs.Count == 0)
                dir.TotSize = dir.Files.Sum(i => i.Value);
        }
        while(toCalc.Count > 0)
        {
            var nextSet = new List<string>();
            foreach(var key in toCalc)
            {
                var dir = dict[key];
                if(!dir.Dirs.All(d => dict[d].TotSize >= 0))
                {
                    nextSet.Add(key);
                    continue;
                }
                else
                {
                    dir.TotSize = dir.Files.Sum(i => i.Value) + dir.Dirs.Select(i => dict[i]).Sum(i => i.TotSize);
                }
            }
            toCalc = nextSet;
        }

        return dict.Values.Where(i => i.TotSize < 100000).Sum(i => i.TotSize);
    }

    public override object Part2()
    {
        throw new NotImplementedException();
    }

    class Directory
    {
        public string Name { get; set; }
        public string Parent{get;set;}
        public HashSet<string> Dirs{get;set;}
        public Dictionary<string, long> Files{get;set;} 
        public long TotSize{get; set;}  = -1;

        public Directory(string name, string parent)
        {
            this.Name = name;
            this.Parent = parent;
            this.Files = new();
            this.Dirs = new ();
        }
    }
}
