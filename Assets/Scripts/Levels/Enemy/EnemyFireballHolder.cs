using UnityEngine;

public class EnemyFireballHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    private void Update()
    {
        if(enemy.name != "Wizard")
            transform.localScale = enemy.localScale;
    }
}
