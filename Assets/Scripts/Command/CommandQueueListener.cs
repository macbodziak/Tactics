using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandQueueListener
{
    void OnNextCommand();
    void OnCommandsFinished();
}
