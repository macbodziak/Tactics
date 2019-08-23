using System.Collections;
using System.Collections.Generic;
// using UnityEngine;

public class CommandQueue
{
    bool isExecuting = false;
    ICommand currentCommand = null;
    Queue<ICommand> commands = new Queue<ICommand>();

    List<ICommandQueueListener> listeners = new List<ICommandQueueListener>();

    public void Push(ICommand com)
    {
        commands.Enqueue(com);
    }

    public void Execute()
    {
        if (commands.Count > 0)
        {
            isExecuting = true;
            currentCommand = commands.Dequeue();
        }
    }

    public void Update()
    {
        if (currentCommand != null)
        {
            currentCommand.Update();
        }
    }

    public void NextCommand()
    {
        if (commands.Count > 0)
        {
            currentCommand = commands.Dequeue();
            NotifyOnNextCommand();
        }
        else
        {
            currentCommand = null;
            isExecuting = false;
            NotifyOnFinish();
        }
    }

    bool IsExecuting
    {
        get
        {
            return isExecuting;
        }
    }

    public void RegisterListener(ICommandQueueListener lis)
    {
        if(listeners.Contains(lis))
        {
            return;
        }
        listeners.Add(lis);
    }

    public void UnRegisterListener(ICommandQueueListener lis)
    {
        listeners.Remove(lis);
    }

    void NotifyOnFinish()
    {
        foreach (var lis in listeners)
        {
            lis.OnCommandsFinished();
        }
    }

    void NotifyOnNextCommand()
    {
        foreach (var lis in listeners)
        {
            lis.OnNextCommand();
        }
    }
}
