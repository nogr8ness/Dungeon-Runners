using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDelay : MonoBehaviour
{
    [SerializeField] private GameObject[] makeActive;
    [SerializeField] private float time;

    private void Update()
    {
        if (Time.timeSinceLevelLoad > time)
        {
            foreach(GameObject obj in makeActive)
            {
                obj.SetActive(true);
            }
        }
    }
}
