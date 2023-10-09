using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private static Button pauseBtn;

    private void Awake()
    {
        pauseBtn = GetComponent<Button>();
    }

    public static void Disable()
    {
        pauseBtn.enabled = false;
    }
    
    public static void Enable()
    {
        pauseBtn.GetComponent<Button>().enabled = true;
    }
}
