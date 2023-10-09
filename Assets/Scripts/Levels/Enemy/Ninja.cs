using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ninja : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private int damage;

    [Header("Melee Attack")]
    [SerializeField] private float range;
    [SerializeField] private float atkCooldown;
    [SerializeField] private float colliderDistMelee;

    [Header("Tornado Attack")]
    [SerializeField] private float tornadoCooldown;
    [SerializeField] private float tornadoRange;
    [SerializeField] private float duration;
    [SerializeField] private float colliderDistTornado;
    [SerializeField] private float speed;
    [SerializeField] private Sprite tornadoSprite;
    
    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Parameters")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private BoxCollider2D physicalCollider;

    [Header("Health Bar")]
    [SerializeField] private Image totalHealthBar; 
    [SerializeField] private Image currentHealthBar;

    private float atkCooldownTimer = Mathf.Infinity;
    private float tornadoCooldownTimer = Mathf.Infinity;
    private float tornadoTimer;
    private bool isTornado;
    private bool touchingWall;
    private float direction;

    //WORK ON TORNADO ATTACK TOMORROW

    //References
    private Animator anim;
    private Health playerHealth;
    private Health health;
    private SpriteRenderer renderer;
    private Rigidbody2D body;
    private Sprite origSprite;


    //Everything above came from knight
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
            touchingWall = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
            touchingWall = false;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        origSprite = GetComponent<SpriteRenderer>().sprite;
        health = GetComponent<Health>();

        Physics2D.IgnoreCollision(physicalCollider, GameObject.Find("Player").GetComponent<BoxCollider2D>());
    }

    private void Update()
    {
        atkCooldownTimer += Time.deltaTime;
        tornadoCooldownTimer += Time.deltaTime;

        //Always look at player
        if (!isTornado)
        {
            float sign = Mathf.Sign(PlayerProperties.position.x - transform.position.x);
            float origScale = transform.localScale.x;

            transform.localScale = new Vector3(sign * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (origScale != transform.localScale.x)
                currentHealthBar.transform.Rotate(0, 0, 180);
        }

        //Attack only when enemy sees player
        if (PlayerInSightMelee())
        {
            if (atkCooldownTimer >= atkCooldown && !isTornado)
            {
                atkCooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        //Check if player in range for tornado attack
        if (PlayerInSightTornado())
        {
            //Set all tornado properties
            if (tornadoCooldownTimer >= tornadoCooldown && health.currentHealth > 0 && !isTornado && !touchingWall)
                SetTornado();
        }

        //Tornado attack
        if (isTornado)
        {
            tornadoTimer += Time.deltaTime;

            if (tornadoTimer < duration && !touchingWall)
            {
                transform.Rotate(0, 180, 0);
                tornadoCooldownTimer = 0;
            }
            //Get rid of all tornado properties when duration expires
            else
            {
                body.MovePosition(new Vector2(transform.position.x - 0.1f * direction, transform.position.y));
                isTornado = false;
                //body.velocity = Vector2.zero;
                tornadoCooldownTimer = 0;
                anim.enabled = true;
                //body.isKinematic = false;
                renderer.sprite = origSprite;
                transform.localScale = new Vector3(transform.localScale.x * 2,
                    transform.localScale.y * 2, transform.localScale.z);
                transform.eulerAngles = Vector3.zero;
                //body.gravityScale = 3;
                tornadoCooldownTimer = 0;
                tornadoTimer = 0;
                boxCollider.size = new Vector2(boxCollider.size.x / 2f, boxCollider.size.y / 2f);
                physicalCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y);

                totalHealthBar.rectTransform.sizeDelta = new Vector2(totalHealthBar.rectTransform.sizeDelta.x / 2,
                    totalHealthBar.rectTransform.sizeDelta.y / 2);
                currentHealthBar.rectTransform.sizeDelta = new Vector2(currentHealthBar.rectTransform.sizeDelta.x / 2,
                    currentHealthBar.rectTransform.sizeDelta.y / 2);
                currentHealthBar.color = Color.white;

                health.invulnerable = false;
            }
        }

        if (health.currentHealth <= 0)
        {
            body.gravityScale = 0;
        }
    }

    private void FixedUpdate()
    {
        if(isTornado)
            body.MovePosition(new Vector2(transform.position.x + speed * Time.deltaTime * direction, transform.position.y));
    }

    private void SetTornado()
    {
        if(health.currentHealth > 0)
        {

            renderer.color = Color.white;
            tornadoCooldownTimer = 0;
            isTornado = true;
            anim.enabled = false;
            //body.isKinematic = true;
            direction = Mathf.Sign(PlayerProperties.position.x - transform.position.x);
            renderer.sprite = tornadoSprite;
            transform.localScale = new Vector3(transform.localScale.x * 0.5f,
                transform.localScale.y * 0.5f, transform.localScale.z);
            //body.gravityScale = 0;
            boxCollider.size = new Vector2(boxCollider.size.x * 2f, boxCollider.size.y * 2f);
            physicalCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y);

            totalHealthBar.rectTransform.sizeDelta = new Vector2(totalHealthBar.rectTransform.sizeDelta.x * 2,
                totalHealthBar.rectTransform.sizeDelta.y * 2);
            currentHealthBar.rectTransform.sizeDelta = new Vector2(currentHealthBar.rectTransform.sizeDelta.x * 2,
                currentHealthBar.rectTransform.sizeDelta.y * 2);
            currentHealthBar.color = new Color(0.5f, 0.5f, 0);

            health.invulnerable = true;
        }
        
    }

    //Came from knight
    private bool PlayerInSightMelee()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistMelee,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }
    
    private bool PlayerInSightTornado()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * tornadoRange * transform.localScale.x * colliderDistTornado,
            new Vector3(boxCollider.bounds.size.x * tornadoRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    //Came from knight
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Melee attack
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistMelee,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        
        //Tornado attack
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * tornadoRange * transform.localScale.x * colliderDistTornado,
            new Vector3(boxCollider.bounds.size.x * tornadoRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    //Came from knight
    private void DamagePlayer()
    {
        if (PlayerInSightMelee() && !isTornado)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
