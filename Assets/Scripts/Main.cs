using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Main : MonoBehaviour
{
    [SerializeField] Graph graph;
    [SerializeField] Node start;
    [SerializeField] Node goal;

    [SerializeField] int range;
    // Start is called before the first frame update
    void Start()
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        Dictionary<Node, Pathfinder.NodePathData> area = Pathfinder.FindWalkableArea(graph, start, range);
        sw.Stop();
        Debug.Log("Time to exectue area search: " + sw.Elapsed);
        Pathfinder.DrawDebugArea(area,100f);
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
                        Dictionary<Node, Pathfinder.NodePathData> area = Pathfinder.FindWalkableArea(graph, start, range);
                        if(area.ContainsKey(no))
                        {
                            Debug.Log("Cost to go here: " + area[no].costSoFar);
                        }
                        else
                        {
                            Debug.Log("Can not reach!");
                        }
                    }
                }
            }
        }
    }
}
