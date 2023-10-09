using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    private float playerPosX;
    private float playerPosY;

    private float leftBorder;
    private float rightBorder;
    private float topBorder;
    private float bottomBorder;

    private Image itemImage;
    private Image itemImage2;

    private void Awake()
    {
        itemImage = GameObject.Find("CurrentItemImage").GetComponent<Image>();
        itemImage2 = GameObject.Find("CurrentItemImage2").GetComponent<Image>();

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public bool InRange()
    {
        return playerPosX > leftBorder && playerPosX < rightBorder && playerPosY > bottomBorder && playerPosY < topBorder;
    }

    private void Update()
    {
        playerPosX = PlayerProperties.position.x;
        playerPosY = PlayerProperties.position.y;

        leftBorder = transform.position.x - 1.3f;
        rightBorder = transform.position.x + 1.3f;

        if(GetComponent<Rigidbody2D>().gravityScale > 0)
        {
            bottomBorder = transform.position.y - 0.25f;
            topBorder = transform.position.y + 1;
        }
        else
        {
            bottomBorder = transform.position.y - 1;
            topBorder = transform.position.y + 0.25f;
        }

        if (InRange())
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {

                if (gameObject.tag == "Box")
                {
                    PlayerProperties.currentItems[0] = gameObject;
                    gameObject.SetActive(false);
                    PlayerProperties.itemTimer = 0;
                }

                else
                {
                    PlayerProperties.currentItems[1] = gameObject;
                    gameObject.SetActive(false);
                    PlayerProperties.itemTimer = 0;
                }
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }

        //Populate item images with proper sprite image

        if (PlayerProperties.currentItems[0] != null)
        {
            itemImage.sprite = PlayerProperties.currentItems[0].GetComponent<SpriteRenderer>().sprite;
            itemImage.color = Color.white;
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = new Color(1, 1, 1, 0);
        }
        
        if (PlayerProperties.currentItems[1] != null)
        {
            itemImage2.sprite = PlayerProperties.currentItems[1].GetComponent<SpriteRenderer>().sprite;
            itemImage2.color = Color.white;
        }
        else
        {
            itemImage2.sprite = null;
            itemImage2.color = new Color(1, 1, 1, 0);
        }
        
    }
}
