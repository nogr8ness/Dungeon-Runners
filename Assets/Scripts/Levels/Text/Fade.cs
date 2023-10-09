using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{ 

    public void Disable()
    {
        gameObject.GetComponent<Image>().enabled = false;
    }
}
