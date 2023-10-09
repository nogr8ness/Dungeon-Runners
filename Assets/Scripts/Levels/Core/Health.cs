using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;

    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iframes")]
    [SerializeField] private float iframesDuration;
    [SerializeField] private int numFlashes;

    private SpriteRenderer rend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    [SerializeField] public GameObject[] deleteAfter;

    [SerializeField] private GameObject deathMenu;

    public bool invulnerable;

    private void Awake()
    {
        //Hard Mode
        if (gameObject.tag == "Player" && PlayerPrefs.GetInt("hardMode") == 1)
            startingHealth = 1;

        currentHealth = startingHealth;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (currentHealth > 0)
            dead = false;
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable || (EffectPresent("Cookie") && _damage != 0))
            return;

        //Shield effect
        foreach(GameObject effect in PlayerProperties.effects)
        {
            if (effect != null && effect.name == "Shield" && gameObject.name == "Player")
                 _damage /= 2;
        }

        //If damage is 0, kill player (for falling off map and timer deaths)
        if (_damage == 0)
            _damage = currentHealth;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (iframesDuration > 0)
            PlayerPrefs.SetFloat("damageTaken", PlayerPrefs.GetFloat("damageTaken") + _damage);

        //Check for death
        if (currentHealth > 0)
        {
            if(!gameObject.name.Contains("Boss"))
                anim.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                if (gameObject.name != "Boss")
                    anim.SetTrigger("die");
                else
                    transform.GetChild(0).GetComponent<Animator>().SetTrigger("die");

                if(gameObject.name == "Player")
                    PauseButton.Disable();

                //Deactivate all component classes
                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                }

                dead = true;

                if(iframesDuration > 0)
                {
                    PlayerPrefs.SetInt("deaths", PlayerPrefs.GetInt("deaths") + 1);
                }
            }
        } 
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void AddExtraHearts(float _value)
    {
        startingHealth += _value;

        //Get to full health
        currentHealth += (startingHealth - currentHealth);
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(8, 10, true);

        for (int i = 0; i < numFlashes; i++)
        {
            rend.color = new Color(1, 0, 0, 0.25f);
            yield return new WaitForSeconds(iframesDuration / (numFlashes * 2));
            rend.color = Color.white;
            yield return new WaitForSeconds(iframesDuration / (numFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(8, 10, false);
        invulnerable = false;
    }

    private bool EffectPresent(string effect)
    {
        bool present = false;

        foreach (GameObject _effect in PlayerProperties.effects)
        {
            if (_effect != null)
            {
                if (_effect.name == effect)
                    present = true;
            }
        }

        return present;
    }

    private IEnumerator Deactivate()
    {
        if(gameObject.tag != "Player")
            yield return new WaitForSeconds(1);

        if (gameObject.tag == "Player")
            DisplayDeathMenu();

        gameObject.SetActive(false);

        foreach(GameObject obj in deleteAfter)
        {
            obj.SetActive(false);
        }

        yield return null;
    }

    private void DisplayDeathMenu()
    {
        Time.timeScale = 0;
        deathMenu.SetActive(true);
    }
}
