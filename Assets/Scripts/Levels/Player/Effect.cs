using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Effect : MonoBehaviour
{
    private Vector3 pos1;
    private Vector3 pos2;
    private bool movingUp;

    private float speed;

    private Image effectImage;

    private Health playerHealth;

    [SerializeField] private GameObject timerObj;
    [SerializeField] private float heartAmt;

    private void Awake()
    {
        pos1 = transform.position;
        pos2 = pos1 + new Vector3(0, 0.5f, 0);
        speed = 0.25f;
        playerHealth = GameObject.Find("Player").GetComponent<Health>();
    }

    private void Update()
    {
        if (movingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos2, speed * Time.deltaTime);

            if (pos2.y - transform.position.y <= 0.1f || transform.position.y - pos1.y >= 0.4f)
                speed = 0.125f;
            else
                speed = 0.25f;

            if (transform.position == pos2)
                movingUp = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pos1, speed * Time.deltaTime);

            if (transform.position.y - pos1.y <= 0.1f || transform.position.y - pos1.y >= 0.4f)
                speed = 0.125f;
            else
                speed = 0.25f;

            if (transform.position == pos1)
                movingUp = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.name == "ClearEffects")
            {
                PlayerProperties.effects = new GameObject[3];
                gameObject.SetActive(false);

                RemoveEffects();
            }

            else if (gameObject.name == "Timer")
            {
                timerObj.SetActive(true);
                gameObject.SetActive(false);
            }

            else if (gameObject.name == "ExtraHearts")
            {
                playerHealth.AddExtraHearts(heartAmt);
                gameObject.SetActive(false);
            }
                
            else
            {
                for (int i = 0; i < PlayerProperties.effects.Length; i++)
                {
                    //Do nothing if you get 2 of the same effect or have reached effect limit
                    if (PlayerProperties.effects[2] != null || (PlayerProperties.effects[i] != null 
                        && PlayerProperties.effects[i].name == gameObject.name))
                    {
                        gameObject.SetActive(false);
                        break;
                    }

                    if (PlayerProperties.effects[i] == null)
                    {
                        PlayerProperties.effects[i] = gameObject;

                        Image effectHolder = GameObject.Find("Canvas").transform.Find("CurrentEffect" + (i + 1)).GetComponent<Image>();
                        effectHolder.enabled = true;

                        effectImage = effectHolder.transform.GetChild(0).GetComponent<Image>();
                        effectImage.enabled = true;
                        effectImage.sprite = gameObject.transform.Find("EffectImage").GetComponent<SpriteRenderer>().sprite;

                        //Set opacity to 1
                        Color imageColor = gameObject.transform.Find("EffectImage").GetComponent<SpriteRenderer>().color;
                        imageColor.a = 1;
                        effectImage.color = imageColor;

                        if (PlayerProperties.effects[i].name == "DoubleDamage")
                            effectImage.rectTransform.sizeDelta = new Vector2(70f, 40f);

                        if (PlayerProperties.effects[i].name == "NoJump")
                            effectImage.rectTransform.sizeDelta = new Vector2(135f, 80f);

                        if (PlayerProperties.effects[i].name == "HalfSpeed" || PlayerProperties.effects[i].name == "Cookie")
                            effectImage.rectTransform.sizeDelta = new Vector2(75f, 75f);

                        gameObject.SetActive(false);
                        break;
                    }
                }
            }   
        }
    }

    //Removes image from top-right corner when an effect is removed
    private void RemoveEffects()
    {
        for (int i = 0; i < PlayerProperties.effects.Length; i++)
        {
            if (PlayerProperties.effects[i] == null)
            {
                Image effectHolder = GameObject.Find("Canvas").transform.Find("CurrentEffect" + (i + 1)).GetComponent<Image>();
                effectImage = effectHolder.transform.GetChild(0).GetComponent<Image>();

                effectHolder.enabled = false;
                effectImage.enabled = false;
                effectImage.sprite = gameObject.transform.Find("EffectImage").GetComponent<SpriteRenderer>().sprite;
                effectImage.color = new Color(1, 1, 1, 0);
            }
        }
    }
}