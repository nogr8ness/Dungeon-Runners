using System.Collections;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    public IEnumerator ChangeDoorState(GameObject door, Vector3 endingPos, float time)
    {
        float elapsedTime = 0;
        Vector3 currentPos = transform.position;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(currentPos, endingPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endingPos;
        yield return null;
    }
}
