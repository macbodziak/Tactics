using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeObject : MonoBehaviour
{
    Node node;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Node inNode) 
    {
        Debug.Log(name + " : init");
        node = inNode;
        if(node == null)
        {
            Debug.Log("node = null");
        }
    }

    public void DrawDebugEdges() 
    {
        if(node == null)
        {
            Debug.Log("node = null");
        }
        foreach(Edge edge in node.edges)
        {  
            if(edge.node == null)
            {
                Debug.Log("edge.node == null");
            }  
            if(edge.node.no == null)
            {
                Debug.Log("edge.node.no == null");
            }  
        Debug.DrawLine(transform.position, edge.node.no.transform.position,Color.green,2.0f);
        }
    }
}
