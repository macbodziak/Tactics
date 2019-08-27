using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] bool myWalkable = true;
    //other things
    public List<Edge> edges = new List<Edge>();

    [SerializeField] Graph paretGraph = null;
    [SerializeField] int _x;
    [SerializeField] int _y;

    [SerializeField] Character myCharacter = null;
    #if UNITY_EDITOR
    //used for serialization for the editor
    #endif
    [SerializeField]  List<Node> _nodes = new List<Node>();
    //used for serialization for the editor
    [SerializeField]  List<int> _costs = new List<int>();

    static Texture2D texture;

    public Character character
    {
        get
        {
            return myCharacter;
        }

        set
        {
            myCharacter = value;
            if (myCharacter == null)
            {
                walkable = true;
            }
            else
            {
                myCharacter.node = this;
                walkable = false;
            }
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
            if (myWalkable == true)
            {
                tag = "WalkableNode";
            }
            else
            {
                tag = "NotWalkableNode";
            }
            Graph.instance.RebakeNode(this);
        }
    }

    public int x
    {
        get
        {
            return _x;
        }
    }

    public int y
    {
        get
        {
            return _y;
        }
    }
    private void Awake()
    {
        //init highlight texture
        texture = Resources.Load<Texture2D>("Textures/walkableTile") as Texture2D;
        GetComponent<Renderer>().material.SetTexture("_DetailAlbedoMap", texture);

        if (myWalkable == true)
        {
            tag = "WalkableNode";
        }
        else
        {
            tag = "NotWalkableNode";
        }

    }

    public void Init(Vector3 pos, int x, int y, string name, bool isWalkable)
    {
        transform.position = pos;
        gameObject.name = name;
        _x = x;
        _y = y;
        myWalkable = isWalkable;
    }

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

    public void ToogleWalkable()
    {
        walkable = !walkable;
    }

    public void AddEdge(Node inNode, int inCost)
    {
        edges.Add(new Edge(inNode, inCost));
    }

    public void ClearEdges()
    {
        edges.Clear();
    }
    public void DrawDebugEdges()
    {
        foreach (Edge edge in edges)
        {
            if (edge.node == null)
            {
                Debug.Log("edge.node == null");
            }
            Vector3 vec = edge.node.transform.position - transform.position;
            vec *= 0.5f;
            vec += transform.position;
            Debug.DrawLine(transform.position, vec, Color.green, 2.0f);
        }
    }

    #if UNITY_EDITOR    
    public void DrawDebugEdgesInEditor()
    {
        foreach (Edge edge in edges)
        {
            if (edge.node == null)
            {
                Debug.Log("edge.node == null");
            }
            Vector3 vec = edge.node.transform.position - transform.position;
            vec *= 0.5f;
            vec += transform.position;
            UnityEditor.Handles.DrawLine(transform.position, vec);
        }
    }
    #endif

    public void Highlight()
    {
        GetComponent<Renderer>().material.EnableKeyword("_DETAIL_MULX2");
    }

    public void UnHighlight()
    {
        GetComponent<Renderer>().material.DisableKeyword("_DETAIL_MULX2");
    }
}
