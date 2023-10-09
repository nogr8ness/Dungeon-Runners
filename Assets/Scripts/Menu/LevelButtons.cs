using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
    [SerializeField] private int lvlNum;

    private void Update()
    {
        //If in hard mode, inc level num by 8 to lock levels
        int hardAdd = 0;
        if (PlayerPrefs.GetInt("hardMode") == 1)
            hardAdd = 8;

        if(gameObject.name != "SecretsBtn")
        {
            if (PlayerPrefs.GetInt("lvlsBeaten") < lvlNum - 1 + hardAdd)
                GetComponent<Button>().interactable = false;

            else
                GetComponent<Button>().interactable = true;
        }
        
        else
        {
            if (PlayerPrefs.GetInt("lvlsBeaten") < 16)
                GetComponent<Button>().interactable = false;

            else
                GetComponent<Button>().interactable = true;
        }
        

    }
}
