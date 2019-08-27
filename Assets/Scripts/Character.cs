using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor.Animations;

public class Character : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] Node _node;
    bool hasCover = false;

    Animator animator;

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

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        if(animator == null)
        {
            Debug.Log(name + " has no animator controller!");
        }
    }
    private void Start() {
        if(_node != null)
        {
            _node.character = this;
        }
    }

    public void AnimStartRunning()
    {
        animator.SetBool("IsRunning", true);
    }

    public void AnimStopRunning()
    {
        animator.SetBool("IsRunning", false);
    }

}
