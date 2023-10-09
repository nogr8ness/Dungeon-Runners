using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image totalHealthBar;
    public Image currentHealthBar;

    private Health health;
    private float totalHealth;

    private void Awake()
    {
        health = GetComponent<Health>();

        //Initializes camera in HealthBarCanvas
        totalHealthBar.GetComponentInParent<Canvas>().worldCamera = Camera.main;
    }

    private void Start()
    {
        totalHealth = health.currentHealth;

        totalHealthBar.fillAmount = 1;
    }

    private void Update()
    {
        if(gameObject.name != "Boss")
        {
            totalHealthBar.transform.position = new Vector3(transform.position.x,
            transform.position.y + 0.5f * Mathf.Sign(transform.localScale.y), transform.position.z);
        }
        

        if (gameObject.name.Contains("Wizard"))
        {
            totalHealthBar.transform.position = new Vector3(transform.position.x - 0.3f * gameObject.transform.localScale.x, 
                transform.position.y + 0.5f * Mathf.Sign(transform.localScale.y), transform.position.z);
        }

        if (gameObject.name.Contains("Ninja"))
        {
            totalHealthBar.transform.position = new Vector3(transform.position.x,
                transform.position.y + 1f * Mathf.Sign(transform.localScale.y), transform.position.z);
        }

        currentHealthBar.transform.position = totalHealthBar.transform.position;
        currentHealthBar.fillAmount = health.currentHealth / totalHealth;
    }
}
