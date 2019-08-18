using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Graph.CreateNewGraph(15, 15);
        // Graph.LoadGraphFromFile("map01.txt");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    Node no = hit.transform.GetComponent<Node>();
                    if(no != null)
                    {
                        Debug.Log("drawing edges of " + no.name);
                        no.DrawDebugEdges();
                    }
                }
            }
        }
    }
}
