using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DK_Teleport : StateMachineBehaviour
{
    private float leftEdge, rightEdge;
    private DK_Run runScript;
    private BoxCollider2D collider;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initialize all values
        runScript = animator.GetBehaviour<DK_Run>();
        leftEdge = runScript.leftEdge;
        rightEdge = runScript.rightEdge;
        collider = animator.GetComponentInParent<BoxCollider2D>();

        collider.enabled = false;
    }

    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        collider.enabled = true;
        animator.ResetTrigger("run");
    }
}
