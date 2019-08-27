using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Graph : MonoBehaviour, ISerializationCallbackReceiver
{
    private static Graph myInstance;
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public bool isEmpty = true;
    [SerializeField] public float nodeInterval = 1.0f;
    Node[,] nodes;

#if UNITY_EDITOR
    public bool showDebugEdges = false;
#endif
    [SerializeField] List<Node> serializedNodes;

    static Vector2Int[] Dir = { Vector2Int.down, new Vector2Int(1, -1), Vector2Int.right, new Vector2Int(1, 1), Vector2Int.up, new Vector2Int(-1, 1), Vector2Int.left, new Vector2Int(-1, -1) };
    static int[] DirCost = { 10, 14, 10, 14, 10, 14, 10, 14 };

    static public Graph instance
    {
        get
        {
            return myInstance;
        }
    }

    private void Awake()
    {
        if (myInstance != null && myInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        myInstance = this;
    }

    public void InitGraph(int width, int height)
    {
        this.width = width;
        this.height = height;
        nodes = new Node[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node no = GameObject.Instantiate(Resources.Load("Prefabs/Tile", typeof(Node)), transform) as Node;
                Vector3 pos = new Vector3(x, 0f, y);
                no.Init(pos, x, y, "Tile " + x + "," + y, true);
                nodes[x, y] = no;
            }
        }
    }

    public void CreateNewGraph(int inWidth, int inHeight)
    {
        isEmpty = false;
        if (serializedNodes == null)
        {
            serializedNodes = new List<Node>();
        }
        InitGraph(inWidth, inHeight);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SetUpNeighboursPerNode(new Vector2Int(x, y));
            }
        }
    }

    public void CreateNewGraphFromTexture(Texture2D tex)
    {
        
        if(tex == null)
        {
            Debug.Log("Error: texture = null");
            return;
        }

        isEmpty = false;
        if (serializedNodes == null)
        {
            serializedNodes = new List<Node>();
        }
        InitGraphFromTexture(tex);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SetUpNeighboursPerNode(new Vector2Int(x, y));
            }
        }
    }

    public void InitGraphFromTexture(Texture2D tex)
    {

        this.width = tex.width;
        this.height = tex.height;
        nodes = new Node[width, height];
        bool isWalkable = true;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                isWalkable = tex.GetPixel(x,y) == Color.white;
                Node no = GameObject.Instantiate(Resources.Load("Prefabs/Tile", typeof(Node)), transform) as Node;
                Vector3 pos = new Vector3(x, 0f, y);
                no.Init(pos, x, y, "Tile " + x + "," + y, isWalkable);
                Material mat = no.GetComponent<Renderer>().material;
                mat.color = Color.red;
                no.GetComponent<Renderer>().material = mat;
                
                nodes[x, y] = no;
            }
        }
    }

    public void SetUpNeighboursPerNode(Vector2Int location)
    {
        Node currentNode = nodes[location.x, location.y];
        currentNode.edges.Clear();

        for (int i = 0; i < 8; i++)
        {
            SetUpEdgePerDirection(location, i);

        }
    }

    public void SetUpNeighboursPerNode(Node currentNode)
    {
        currentNode.edges.Clear();
        Vector2Int location = new Vector2Int(currentNode.x, currentNode.y);
        // if (currentNode.walkable == false)
        // {   
        //     return;
        // }

        for (int i = 0; i < 8; i++)
        {
            SetUpEdgePerDirection(location, i);
        }
    }

    bool SetUpEdgePerDirection(Vector2Int location, int DirIndex)
    {
        Node currentNode = nodes[location.x, location.y];

        int x = Dir[DirIndex].x + location.x;
        int y = Dir[DirIndex].y + location.y;
        int prevX = 0;
        int prevY = 0;
        int nextX = 0;
        int nextY = 0;

        bool diagonal = true;

        switch (DirIndex)
        {
            case 1:
                prevX = Dir[0].x + location.x;
                prevY = Dir[0].y + location.y;
                nextX = Dir[2].x + location.x;
                nextY = Dir[2].y + location.y;
                break;
            case 3:
                prevX = Dir[2].x + location.x;
                prevY = Dir[2].y + location.y;
                nextX = Dir[4].x + location.x;
                nextY = Dir[4].y + location.y;
                break;
            case 5:
                prevX = Dir[4].x + location.x;
                prevY = Dir[4].y + location.y;
                nextX = Dir[6].x + location.x;
                nextY = Dir[6].y + location.y;
                break;
            case 7:
                prevX = Dir[6].x + location.x;
                prevY = Dir[6].y + location.y;
                nextX = Dir[0].x + location.x;
                nextY = Dir[0].y + location.y;
                break;
            default:
                diagonal = false;
                break;
        }

        if (IsInGraphRange(x, y))
        {
            if (diagonal)
            {
                if (nodes[x, y].walkable && nodes[prevX, prevY].walkable && nodes[nextX, nextY].walkable)
                {
                    currentNode.AddEdge(nodes[x, y], DirCost[DirIndex]);
                    return true;
                }
            }
            else if (nodes[x, y].walkable)
            {
                currentNode.AddEdge(nodes[x, y], DirCost[DirIndex]);
                return true;
            }
        }
        return false;
    }

    public void RebakeNode(Node currentNode)
    {
        int nextX;
        int nextY;

        for (int i = 0; i < 8; i++)
        {
            nextX = currentNode.x + Dir[i].x;
            nextY = currentNode.y + Dir[i].y;
            if (IsInGraphRange(nextX, nextY))
            {
                SetUpNeighboursPerNode(nodes[nextX, nextY]);
            }
        }
        SetUpNeighboursPerNode(currentNode);
    }

    bool IsInGraphRange(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return false;
        }
        return true;
    }

    public void OnAfterDeserialize()
    {
        nodes = new Node[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodes[x, y] = serializedNodes[x * height + y];
            }
        }
    }
    public void OnBeforeSerialize()
    {
        if (serializedNodes != null)
        {
            serializedNodes.Clear();
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                serializedNodes.Add(nodes[x, y]);
            }
        }
    }

#if UNITY_EDITOR
    public void DeleteGraph()
    {
        isEmpty = true;
        for (int i = 0; i < serializedNodes.Count; i++)
        {
            UnityEditor.Undo.DestroyObjectImmediate(serializedNodes[i].gameObject);
        }
        UnityEditor.Undo.undoRedoPerformed += Rebake;
        serializedNodes.Clear();
    }
#endif

    public void Rebake()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodes[x, y].ClearEdges();
                SetUpNeighboursPerNode(new Vector2Int(x, y));
            }
        }
    }

    static public void DrawDebugArea(Dictionary<Node, Pathfinder.NodePathData> nodeSet, float duration = 5.0f)
    {
        foreach (var item in nodeSet)
        {
            if (item.Value != null)
            {
                Debug.DrawLine(item.Key.transform.position, item.Value.cameFrom.transform.position, Color.magenta, duration);
            }
        }
    }

    static public void HighlightArea(Dictionary<Node, Pathfinder.NodePathData> nodeSet)
    {
        foreach (var item in nodeSet)
        {
            item.Key.Highlight();
            if (item.Value != null)
            {
                // Debug.DrawLine(item.Key.transform.position, item.Value.cameFrom.transform.position, Color.magenta, 2f);
            }
        }
    }

    static public void UnHighlightArea(Dictionary<Node, Pathfinder.NodePathData> nodeSet)
    {
        foreach (var item in nodeSet)
        {
            item.Key.UnHighlight();
        }
    }

    static public void DrawDebugPath(List<Node> path, float duration = 5.0f)
    {
        if (path.Count <= 1)
        {
            Debug.Log("Empty Path");
        }
        for (int i = 1; i < path.Count; i++)
        {
            Debug.DrawLine(path[i - 1].transform.position, path[i].transform.position, Color.red, duration);
        }
    }

    static public void DrawDebugGraph(Graph gr, float duration = 5.0f)
    {
        foreach (Node node in gr.nodes)
        {
            node.DrawDebugEdges();
        }
    }

#if UNITY_EDITOR
    static public void DrawDebugGraphInEditor(Graph gr, float duration = 5.0f)
    {
        foreach (Node node in gr.nodes)
        {
            node.DrawDebugEdgesInEditor();
        }
    }
#endif
}
