using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3[] destinations;
    [SerializeField] private float[] times;
    [SerializeField] private float[] idleTimes;

    [SerializeField] private bool requiresTouch;
    [SerializeField] private GameObject[] activateOnTouch;

    private float elapsedTime;
    private int index;

    private void Awake()
    {
        elapsedTime = 0;
        if (!requiresTouch)
            StartCoroutine(MovePlatform());
        Physics2D.IgnoreLayerCollision(14, 15);
    }

    private IEnumerator MovePlatform()
    {
        int nextPos = index + 1;

        if (nextPos >= destinations.Length)
            nextPos = 0;

        while (elapsedTime < times[index])
        {
            transform.position = Vector3.Lerp(destinations[index], destinations[nextPos], (elapsedTime / times[index]));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;

        yield return new WaitForSeconds(idleTimes[index]);

        index++;
        if (index >= destinations.Length)
            index = 0;

        StartCoroutine(MovePlatform());
    }

    //IMPLEMENT PLAYER STICKING TO MOVING PLATFORM

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && requiresTouch)
        {
            StartCoroutine(MovePlatform());
            requiresTouch = false;
            foreach(GameObject obj in activateOnTouch)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag != "Arrow" && col.gameObject.layer != 14)
        {
            if (col.name == "Knight")
                col.transform.parent.SetParent(transform, true);
            else
                col.transform.SetParent(transform, true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag != "Arrow" && col.gameObject.layer != 14)
        {
            if (col.name == "Knight")
                col.transform.parent.parent = null;
            else
                col.transform.parent = null;
        }
    }
}
