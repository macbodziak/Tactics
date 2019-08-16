using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Graph.CreateNewGraph(3, 2);
        // Graph.LoadGraphFromFile("map01.txt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
