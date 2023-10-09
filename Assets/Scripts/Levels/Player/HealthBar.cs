using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalHealthBar.fillAmount = health.currentHealth / 10;
    }

    private void Update()
    {
        currentHealthBar.fillAmount = health.currentHealth / 10;

        if (currentHealthBar.fillAmount > totalHealthBar.fillAmount)
            totalHealthBar.fillAmount = currentHealthBar.fillAmount;
    }
}
