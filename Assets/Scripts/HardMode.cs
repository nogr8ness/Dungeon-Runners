using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HardMode : MonoBehaviour
{
    [SerializeField] private GameObject[] enableOnHardMode;
    [SerializeField] private GameObject[] disableOnHardMode;

    [Header("Only In Menu")]
    [SerializeField] private Image background;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("hardMode") == 1)
        {
            foreach (GameObject obj in enableOnHardMode)
                obj.SetActive(true);
            
            foreach (GameObject obj in disableOnHardMode)
                obj.SetActive(false);

            if(background != null)
                background.color = new Color(1, 0.75f, 0.75f);
        }
    }
}
