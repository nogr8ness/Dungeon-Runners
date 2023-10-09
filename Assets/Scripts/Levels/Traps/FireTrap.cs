using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Timers")]
    [SerializeField] private float wait;
    [SerializeField] private float activationTime;
    [SerializeField] private float activeTime;

    private Animator anim;

    private float waitTime;

    private bool triggered;
    private bool active;
    private bool firstTriggerOccurred;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        waitTime += Time.deltaTime;

        if (!triggered && waitTime >= wait)
            StartCoroutine(ActivateFireTrap());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }

    private IEnumerator ActivateFireTrap()
    {
        triggered = true;

        if (firstTriggerOccurred)
            yield return new WaitForSeconds(activationTime);
        else
            firstTriggerOccurred = true;

        //Activate trap after delay
        active = true;
        anim.SetBool("activate", true);

        //Deactivate trap
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activate", false);
    }
}
