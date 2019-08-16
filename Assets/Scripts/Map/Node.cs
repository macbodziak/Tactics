using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    bool walkable = true;
    //other things
    public List<Edge> edges = new List<Edge>();

    public Node() 
    {
        edges = new List<Edge>();
    }
}
