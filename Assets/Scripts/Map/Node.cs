using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    bool myWalkable = true;
    public NodeObject no;
    //other things
    public List<Edge> edges = new List<Edge>();

    public Node(NodeObject gobj) 
    {
        no = gobj;
        edges = new List<Edge>();
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
}
