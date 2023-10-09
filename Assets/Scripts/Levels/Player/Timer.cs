using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer Params")]
    [SerializeField] private int min;
    [SerializeField] private float sec;

    [Header("Textboxes")]
    [SerializeField] private TextMeshProUGUI timerTxt;

    [Header("Player")]
    [SerializeField] private Health playerHealth;

    public static bool pause;

    private void OnEnable()
    {
        timerTxt.enabled = true;
        timerTxt.color = Color.yellow;
        pause = false;
    }

    private void Update()
    {
        if (PlayerProperties.gamePaused || pause) return;

        if (sec > 9f)
            timerTxt.text = min + ":" + Mathf.Ceil(sec);
        else
            timerTxt.text = min + ":0" + Mathf.Ceil(sec);

        sec -= Time.deltaTime;

        //Death logic
        if(min == 0f && sec <= 0f)
        {
            timerTxt.text = "0:00";
            pause = true;
            playerHealth.invulnerable = false;
            playerHealth.TakeDamage(0);
        }

        //Min-sec logic (e.g. 2:00 -> 1:59)
        if (sec <= -1f)
        {
            min--;
            sec = 59f;
        }
            
    }
}
