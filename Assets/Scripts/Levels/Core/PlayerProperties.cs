using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    [SerializeField] private Behaviour[] compts;
    

    [Header ("Level Items")]
    [SerializeField] private Item[] lvlItems;
    public static Item[] levelItems;

    public static Behaviour[] components;
    
    public static Vector3 position;
    public static GameObject[] currentItems;
    public static GameObject[] effects;

    public static float itemTimer;
    public static bool camAuto;
    public static bool gamePaused;
    public static float damage;

    private float itemCooldown;

    private Rigidbody2D body;
    

    private void Awake()
    {
        levelItems = lvlItems;
        itemTimer = 0;
        itemCooldown = 0.25f;
        effects = new GameObject[3];
        components = compts;
        camAuto = false;
        damage = 1;

        currentItems = new GameObject[2];
        body = GetComponent<Rigidbody2D>();

        //TESTING

    }

    private void DropItem()
    {
        bool pickedUp = false;

        foreach (Item item in levelItems)
        {
            if (item.InRange() && item.gameObject.activeInHierarchy)
            {
                //Box-Box
                if (item.tag == "Box" && currentItems[0] != null)
                {
                    Drop(0);
                }
                //Teleporter-Teleporter
                else if (item.tag != "Box" && currentItems[1] != null)
                {
                    Drop(1);
                }

                pickedUp = true;
                break;
            }
        }

        if (!pickedUp)
        {
            for (int i = 0; i < currentItems.Length; i++)
            {
                if (currentItems[i] != null)
                {
                    Drop(i);
                    break;
                }
            }
        }
    }

    private void Drop(int index)
    {
        currentItems[index].SetActive(true);

        //Flip item gravity
        if (Mathf.Sign(currentItems[index].GetComponent<Rigidbody2D>().gravityScale) != Mathf.Sign(body.gravityScale))
        {
            currentItems[index].GetComponent<Rigidbody2D>().gravityScale *= -1;
            currentItems[index].transform.localScale = new Vector3(currentItems[index].transform.localScale.x,
                -currentItems[index].transform.localScale.y, currentItems[index].transform.localScale.z);
        }
            

        currentItems[index].transform.position = transform.GetChild(0).position;

        itemTimer = 0;
        currentItems[index] = null;
    }

    private void Update()
    {
        //PlayerStats.timePlayed += Time.deltaTime;

        position = transform.position;
        itemTimer += Time.deltaTime;

        Physics2D.IgnoreLayerCollision(8, 9, true);

        //Drop item
        if ((currentItems[0] != null || currentItems[1] != null) && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && itemTimer > itemCooldown)
        {

            DropItem();

        }
    }
}
