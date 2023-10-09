using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    
    [SerializeField] private Animator anim;
    [SerializeField] PlayerMovement movement;

    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer > attackCooldown && movement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    GameObject.Find("Player").GetComponent<Health>().AddHealth(5);
        //}
        
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    GameObject.Find("Player").GetComponent<Health>().TakeDamage(1);
        //}
           
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().setDirection(Mathf.Sign(transform.localScale.x));

        PlayerPrefs.SetInt("attacks", PlayerPrefs.GetInt("attacks") + 1);
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
