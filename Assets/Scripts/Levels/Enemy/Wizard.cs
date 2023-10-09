using UnityEngine;

public class Wizard : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Parameters")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private bool movingLeft;

    private Vector3 initScale;
    

    [Header("Idle Behavior")]
    [SerializeField] private float idleCooldown;

    private float idleTimer;

    [Header("Edges")]
    [SerializeField] private float leftEdge;
    [SerializeField] private float rightEdge;

    private float cooldownTimer = Mathf.Infinity;

    //References
    private Animator anim;
    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        initScale = transform.localScale;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (movingLeft)
        {
            if (transform.position.x >= leftEdge)
                MoveInDirection(-1);
               
            else
                ChangeDirection();
        }
        else
        {
            if (transform.position.x <= rightEdge)
                MoveInDirection(1);
            else
                ChangeDirection();
        }

        if (PlayerInSight())
        {
            if(cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        } 
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;

        //Make enemy face correct direction
        transform.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        //Move in that direction
        transform.position = new Vector3(transform.position.x + Time.deltaTime * _direction * speed,
            transform.position.y, transform.position.z);
    }

    private void ChangeDirection()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > idleCooldown)
        {
            movingLeft = !movingLeft;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -transform.eulerAngles.z);
            Transform healthCanvas = transform.GetChild(0).GetChild(1);
            healthCanvas.localScale = new Vector3(-healthCanvas.localScale.x, healthCanvas.localScale.y, healthCanvas.localScale.z);
        }
            
    }

    private bool PlayerInSight()
    {
        if (Mathf.Abs(PlayerProperties.position.y - transform.position.y) < 10 && ((transform.localScale.x == -1 && PlayerProperties.position.x >= transform.position.x - range && PlayerProperties.position.x < transform.position.x + range/2) ||
            ((transform.localScale.x == 1) && PlayerProperties.position.x <= transform.position.x + range && PlayerProperties.position.x > transform.position.x - range/2)))
        {
            return true;
        }

        return false;
    }

    private void Attack()
    {
        GameObject fireball = fireballs[FindFireball()];
        fireball.transform.position = firePoint.position;
        
        fireball.GetComponent<EnemyProjectile>().moveTo = (PlayerProperties.position - transform.position) * 10 + transform.position;

        //Potential nerf
        float deviation = Random.Range(-5f, 5f);
        fireball.GetComponent<EnemyProjectile>().moveTo.x += deviation * 10;


        fireball.GetComponent<EnemyProjectile>().ActivateProjectile();
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

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
