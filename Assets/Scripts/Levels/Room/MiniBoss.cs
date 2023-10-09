using UnityEngine;

public class MiniBoss : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject[] doors;

    private bool hasBeenTriggered;

    private void Awake()
    {
        hasBeenTriggered = false;
    }

    private void Update()
    {
        if(!CheckForEnemies() && !hasBeenTriggered)
        {
            foreach(GameObject door in doors)
            {
                Vector3 doorPos1 = new Vector3(door.transform.position.x, door.transform.position.y + 2.69f, door.transform.position.z);
                Vector3 doorPos2 = new Vector3(door.transform.position.x, door.transform.position.y + 2.69f, door.transform.position.z);

                StartCoroutine(door.GetComponent<DoorTrigger>().ChangeDoorState(door.gameObject, doorPos1, 0.25f));
                StartCoroutine(door.GetComponent<DoorTrigger>().ChangeDoorState(door.gameObject, doorPos2, 0.25f));

                hasBeenTriggered = true;
            }
            
        }
    }

    private bool CheckForEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].activeInHierarchy)
                return true;
        }
        return false;
    }
}
