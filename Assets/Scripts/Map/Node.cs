using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    bool myWalkable = true;
    //other things
    public List<Edge> edges = new List<Edge>();

    public List<Node> _nodes = new List<Node>();
    public List<int> _costs = new List<int>();

    // public Dictionary<Node, int> edges2 = new  Dictionary<Node, int>();

    public void OnBeforeSerialize()
    {
        _nodes.Clear();
        _costs.Clear();

        foreach (Edge e in edges)
        {
            _nodes.Add(e.node);
            _costs.Add(e.cost);
        }
    }

    public void OnAfterDeserialize()
    {
        edges.Clear();
        for (int i = 0; i < _nodes.Count; i++)
        {
            edges.Add(new Edge(_nodes[i], _costs[i]));
        }
    }

    public bool walkable
    {
        get
        {
            return myWalkable;
        }

        set
        {
            myWalkable = value;
        }
    }

    public void ToogleWalkable()
    {
        walkable = !walkable;
    }

    public void AddEdge(Node inNode, int inCost)
    {
        edges.Add(new Edge(inNode, inCost));
    }
    public void DrawDebugEdges()
    {
        // Debug.Log("edge count : " + edges.Count);
        foreach (Edge edge in edges)
        {
            if (edge.node == null)
            {
                Debug.Log("edge.node == null");
            }

            Debug.DrawLine(transform.position, edge.node.transform.position, Color.green, 2.0f);
            // Debug.Log("drawing line : " + transform.position + " - " + edge.node.transform.position);
        }
    }
}
