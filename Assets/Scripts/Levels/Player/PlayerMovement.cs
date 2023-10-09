using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float forgivenessTime;

    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;

    [SerializeField] private Teleporter teleporter;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    

    private float horizontalInput;
    public bool grounded;
    public static bool canTeleport;

    private GameObject currentItem;
    private float lowGravScale;

    private void OnEnable()
    {
        //INITIALIZE ALL TEXT VISIBILITIES AND DOOR POSITIONS + other stuff (maybe)
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        canTeleport = true;

        lowGravScale = 2.75f; //Change this to change low gravity
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Flip player depending on input
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if(horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.C))
        {
            if (grounded)
                Jump();
        }

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);

        //Item logic
        if (PlayerProperties.currentItems[1] != null)
        {
            //Teleporter logic
            if(PlayerProperties.currentItems[1].ToString().Substring(0, 10) == "Teleporter")
            {
                if (Input.GetMouseButtonDown(0) && grounded && canTeleport && !PlayerProperties.gamePaused)
                {
                    Vector3 mousePos = Input.mousePosition;

                    if (mousePos.x < Screen.width * 0.91 || mousePos.y < Screen.height * 0.87)
                        teleporter.setPosition(cam.ScreenToWorldPoint(Input.mousePosition));
                }
            }

            //GraviSwitcher logic
            if(PlayerProperties.currentItems[1].ToString().Substring(0, 13) == "GraviSwitcher")
            {
                if((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.L)) && grounded)
                {
                    body.gravityScale *= -1;
                    transform.position = new Vector3(transform.position.x, transform.position.y + 1.25f * Mathf.Sign(body.gravityScale), 
                        transform.position.z);
                    transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                }
            }  
        }

        ApplyEffects();
    }
    
    private void Jump()
    {
        if(grounded == true)
        {
            if(body.gravityScale > 0)
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
                body.velocity = new Vector2(body.velocity.x, -jumpPower);

            grounded = false;
            anim.SetTrigger("jump");
            PlayerPrefs.SetInt("jumps", PlayerPrefs.GetInt("jumps") + 1);
        }
    }

    private void ApplyEffects()
    {
        foreach (GameObject effect in PlayerProperties.effects)
        {
            if (effect != null)
            {
                if (effect.name == "LowGravity" && Mathf.Abs(body.gravityScale) > 3 / lowGravScale + 0.001f)
                {
                    body.gravityScale /= lowGravScale;
                    
                }

                if (effect.name == "DoubleDamage")
                    PlayerProperties.damage = 1.5f;

                if (effect.name == "HalfSpeed" && speed > 4)
                    speed /= 2;

                if (effect.name == "NoJump" && jumpPower > 4.34f)
                    jumpPower /= 4;

                //Cookie is applied in the Health script
            }
        }

        if (EffectNotPresent("LowGravity") && Mathf.Abs(body.gravityScale) < 3)
            body.gravityScale *= lowGravScale;

        if (EffectNotPresent("DoubleDamage"))
            PlayerProperties.damage = 1;

        if (EffectNotPresent("HalfSpeed") && speed < 8)
            speed *= 2;

        if (EffectNotPresent("NoJump") && jumpPower < 13)
            jumpPower *= 3;
    }

    private bool EffectNotPresent(string effect)
    {
        bool present = false;

        foreach (GameObject _effect in PlayerProperties.effects)
        {
            if (_effect != null)
            {
                if (_effect.name == effect)
                    present = true;
            }
        }

        return !present;
    }

    public bool canAttack()
    {
        return grounded;
    }

    //Handles grounded logic for both collisions and triggers
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (body.gravityScale > 0 && collision.gameObject.tag == "Ground")
            grounded = true;

        if (body.gravityScale < 0 && collision.gameObject.tag == "Ceiling")
            grounded = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Ceiling")
            grounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Ceiling")
            StartCoroutine(LeaveGround());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Ceiling")
            StartCoroutine(LeaveGround());
    }

    //Jump forgiveness code
    private IEnumerator LeaveGround()
    {
        if(body.velocity.y * body.gravityScale <= 0)
        {
            yield return new WaitForSeconds(forgivenessTime);
            grounded = false;
        }
        grounded = false;
        yield return null;
    }
}
