using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToggleHardMode : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private GameObject fade;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Image background;
    [SerializeField] private GameObject normalBtn;
    [SerializeField] private GameObject hardBtn;

    public void HardMode(int _hard)
    {
        PlayerPrefs.SetInt("hardMode", _hard);

        if (_hard == 1)
            StartCoroutine(LoadHardMenu());

        else
            StartCoroutine(LoadNormalMenu());
    }

    public IEnumerator LoadHardMenu()
    {
        fade.GetComponent<Image>().enabled = true;
        fadeAnim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Main Menu");

        fadeAnim.SetTrigger("fadeIn");
        gameObject.SetActive(false);
    }
    
    public IEnumerator LoadNormalMenu()
    {
        fade.GetComponent<Image>().enabled = true;
        fadeAnim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Main Menu");

        fadeAnim.SetTrigger("fadeIn");
        gameObject.SetActive(false);
    }
}
