using UnityEngine;

public class TriggerMiniBoss : MonoBehaviour
{
    private bool hasBeenTriggered;
    
    [Header("Doors")]    
    [SerializeField] private GameObject[] doors;

    private void Update()
    {
        if (PlayerProperties.position.x >= transform.position.x && Mathf.Abs(PlayerProperties.position.y - transform.position.y) <= 4 && 
            hasBeenTriggered == false && Time.timeSinceLevelLoad > 0.5f)
        {
            foreach(GameObject door in doors)
            {
                Vector3 doorPos1 = new Vector3(door.transform.position.x, door.transform.position.y - 2.69f, door.transform.position.z);
                Vector3 doorPos2 = new Vector3(door.transform.position.x, door.transform.position.y - 2.69f, door.transform.position.z);

                StartCoroutine(door.GetComponent<DoorTrigger>().ChangeDoorState(door.gameObject, doorPos1, 0.25f));
                StartCoroutine(door.GetComponent<DoorTrigger>().ChangeDoorState(door.gameObject, doorPos2, 0.25f));

                hasBeenTriggered = true;
            }
            
        }
    }
}
