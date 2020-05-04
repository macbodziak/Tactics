using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterCommand : ICommand
{
    Character character;
    List<Node> path;
    int nodeCount;
    int i = 0;
    float distanceThisFrame = 0.0f;
    float distanceToNextNode;

    bool isFinished = false;
    CommandQueue parent;

    public MoveCharacterCommand(CommandQueue commandQueue, Character character, List<Node> path)
    {
        this.character = character;
        this.path = path;
        nodeCount = path.Count;
        parent = commandQueue;

        // update rotation
        if (path != null && nodeCount >= 1)
        {
            character.transform.LookAt(path[0].transform.position);
        }
    }

    public void Update()
    {
        if (nodeCount < 0)
        {
            isFinished = true;
            parent.NextCommand();
        }
        character.AnimStartRunning();
        character.hasCover = false;

        if (character.transform.position != path[nodeCount - 1].transform.position)
        {
            distanceThisFrame = character.speed * Time.deltaTime;
            distanceToNextNode = Vector3.Distance(character.transform.position, path[i].transform.position);


            character.transform.position = Vector3.MoveTowards(character.transform.position, path[i].transform.position, distanceThisFrame);

            while (distanceThisFrame > distanceToNextNode)
            {
                distanceThisFrame -= distanceToNextNode;
                ++i;
                if (i >= nodeCount)
                {
                    break;
                }
                character.transform.LookAt(path[i].transform.position);
                distanceToNextNode = Vector3.Distance(character.transform.position, path[i].transform.position);
                character.transform.position = Vector3.MoveTowards(character.transform.position, path[i].transform.position, distanceThisFrame);
            }
        }
        else
        {
            character.node.character = null;
            path[path.Count - 1].character = character;
            isFinished = true;
            character.AnimStopRunning();
            character.hasCover = Graph.instance.CheckCover(path[path.Count - 1]);
            parent.NextCommand();
        }
    }

    public bool IsFinished()
    {
        return isFinished;
    }
}
