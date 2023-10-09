using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omniscent : MonoBehaviour
{
    //Updates time played
    private void Update()
    {
        PlayerPrefs.SetFloat("timePlayedSec", PlayerPrefs.GetFloat("timePlayedSec") + Time.unscaledDeltaTime);

        if (PlayerPrefs.GetFloat("timePlayedSec") >= 60)
        {
            PlayerPrefs.SetFloat("timePlayedSec", 0);
            PlayerPrefs.SetInt("timePlayedMin", PlayerPrefs.GetInt("timePlayedMin") + 1);
        }
    }

    //Deletes all stats on application quit
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
