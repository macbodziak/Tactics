using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] Node _node;
    public int range;

    public Node node
    {
        get
        {
            return _node;
        }

        set
        {
            _node = value;
        }
    }

    private void Start() {
        if(_node != null)
        {
            _node.character = this;
        }
    }
}
