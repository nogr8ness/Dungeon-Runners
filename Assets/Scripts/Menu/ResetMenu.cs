using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetMenu : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private GameObject fade;

    public void ResetProgress()
    {
        //Reset all stats
        PlayerPrefs.SetInt("jumps", 0);
        PlayerPrefs.SetInt("attacks", 0);
        PlayerPrefs.SetFloat("damageTaken", 0f);
        PlayerPrefs.SetInt("deaths", 0);
        PlayerPrefs.SetInt("timePlayedMin", 0);
        PlayerPrefs.SetFloat("timePlayedSec", 0);
        PlayerPrefs.SetInt("lvlsBeaten", 0);
        PlayerPrefs.SetInt("hardMode", 0);

        fade.GetComponent<Image>().enabled = true;
        fadeAnim.SetTrigger("fadeOut");
        StartCoroutine(LoadScene(0.5f, "Main Menu"));
    }

    private IEnumerator LoadScene(float sec, string scene)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene(scene);
    }
}
