using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class GameController : MonoBehaviour, ICommandQueueListener
{
    private static GameController myInstance;
    [SerializeField] Graph graph;
    [SerializeField] Character character;
    Dictionary<Node, Pathfinder.NodePathData> area = null;
    bool isExecuting = false;
    Character selectedChar = null;

    CommandQueue commands = new CommandQueue();

    public static GameController instance
    {
        get
        {
            return myInstance;
        }
    }

    private void Awake()
    {
        if (myInstance != null && myInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        myInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        commands.RegisterListener(this);
        Graph.DrawDebugGraph(graph);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        commands.Update();
    }

    public void OnCharacterClick(GameObject go)
    {
        if (selectedChar != null && selectedChar.Equals(go.transform.parent))
        {
            return;
        }
        if (area != null)
        {
            Graph.UnHighlightArea(area);
            area.Clear();
        }
        selectedChar = go.GetComponentInParent<Character>();
        if (selectedChar != null)
        {
            area = Pathfinder.FindWalkableArea(graph, selectedChar.node, selectedChar.range);
            Graph.HighlightArea(area);
        }
    }

    public void OnTileClick(GameObject go)
    {
        Node node = go.GetComponent<Node>();

        if (node.tag == "WalkableNode")
        {
            OnWalkableNodeClick(go);
        }
    }

    public void OnWalkableNodeClick(GameObject go)
    {
        Node node = go.GetComponent<Node>();

        if (selectedChar != null)
        {
            if (area != null && area.ContainsKey(node))
            {
                Graph.UnHighlightArea(area);
                List<Node> path = Pathfinder.GetPathFromArea(area, node);
                // Graph.DrawDebugPath(path);
                commands.Push(new MoveCharacterCommand(commands, selectedChar, path));
                area.Clear();
                commands.Execute();
                isExecuting = true;
            }
        }

    }

    public void OnNextCommand()
    {

    }

    public void OnCommandsFinished()
    {
        isExecuting = false;
    }

    public void HandleInput()
    {
        if (isExecuting)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform != null)
                {
                    Debug.Log("Clicked on: " + hit.transform.name + " tag: " + hit.transform.tag);
                    switch (hit.transform.tag)
                    {
                        case "Character":
                            OnCharacterClick(hit.transform.gameObject);
                            break;
                        case "WalkableNode":
                            OnWalkableNodeClick(hit.transform.gameObject);
                            break;
                    }
                }
            }
        }

        //---Debug
        HandleDebugInput();
    }

    void HandleDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Graph.DrawDebugGraph(graph);
        }
    }
}
