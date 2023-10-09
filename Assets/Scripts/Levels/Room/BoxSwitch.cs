using System.Collections;
using UnityEngine;

public class BoxSwitch : MonoBehaviour
{
    [SerializeField] private GameObject triggered;
    [SerializeField] private float moveAmt;
    
    private SpriteRenderer rend;
    private Vector3 startingPos;
    private Vector3 endingPos;

    private void Awake()
    {
        if (moveAmt == 0)
            moveAmt = 2.69f;

        rend = GetComponent<SpriteRenderer>();
        startingPos = triggered.transform.position;
        endingPos = new Vector3(triggered.transform.position.x, triggered.transform.position.y + moveAmt, triggered.transform.position.z);

        Physics2D.IgnoreLayerCollision(12, 9, true);
        Physics2D.IgnoreLayerCollision(12, 10, true);
        Physics2D.IgnoreLayerCollision(12, 11, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Box" && triggered.gameObject.tag == "Door")
        {
            collision.transform.position = new Vector3(transform.position.x, collision.transform.position.y, collision.transform.position.z);
            rend.color = new Color(0.3f, 1, 0);
            StartCoroutine(triggered.GetComponent<DoorTrigger>().ChangeDoorState(triggered.gameObject, endingPos, 0.75f));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Box" && triggered.gameObject.tag == "Door")
        {
            rend.color = Color.white;
            StartCoroutine(triggered.GetComponent<DoorTrigger>().ChangeDoorState(triggered.gameObject, startingPos, 0.75f));
        }
    }

    
}
