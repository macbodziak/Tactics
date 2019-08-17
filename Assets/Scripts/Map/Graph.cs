using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Graph
{
    int width;
    int height;
    Node[,] nodes;

    static Vector2Int[] Dir = { Vector2Int.down, new Vector2Int(1, -1), Vector2Int.right, new Vector2Int(1, 1), Vector2Int.up, new Vector2Int(-1, 1), Vector2Int.left, new Vector2Int(-1, -1) };
    static int[] DirCost = { 10, 14, 10, 14, 10, 14, 10, 14 };
    public Graph(int width, int height)
    {
        this.width = width;
        this.height = height;
        nodes = new Node[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                NodeObject tempGo = GameObject.Instantiate(Resources.Load("Prefabs/Tile", typeof(NodeObject))) as NodeObject;
                Vector3 pos = new Vector3(x, 0f, y);
                tempGo.transform.position = pos;
                tempGo.name = "Tile " + x + "," + y;
                nodes[x, y] = new Node(tempGo);
                tempGo.Init(nodes[x,y]);
            }
        }
    }

    static public Graph CreateNewGraph(int width, int height)
    {
        Graph g = new Graph(width, height);
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                g.SetUpNeighboursPerNode(new Vector2Int(x, y));
            }
        }
        return g;
    }

    void SetUpNeighboursPerNode(Vector2Int location)
    {
        Node currentNode = nodes[location.x, location.y];
        int x;
        int y;
        // string debugString = "[" + location.x +", " + location.y + "] : ";
        for (int i = 0; i < 8; i++)
        {
            x = Dir[i].x + location.x;
            y = Dir[i].y + location.y;

            if (IsInGraphRange(x, y))
            {
                // Debug.Log(x + " ," + y);
                currentNode.edges.Add(new Edge(nodes[x, y], DirCost[i]));
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

    static public Graph LoadGraphFromFile(string Filename)
    {
        TextAsset textFile = Resources.Load(Filename) as TextAsset;
        if (textFile != null)
        {
            Debug.Log(Filename + " exists!");
            string text = textFile.text;
            char[] seperator = { ' ', '\n' };
            string[] words = text.Split(seperator);
            if (words.Length < 2)
            {
                Debug.Log(Filename + " has incorrect data, too short");
                return null;
            }
            int w = int.Parse(words[0]);
            int h = int.Parse(words[1]);
            Graph graph = new Graph(w, h);

            return null;
        }
        else
        {
            Debug.Log(Filename + " does not exist :(");
            return null;
        }

    }
}
