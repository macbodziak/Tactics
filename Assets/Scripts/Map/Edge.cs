using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge 
{
    Node myNode;
    int myCost;

    public  Edge(Node otherNode, int cost)
    {
        myNode = otherNode;
        myCost = cost;
    }

    public Node node
    {
        get
        {
            return myNode;
        }
    }

    public int cost
    {
        get
        {
            return myCost;
        }
    }
}
