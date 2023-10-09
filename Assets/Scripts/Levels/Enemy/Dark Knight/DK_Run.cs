using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DK_Run : StateMachineBehaviour
{
    [SerializeField] private float speed;
    public float leftEdge, rightEdge;
    [SerializeField] private float meleeCD;

    //References
    private Rigidbody2D body;
    private DarkKnight dkScript;

    private float meleeTimer, teleportTimer;
    private float teleportCD;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        body = animator.GetComponentInParent<Rigidbody2D>();
        dkScript = animator.GetComponentInParent<DarkKnight>();
        teleportCD = Random.Range(4f, 7f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dkScript.LookAtPlayer();

        Vector2 target = new Vector2(PlayerProperties.position.x, body.position.y);
        Vector2 newPos = Vector2.MoveTowards(body.position, target, speed * Time.fixedDeltaTime);

        if(newPos.x > leftEdge && newPos.x < rightEdge)
            body.MovePosition(newPos);

        if(dkScript.PlayerInSight() && meleeTimer > meleeCD)
        {
            animator.SetTrigger("attack");
            meleeTimer = 0;
        }

        if(teleportTimer > teleportCD)
        {
            animator.SetTrigger("teleport");
            teleportTimer = 0;
            teleportCD = Random.Range(4f, 7f);
        }

        meleeTimer += Time.deltaTime;
        teleportTimer += Time.deltaTime;

        if(dkScript.GetComponent<Health>().currentHealth <= 30)
        {
            animator.SetBool("phase2", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("attack");
        animator.ResetTrigger("teleport");
    }
}
