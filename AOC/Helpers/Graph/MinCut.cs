namespace rsmith985.AOC.Y2023;

public class StoerWagnerMinCut<T>
{
    private Dictionary<int, Node<T>> _lookup;
    private Dictionary<Node<T>, int> _reverse;

    public int VertexCount {get; set;}
    private List<List<(int, double)>> AdjacencyList{get;}
    public StoerWagnerMinCut(Graph<T> graph)
    {
        this.VertexCount = graph.Nodes.Count;

        this.AdjacencyList = new List<List<(int, double)>>();
        for(int i = 0; i < this.VertexCount; i++)
            this.AdjacencyList.Add(new List<(int, double)>());

        _lookup = new Dictionary<int, Node<T>>();
        _reverse = new Dictionary<Node<T>, int>();
        int num = 0;
        foreach(var node in graph.Nodes.Values)
        {
            _lookup.Add(num, node);
            _reverse.Add(node, num);
            num++;
        }

        foreach(var node in graph.Nodes.Values)
        {
            var n1 = _reverse[node];
            foreach(var e in node.Edges)
            {
                var n2 = _reverse[e.GetOpposite(node)];
                this.AdjacencyList[n1].Add((n2, e.Weight));
                this.AdjacencyList[n2].Add((n1, e.Weight));
            }
        }
    }

    public double Run()
    {
        double minCut = double.MaxValue;

        while (this.VertexCount > 1)
        {
            int[] a = new int[this.VertexCount];
            double[] w = new double[this.VertexCount];

            // Initialize a and w for the first vertex
            for (int i = 0; i < this.VertexCount; i++)
            {
                a[i] = i;
                w[i] = 0;
            }

            // Perform the main loop of the algorithm
            for (int i = 1; i < this.VertexCount; i++)
            {
                int bestVertex = -1;
                double maxWeight = 0;

                for (int j = 0; j < VertexCount; j++)
                {
                    if (a[j] != -1)
                    {
                        for (int k = 0; k < AdjacencyList[j].Count; k++)
                        {
                            int neighbor = AdjacencyList[j][k].Item1;
                            if (a[neighbor] != -1 && w[j] + AdjacencyList[j][k].Item2 > maxWeight)
                            {
                                maxWeight = w[j] + AdjacencyList[j][k].Item2;
                                bestVertex = j;
                            }
                        }
                    }
                }

                // Merge bestVertex into a[0]
                for (int j = 0; j < AdjacencyList[bestVertex].Count; j++)
                {
                    int neighbor = AdjacencyList[bestVertex][j].Item1;
                    if (a[neighbor] != -1 && neighbor != a[0])
                    {
                        w[neighbor] += AdjacencyList[bestVertex][j].Item2;
                    }
                }

                a[bestVertex] = -1;
                if (i == VertexCount - 1)
                {
                    minCut = Math.Min(minCut, w[a[0]]);
                }
            }

            // Contract the vertex a[0]
            ContractVertex(a[0]);
        }

        return minCut;
    }

    private void ContractVertex(int vertex)
    {
        // Update adjacency lists, removing vertex and merging edges
        for (int i = 0; i < AdjacencyList[vertex].Count; i++)
        {
            int neighbor = AdjacencyList[vertex][i].Item1;
            if (neighbor != vertex)
            {
                // Add edges between neighbors of vertex
                foreach (var edge in AdjacencyList[neighbor])
                {
                    if (edge.Item1 == vertex)
                    {
                        AdjacencyList[neighbor].Remove(edge);
                    }
                }
                AdjacencyList[neighbor].AddRange(AdjacencyList[vertex]);
            }
        }

        AdjacencyList.RemoveAt(vertex);
        VertexCount--;
    }
}
