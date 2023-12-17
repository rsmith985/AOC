namespace rsmith985.AOC.Y2023;

class PriorityQueue<D, V> : IEnumerable<D>
    where V : IComparable<V>
{
    public bool IsEmpty => _heap.IsEmpty();

    private Dictionary<D, FibonacciHeapNode<D, V>> _dict;

    private FibonacciHeap<D, V> _heap;

    public int Count => _dict.Count;

    public PriorityQueue(V minValue = default)
    {
        _dict = new Dictionary<D, FibonacciHeapNode<D, V>>();
        _heap = new FibonacciHeap<D, V>(minValue);
    }

    public void Add(D key, V priority)
    {
        this.Insert(key, priority);
    }

    public void Insert(D key, V priority)
    {
        var node = new FibonacciHeapNode<D, V>(key, priority);
        _heap.Insert(node);
        _dict.Add(key, node);
    }

    public D PopMin()
    {
        var node = _heap.RemoveMin();
        _dict.Remove(node.Data);
        return node.Data;
    }

    public D PeekMin() => _heap.Min().Data;

    public bool Contains(D key) => _dict.ContainsKey(key);

    public void ChangePriority(D key, V priority)
    {
        var node = _dict[key];
        _heap.ChangeValue(node, priority);
    }

    public IEnumerator<D> GetEnumerator()
    {
        return _dict.Keys.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _dict.Keys.GetEnumerator();
    }
}
