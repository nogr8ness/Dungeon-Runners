using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;

    private float lifeTime;

    public void ActivateProjectile()
    {
        lifeTime = 0;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        if (lifeTime >= resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 13)
            return;

        base.OnTriggerStay2D(collision);
        gameObject.SetActive(false);
    }
}
