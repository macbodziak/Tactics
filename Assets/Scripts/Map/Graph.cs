using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Graph : MonoBehaviour, ISerializationCallbackReceiver
{
    public int width;
    public int height;
    
    public bool isEmpty = false;
    Node[,] nodes;

    [SerializeField] List<Node> serializedNodes;
    static Vector2Int[] Dir = { Vector2Int.down, new Vector2Int(1, -1), Vector2Int.right, new Vector2Int(1, 1), Vector2Int.up, new Vector2Int(-1, 1), Vector2Int.left, new Vector2Int(-1, -1) };
    static int[] DirCost = { 10, 14, 10, 14, 10, 14, 10, 14 };

    // void Start()
    // {
    //     Debug.Log("Graph Start");
    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {
    //             SetUpNeighboursPerNode(new Vector2Int(x, y));
    //         }
    //     }
    // }

    public void InitGraph(int width, int height)
    {
        this.width = width;
        this.height = height;
        nodes = new Node[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node no = GameObject.Instantiate(Resources.Load("Prefabs/Tile", typeof(Node))) as Node;
                Vector3 pos = new Vector3(x, 0f, y);
                no.transform.position = pos;
                no.name = "Tile " + x + "," + y;
                nodes[x, y] = no;
            }
        }
    }

    public void CreateNewGraph(int inWidth, int inHeight)
    {
        isEmpty = false;
        InitGraph(inWidth, inHeight);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SetUpNeighboursPerNode(new Vector2Int(x, y));
            }
        }
    }

    void SetUpNeighboursPerNode(Vector2Int location)
    {
        Node currentNode = nodes[location.x, location.y];
        int x;
        int y;
        // string debugString = "[" + location.x + ", " + location.y + "] : ";
        for (int i = 0; i < 8; i++)
        {
            x = Dir[i].x + location.x;
            y = Dir[i].y + location.y;

            if (IsInGraphRange(x, y))
            {
                // Debug.Log(x + " ," + y);
                // currentNode.edges.Add(new Edge(nodes[x, y], DirCost[i]));
                currentNode.AddEdge(nodes[x, y], DirCost[i]);
                // debugString += "(" + x + "," + y + ") - " + DirCost[i] + " ";
            }
        }
        // Debug.Log(debugString);
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
        serializedNodes.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                serializedNodes.Add(nodes[x, y]);
            }
        }
    }

    public void DeleteGraph()
    {
        isEmpty = true;
        for(int i = 0; i < serializedNodes.Count; i++)
        {
            DestroyImmediate(serializedNodes[i].gameObject);
        }
        serializedNodes.Clear();
    }
    // static public Graph LoadGraphFromFile(string Filename)
    // {
    //     TextAsset textFile = Resources.Load(Filename) as TextAsset;
    //     if (textFile != null)
    //     {
    //         Debug.Log(Filename + " exists!");
    //         string text = textFile.text;
    //         char[] seperator = { ' ', '\n' };
    //         string[] words = text.Split(seperator);
    //         if (words.Length < 2)
    //         {
    //             Debug.Log(Filename + " has incorrect data, too short");
    //             return null;
    //         }
    //         int w = int.Parse(words[0]);
    //         int h = int.Parse(words[1]);
    //         Graph graph = new Graph(w, h);

    //         return null;
    //     }
    //     else
    //     {
    //         Debug.Log(Filename + " does not exist :(");
    //         return null;
    //     }

    // }
}
