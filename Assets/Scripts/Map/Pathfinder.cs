﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    class NodePathData
    {
        public Node cameFrom = null;
        public int costSoFar = 0;
    }
    public static List<Node> FindPathDijkstra(Graph g, Node start, Node destination)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        List<Node> path = new List<Node>();

        Node currentNode;
        frontier.Push(start, 0);
        cameFrom.Add(start, null);
        costSoFar.Add(start, 0);

        while (frontier.Count > 0)
        {
            currentNode = frontier.Pop();

            if (currentNode == destination)
            {
                //reconstruct path:
                Node n = destination;
                while (n != null)
                {
                    path.Add(n);
                    n = cameFrom[n];
                }
                break;
            }

            foreach (Edge edge in currentNode.edges)
            {
                Node nextNode = edge.node;
                int newCost = costSoFar[currentNode] + edge.cost;
                if (!costSoFar.ContainsKey(nextNode) || costSoFar[nextNode] > newCost)
                {
                    costSoFar[nextNode] = newCost;
                    frontier.Push(nextNode, newCost);
                    cameFrom[nextNode] = currentNode;
                }
            }
        }
        return path;
    }

    static public Dictionary<Node, Node> FindWalkableArea(Graph g, Node start, int range)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        // Dictionary<Node, NodePathData> nodeData = new Dictionary<Node, NodePathData>();

        Node currentNode;
        // NodePathData currentNodePathData;
        frontier.Push(start, 0);
        cameFrom.Add(start, null);
        costSoFar.Add(start, 0);
        // currentNodePathData = new NodePathData();
        // currentNodePathData.cameFrom = null;
        // currentNodePathData.costSoFar = 0;
        // nodeData.Add(start, currentNodePathData);

        while (frontier.Count > 0)
        {
            currentNode = frontier.Pop();

            foreach (Edge edge in currentNode.edges)
            {
                Node nextNode = edge.node;
                int newCost = costSoFar[currentNode] + edge.cost;
                if (newCost > range)
                {
                    continue;
                }
                if (!costSoFar.ContainsKey(nextNode) || costSoFar[nextNode] > newCost)
                {
                    costSoFar[nextNode] = newCost;
                    frontier.Push(nextNode, newCost);
                    cameFrom[nextNode] = currentNode;
                }
            }
        }
        cameFrom.Remove(start);
        return cameFrom;
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

    static public void DrawDebugArea(Dictionary<Node, Node> nodeSet, float duration = 5.0f)
    {
        Renderer rd;
        foreach (var item in nodeSet)
        {
            if (item.Value != null)
            {
                Debug.DrawLine(item.Key.transform.position, item.Value.transform.position, Color.magenta, duration);
                rd = item.Key.transform.gameObject.GetComponent<Renderer>();
                rd.material.color = Color.blue;
            }
        }
    }
}