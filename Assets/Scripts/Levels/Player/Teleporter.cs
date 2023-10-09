using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxLifetime;
    [SerializeField] private Transform player;

    [Header ("Camera")]
    [SerializeField] private CameraController cam;

    private float camMoveAmount;

    private Vector3 moveTo;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (hit)
            return;

        transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
        lifetime += Time.deltaTime;

        if (lifetime > maxLifetime)
        {
            Deactivate();
        }
    }

    public void setPosition(Vector3 mousePos)
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<CircleCollider2D>(), GameObject.Find("Player").GetComponent<BoxCollider2D>());
        Physics2D.IgnoreLayerCollision(9, 11, true);

        gameObject.SetActive(true);
        hit = false;
        circleCollider.enabled = true;
        lifetime = 0;
        PlayerMovement.canTeleport = false;

        transform.position = player.transform.position;
        moveTo = (mousePos - transform.position) * 10 + transform.position;
        moveTo = new Vector3(moveTo.x, moveTo.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "yTrigger" || collision.tag == "Player" || collision.tag == "Arrow" ||
            collision.gameObject.name.Contains("Portal"))
            return;

        hit = true;
        circleCollider.enabled = false;

        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Health>().TakeDamage(1);
            StartCoroutine("TeleportCooldown");
        }   
        else
        {
            player.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            //cam.CheckCameraPosition();

            Deactivate();
        }
    }

    private IEnumerator TeleportCooldown()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.SetActive(false);
        PlayerMovement.canTeleport = true;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        PlayerMovement.canTeleport = true;
    }
}
