using UnityEngine;

public class EnemyProjectile : EnemyDamage //Will damage the player evrey time it touches
{
    [SerializeField] public float speed;
    [SerializeField] private float resetTime;
    [SerializeField] private Transform player;
    
    private float lifeTime;

    //References
    private Animator anim;
    private BoxCollider2D collider;

    private bool hit;

    public Vector3 moveTo;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifeTime = 0;
        gameObject.SetActive(true);
        if(gameObject.tag == "WizardFireball")
        {
            var dir = moveTo - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        collider.enabled = true;
    }

    private void Update()
    {
        if (hit)
            return;

        if(gameObject.tag == "WizardFireball")
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
            
        else
            transform.Translate(speed * Time.deltaTime, 0, 0);

        lifeTime += Time.deltaTime;
        if (lifeTime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Arrow")
            return;

        hit = true;
        base.OnTriggerStay2D(collision);
        collider.enabled = true;

        if (anim != null)
            anim.SetTrigger("hit"); //when this object is a fireball explode it
        else
            gameObject.SetActive(false); //When this hits any object deactivate arrow
    }

    private void DeactivateCollider()
    {
        collider.enabled = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
