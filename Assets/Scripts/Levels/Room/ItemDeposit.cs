using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeposit : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float moveAmt;

    private Vector3 startPos, endPos;
    private SpriteRenderer renderer;

    private void Awake()
    {
        if (moveAmt == 0)
            moveAmt = 2.69f;

        startPos = door.transform.position;
        endPos = new Vector3(startPos.x, startPos.y + moveAmt, startPos.z);

        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            StartCoroutine(door.GetComponent<DoorTrigger>().ChangeDoorState(door, endPos, 0.75f));
            renderer.color = new Color(0, 1, 0, 0.3f);
        }
            
    }
}
