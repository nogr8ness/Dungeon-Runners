using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnight : MonoBehaviour
{

	[SerializeField] private float spawnPoint, ySpawn;
	[SerializeField] public GameObject[] activateOnSpawn;
	public Animator anim;

	[Header("Melee Attack Parameters")]
	public BoxCollider2D boxCollider;
	[SerializeField] private float meleeRange;
	[SerializeField] private float colliderDist;
	[SerializeField] private float damage;

	[Header("Player Parameters")]
	[SerializeField] private LayerMask playerLayer;

	[Header("Teleport Parameters")]
	[SerializeField] public SpriteRenderer[] knightParts;

	[Header("Spawn Enemies")]
	[SerializeField] private GameObject[] platforms;
	[SerializeField] private GameObject[] platforms2;
	[SerializeField] private List<GameObject> enemies;
	[SerializeField] private GameObject[] flippedEnemies;
	[SerializeField] private GameObject[] despawn;
	[SerializeField] public List<GameObject> fireballs;
	private int spawn1, spawn2, spawn3;
	private bool triggered1, triggered2, triggered3;

	//References
	public Health playerHealth;
	private Health dkHealth;

	private bool spawned;
	public bool isFlipped = false;

    private void Awake()
    {
        dkHealth = GetComponent<Health>();

		spawn1 = Random.Range(43, 53);
		spawn2 = Random.Range(32, 38);
		spawn3 = Random.Range(5, 18);
    }

    private void Update()
    {
		if (PlayerProperties.position.x > spawnPoint && PlayerProperties.position.y < ySpawn && !spawned)
        {
			anim.SetTrigger("spawn");
			spawned = true;

			foreach(GameObject obj in activateOnSpawn)
            {
				obj.SetActive(true);
            }

			boxCollider.enabled = true;
		}

		//Add upside-down enemies to the mix when in phase 2
		if(anim.GetBool("phase2") == true && flippedEnemies != null)
        {
			foreach(GameObject enemy in flippedEnemies)
            {
				enemies.Add(enemy);
            }

			flippedEnemies = null;
        }

		if(dkHealth.currentHealth <= spawn1 && !triggered1)
        {
			StartCoroutine(SpawnEnemies(3));
			triggered1 = true;
        }
		
		if(dkHealth.currentHealth <= spawn2 && !triggered2)
        {
			StartCoroutine(SpawnEnemies(4));
			triggered2 = true;
        }
		
		if(dkHealth.currentHealth <= spawn3 && !triggered3)
        {
			StartCoroutine(SpawnEnemies(5));
			triggered3 = true;
        }

	}

    public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > PlayerProperties.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < PlayerProperties.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}

	//Melee Attack - Taken from knight script
	public bool PlayerInSight()
	{
		RaycastHit2D hit =
			Physics2D.BoxCast(boxCollider.bounds.center + transform.right * meleeRange * transform.localScale.x * colliderDist,
			new Vector3(boxCollider.bounds.size.x * meleeRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
			0, Vector2.left, 0, playerLayer);

		return hit.collider != null;
	}

	public IEnumerator SpawnEnemies(int numSpawned)
    {
		anim.SetTrigger("teleport");

        foreach(SpriteRenderer part in knightParts)
        {
			part.color = new Color(part.color.r, part.color.g, part.color.b, 0);
        }

		yield return new WaitForSeconds(0.8f);

		anim.speed = 0;

        #region enable platforms
        //If in phase2, enable different set of platforms
        if (anim.GetBool("phase2") == false)
        {
			foreach (GameObject platform in platforms)
			{
				platform.SetActive(true);
				fireballs[0].transform.position = platform.transform.position;
				fireballs[0].SetActive(true);
				fireballs[0].GetComponent<Animator>().SetTrigger("hit");
				fireballs.RemoveAt(0);
			}
		}
		else
        {
			foreach (GameObject platform in platforms2)
			{
				platform.SetActive(true);
				fireballs[0].transform.position = platform.transform.position;
				fireballs[0].SetActive(true);
				fireballs[0].GetComponent<Animator>().SetTrigger("hit");
				fireballs.RemoveAt(0);
			}
		}
        #endregion

        #region disable arrow traps
		foreach(GameObject obj in despawn)
        {
			obj.SetActive(false);
        }
        #endregion

        GameObject[] spawned = new GameObject[numSpawned];

		for(int i = 0; i < numSpawned; i++)
        {
			int index = Random.Range(0, enemies.Count);
			spawned[i] = enemies[index];
            StartCoroutine(EnableComponents(spawned[i]));
			enemies.RemoveAt(index);
        }

		foreach(GameObject enemy in spawned)
        {
			enemy.SetActive(true);
			fireballs[0].transform.position = enemy.transform.position;
			fireballs[0].SetActive(true);
			fireballs[0].GetComponent<Animator>().SetTrigger("hit");
			fireballs.RemoveAt(0);
		}

		yield return new WaitForSeconds(0.5f);

		foreach(GameObject enemy in spawned)
        {
			//enemy.GetComponent<BoxCollider2D>().enabled = true;
        }

		//Wait for enemies to be killed
		yield return new WaitUntil(() => AllEnemiesKilled(spawned));

		foreach (SpriteRenderer part in knightParts)
		{
			part.color = new Color(part.color.r, part.color.g, part.color.b, 1);
		}

		#region disable platforms
		//If in phase2, disable different set of platforms
		if (anim.GetBool("phase2") == false)
		{
			foreach (GameObject platform in platforms)
			{
				platform.SetActive(false);
				fireballs[0].transform.position = platform.transform.position;
				fireballs[0].SetActive(true);
				fireballs[0].GetComponent<Animator>().SetTrigger("hit");
				fireballs.RemoveAt(0);
			}
		}
		else
		{
			foreach (GameObject platform in platforms2)
			{
				platform.SetActive(false);
				fireballs[0].transform.position = platform.transform.position;
				fireballs[0].SetActive(true);
				fireballs[0].GetComponent<Animator>().SetTrigger("hit");
				fireballs.RemoveAt(0);
			}
		}
        #endregion

        #region enable arrow traps
        if (anim.GetBool("phase2") == true)
        {
			foreach (GameObject obj in despawn)
			{
				obj.SetActive(true);
			}
		}
		#endregion

		anim.speed = 1;
	}

	private IEnumerator EnableComponents(GameObject enemy)
    {
		yield return new WaitForSeconds(1);

        #region Knight
        if (enemy.GetComponent<ComponentList>().enemyType == "knight")
        {
			Transform child = enemy.transform.GetChild(2);

			enemy.GetComponent<EnemyPatrol>().enabled = true;
			child.GetComponent<Animator>().enabled = true;
			child.GetComponent<BoxCollider2D>().enabled = true;
			child.GetComponent<MeleeEnemy>().enabled = true;
        }
        #endregion

        #region Mage
		if (enemy.GetComponent<ComponentList>().enemyType == "mage")
        {
			Transform mage = enemy.transform.GetChild(1);

			mage.GetComponent<Animator>().enabled = true;
			mage.GetComponent<BoxCollider2D>().enabled = true;
			mage.GetComponent<RangedEnemy>().enabled = true;
        }
        #endregion

        #region Ninja
		if (enemy.GetComponent<ComponentList>().enemyType == "ninja")
        {
			foreach(BoxCollider2D collider in enemy.GetComponents<BoxCollider2D>())
            {
				collider.enabled = true;
            }
			enemy.GetComponent<Animator>().enabled = true;
			enemy.GetComponent<Ninja>().enabled = true;
			enemy.GetComponent<Rigidbody2D>().gravityScale = 3 * Mathf.Sign(enemy.transform.localScale.y);
        }
		#endregion
	}

    private bool AllEnemiesKilled(GameObject[] enemies)
    {
		bool allKilled = true;

		foreach(GameObject enemy in enemies)
        {
			if(enemy.activeInHierarchy)
				allKilled = false;
        }

		return allKilled;
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * meleeRange * transform.localScale.x * colliderDist,
			new Vector3(boxCollider.bounds.size.x * meleeRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
	}
}
