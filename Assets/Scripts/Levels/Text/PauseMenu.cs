using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject completeUI;

    [Header("Fade")]
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private GameObject fade;

    public void Pause(string type)
    {
        Time.timeScale = 0f;

        if (type == "pause")
            pauseUI.SetActive(true);

        if (type == "death")
            deathUI.SetActive(true);

        if (type == "complete")
            completeUI.SetActive(true);

        PlayerProperties.gamePaused = true;
        
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        deathUI.SetActive(false);
        completeUI.SetActive(false);
        PlayerProperties.gamePaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        fade.GetComponent<Image>().enabled = true;
        fadeAnim.SetTrigger("fadeOut");
        StartCoroutine(LoadScene(0.5f, SceneManager.GetActiveScene().name));
        PlayerProperties.gamePaused = false;
    }

    public void LoadNextLevel(int level)
    {
        Time.timeScale = 1f;
        fade.GetComponent<Image>().enabled = true;
        fadeAnim.SetTrigger("fadeOut");
        StartCoroutine(LoadScene(0.5f, "Level" + level));
        PlayerProperties.gamePaused = false;
    }

    public void Levels()
    {
        Time.timeScale = 1f;
        fade.GetComponent<Image>().enabled = true;
        fadeAnim.SetTrigger("fadeOut");
        StartCoroutine(LoadScene(0.5f, "Main Menu"));
        PlayerProperties.gamePaused = false;
    }

    private IEnumerator LoadScene(float sec, string scene)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene(scene);
    }
}
