using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private GameObject fade;

    public void LoadLevels(int lvlNum)
    {
        StartCoroutine(Load(lvlNum));
    }

    public IEnumerator Load(int lvlNum)
    {
        fade.GetComponent<Image>().enabled = true;
        fadeAnim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Level" + lvlNum);
    }
}
