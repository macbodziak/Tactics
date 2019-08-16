using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Edge {
    Node node;
    int cost;

    public Edge(Node otherNode, int cost) {
        node = otherNode;
        this.cost = cost;
    }
}
