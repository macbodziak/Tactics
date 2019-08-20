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
        if (currentNode.walkable == false)
        {
            return;
        }

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
        for (int i = 0; i < serializedNodes.Count; i++)
        {
            DestroyImmediate(serializedNodes[i].gameObject);
        }
        serializedNodes.Clear();
    }

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
