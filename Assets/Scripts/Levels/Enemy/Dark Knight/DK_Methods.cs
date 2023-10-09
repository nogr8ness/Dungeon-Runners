using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DK_Methods : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Phase 2")]
    [SerializeField] private GameObject[] deactivatePhase2;
    [SerializeField] private GameObject[] activatePhase2;
    [SerializeField] private GameObject graviSwitcher;

    [Header("Teleport Y Positions")]
    [SerializeField] private float normalYPos;
    [SerializeField] private float flippedYPos;

    private DarkKnight knight;
    private DK_Run runScript;
    private Transform parent;

    private void Awake()
    {
        knight = GetComponentInParent<DarkKnight>();
        runScript = knight.anim.GetBehaviour<DK_Run>();
        parent = transform.parent;
    }

    public void DamagePlayer()
    {
        if (knight.PlayerInSight())
            knight.playerHealth.TakeDamage(1);
    }

    public void Teleport()
    {
        float xPos = Mathf.Clamp(Random.Range(player.position.x - 4, player.position.x + 4), 
            runScript.leftEdge, runScript.rightEdge);

        float yPos = transform.position.y;

        //If player is flipped in phase 2, flip enemy as well
        if(knight.anim.GetBool("phase2") == true && 
            Mathf.Sign(player.localScale.y) != Mathf.Sign(parent.localScale.y))
        {
            if (player.localScale.y > 0) //downward gravity
                yPos = normalYPos;
            else                         //upward gravity
                yPos = flippedYPos;

            parent.localScale = new Vector3(parent.localScale.x, -parent.localScale.y, parent.localScale.z);
        }

        parent.position = new Vector3(xPos, yPos, transform.position.z);
    }

    public void TriggerRun()
    {
        knight.anim.SetTrigger("run");
    }

    public void StartPhase2()
    {
        knight.GetComponent<Health>().invulnerable = true;
        knight.GetComponent<EnemyHealthBar>().currentHealthBar.color = new Color(0.5f, 0.5f, 0);

        foreach(GameObject obj in deactivatePhase2)
        {
            obj.SetActive(false);
            knight.fireballs[0].transform.position = obj.transform.position;
            knight.fireballs[0].SetActive(true);
            knight.fireballs[0].GetComponent<Animator>().SetTrigger("hit");
            knight.fireballs.RemoveAt(0);
        }

        
        foreach (GameObject obj in activatePhase2)
        {
            obj.SetActive(true);
            knight.fireballs[0].transform.position = obj.transform.position;
            knight.fireballs[0].SetActive(true);
            knight.fireballs[0].GetComponent<Animator>().SetTrigger("hit");
            knight.fireballs.RemoveAt(0);
        }
        graviSwitcher.transform.position = new Vector3(PlayerProperties.position.x,
            PlayerProperties.position.y + 3, PlayerProperties.position.z);
    }
    
    public void Phase2()
    {
        knight.GetComponent<Health>().invulnerable = false;
        knight.GetComponent<EnemyHealthBar>().currentHealthBar.color = Color.red;

        //Needed so that box collider works properly
        transform.localScale = new Vector3(transform.localScale.x * .75f, transform.localScale.y * .75f, transform.localScale.z);
        parent.localScale = new Vector3(1, 1, parent.localScale.z);

        parent.Translate(new Vector3(0, .2f, 0));
    }

    public void DisableCollider()
    {
        knight.boxCollider.enabled = false;
    }

    public void Deactivate()
    {
        knight.gameObject.SetActive(false);

        foreach(GameObject obj in knight.activateOnSpawn)
        {
            obj.SetActive(false);
        }
    }
}
