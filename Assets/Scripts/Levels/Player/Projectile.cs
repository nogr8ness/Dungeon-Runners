using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), GameObject.Find("Player").GetComponent<BoxCollider2D>());
    }

    private void Update()
    {
        Physics2D.IgnoreLayerCollision(9, 11, true);
        Physics2D.IgnoreLayerCollision(11, 12, true);
        Physics2D.IgnoreLayerCollision(11, 13, true);

        if (hit)
            return;

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 2)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Arrow")
            return;

        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("hit");

        if (collision.tag == "Enemy" && gameObject.tag != "DummyFireball")
        {
            if (collision.gameObject.name.Contains("Ninja"))
                collision.GetComponent<Health>().TakeDamage(PlayerProperties.damage / 2);

            else
                collision.GetComponent<Health>().TakeDamage(PlayerProperties.damage);
        }
            
    }

    public void setDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void DeactivateCollider()
    {
        boxCollider.enabled = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
