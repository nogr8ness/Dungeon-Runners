using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Only Use If Purple Portal")]
    [SerializeField] private GameObject completeMenu;
    [SerializeField] private int lvlNum;

    [Header("Only Use If Blue Portal")]
    [SerializeField] private bool bluePortal;
    [SerializeField] private Portal linkedPortal;
    [SerializeField] public Vector3 movePlayerTo;
    [SerializeField] private float newScaleX;
    [SerializeField] private float newScaleY;
    [SerializeField] private GameObject fade;

    private Rigidbody2D playerBody;
    private float gravScale;
    private Vector3 newScale;
    private Animator fadeAnim;

    private void Awake()
    {
        playerBody = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        //Scale for when player comes out of portal
        if (newScaleX == 0) newScaleX = 1;
        if (newScaleY == 0) newScaleY = playerBody.transform.localScale.y * Mathf.Sign(playerBody.gravityScale);
        newScale = new Vector3(newScaleX, newScaleY, 1);

        if(fade != null)
            fadeAnim = fade.GetComponent<Animator>();
    }

    private void Update()
    {
        //var rotationVector = transform.rotation.eulerAngles;
        //rotationVector.z += 0.75f;
        //transform.rotation = Quaternion.Euler(rotationVector);
        transform.Rotate(0, 0, 180 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            CameraController.freeze = true;
            Timer.pause = true;

            foreach (Behaviour component in PlayerProperties.components)
            {
                component.enabled = false;
            }

            gravScale = playerBody.gravityScale;
            playerBody.gravityScale = 0;

            StartCoroutine(MoveToCenter(GameObject.Find("Player").transform, PlayerProperties.position, 1));

            //SceneManager.LoadScene("Main Menu");
        }
    }

    private IEnumerator MoveToCenter(Transform player, Vector3 playerPos, float time)
    {
        float elapsedTime = 0;
        Vector3 origScale = player.localScale;
        player.GetComponent<Health>().invulnerable = true;
        player.GetComponent<BoxCollider2D>().enabled = false;

        PauseButton.Disable();

        while(elapsedTime < time)
        {
            player.position = Vector3.Lerp(playerPos, transform.position, elapsedTime / time);
            player.localScale = Vector3.Lerp(origScale, Vector3.zero, elapsedTime / time);
            player.transform.Rotate(0, 0, 450 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!bluePortal) {
            player.position = Vector3.zero;
            player.localScale = Vector3.zero;
            player.gameObject.SetActive(false);

            //Add 8 to lvl completed if in hard mode
            int hardAddition = 0;
            if (PlayerPrefs.GetInt("hardMode") == 1)
                hardAddition = 8;

            if (PlayerPrefs.GetInt("lvlsBeaten") < lvlNum + hardAddition)
                PlayerPrefs.SetInt("lvlsBeaten", lvlNum + hardAddition);

            Time.timeScale = 0f;
            completeMenu.SetActive(true);
        }
        else
        {
            playerBody.velocity = Vector2.zero;
            linkedPortal.GetComponent<CircleCollider2D>().enabled = false;

            //Fade screen out
            fade.GetComponent<Image>().enabled = true;
            fadeAnim.SetTrigger("fadeOut");
            yield return new WaitForSeconds(0.6f);

            //Teleport player to other portal and set camera pos
            player.position = linkedPortal.movePlayerTo;
            CameraController.freeze = false;
            yield return new WaitForSeconds(0.05f);
            CameraController.freeze = true;
            player.position = linkedPortal.transform.position;

            //Camera.main.transform.position = new Vector3(linkedPortal.movePlayerTo.x, linkedPortal.movePlayerTo.y, 
                //Camera.main.transform.position.z);

            GetComponent<CircleCollider2D>().enabled = true;

            

            StartCoroutine(linkedPortal.MoveToPosition(player, linkedPortal.movePlayerTo, 1, gravScale));
        }

        yield return null;
    }

    //This is called from the OTHER portal
    public IEnumerator MoveToPosition(Transform player, Vector3 moveTo, float time, float gravScale)
    {
        //Fade screen in
        fadeAnim.SetTrigger("fadeIn");
        yield return new WaitForSeconds(0.6f);
        fade.GetComponent<Image>().enabled = false;

        float elapsedTime = 0;
        Vector3 origScale = player.localScale; //This should be (0, 0, 0)

        while (elapsedTime < time)
        {
            player.position = Vector3.Lerp(transform.position, moveTo, elapsedTime / time);
            player.localScale = Vector3.Lerp(origScale, Vector3.one, elapsedTime / time);
            player.transform.Rotate(0, 0, 510 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.localScale = newScale;
        player.rotation = Quaternion.identity;
        player.position = moveTo;
        player.GetComponent<Health>().invulnerable = false;
        player.GetComponent<BoxCollider2D>().enabled = true;

        CameraController.freeze = false;
        Timer.pause = false;

        foreach (Behaviour component in PlayerProperties.components)
        {
            component.enabled = true;
        }

        playerBody.gravityScale = Mathf.Abs(gravScale) * Mathf.Sign(newScaleY);
        GetComponent<CircleCollider2D>().enabled = true;
        PauseButton.Enable();
    }
}
