using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jumpTxt;
    [SerializeField] private TextMeshProUGUI attackTxt;
    [SerializeField] private TextMeshProUGUI damageTxt;
    [SerializeField] private TextMeshProUGUI deathTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;

    private void Awake()
    {

        if(PlayerPrefs.GetInt("jumps", -1) == -1)
            PlayerPrefs.SetInt("jumps", 0);

        if (PlayerPrefs.GetInt("attacks", -1) == -1)
            PlayerPrefs.SetInt("attacks", 0);

        if (PlayerPrefs.GetFloat("damageTaken", -1f) == -1)
            PlayerPrefs.SetFloat("damageTaken", 0f);

        if (PlayerPrefs.GetInt("deaths", -1) == -1)
            PlayerPrefs.SetInt("deaths", 0);

        if (PlayerPrefs.GetInt("timePlayedMin", -1) == -1)
            PlayerPrefs.SetInt("timePlayedMin", 0);

        if (PlayerPrefs.GetFloat("timePlayedSec", -1) == -1)
            PlayerPrefs.SetFloat("timePlayedSec", 0);

        if (PlayerPrefs.GetInt("lvlsBeaten", -1) == -1)
            PlayerPrefs.SetInt("lvlsBeaten", 0);

        if (PlayerPrefs.GetInt("hardMode", -1) == -1)
            PlayerPrefs.SetInt("hardMode", 0);
    }

    private void Update()
    {
        jumpTxt.text = PlayerPrefs.GetInt("jumps").ToString();
        attackTxt.text = PlayerPrefs.GetInt("attacks").ToString();
        damageTxt.text = PlayerPrefs.GetFloat("damageTaken").ToString();
        deathTxt.text = PlayerPrefs.GetInt("deaths").ToString();

        if (PlayerPrefs.GetInt("deaths") == 0)
            deathTxt.color = Color.green;
        else
            deathTxt.color = Color.red;

        //Decides whether to print seconds with or without leading 0 (:09 vs :12)
        if (PlayerPrefs.GetFloat("timePlayedSec") < 10)
            timeTxt.text = PlayerPrefs.GetInt("timePlayedMin").ToString() + ":0" + ((int)(PlayerPrefs.GetFloat("timePlayedSec"))).ToString();
        else
            timeTxt.text = PlayerPrefs.GetInt("timePlayedMin").ToString() + ":" + ((int)(PlayerPrefs.GetFloat("timePlayedSec"))).ToString();
    }
}
