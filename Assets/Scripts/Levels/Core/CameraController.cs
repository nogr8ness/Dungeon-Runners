using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;

    //x-position
    [SerializeField] private Transform player;

    //y-position
    [SerializeField] private float roomBottom;
    [SerializeField] private float roomTop;

    [SerializeField] private RoomBounds[] rooms;

    public static bool freeze;
    private string currentRoom;
    private bool isNewRoom;

    private void Awake()
    {
        transform.position = new Vector3(0, 0, -10);
        freeze = false;
    }

    private void Update()
    {
        /* 
         
         CLAMP CAMERA TO ROOM HEIGHT (bottom and top)
         SET ROOM HEIGHT IN SERIALIZEFIELD; APPLY TO ALL ROOMS (unless otherwise needed)
         
         */

        float yPos = Mathf.Clamp(player.position.y, roomBottom + 5, roomTop - 5);

        if(!freeze)
            transform.position = new Vector3(player.position.x, yPos, transform.position.z);

        foreach(RoomBounds room in rooms)
        {
            if(player.position.x > room.left && player.position.x < room.right && player.position.y > room.bottom && player.position.y < room.top &&
                player.GetComponent<Rigidbody2D>().velocity.y == 0)
            {
                roomBottom = room.bottom;
                roomTop = room.top;

                if (room.name != currentRoom)
                    isNewRoom = true;

                currentRoom = room.name;

                //Respawn all enemies in room if enabled
                if(room.respawnEnemies != null && isNewRoom)
                {
                    foreach(Health enemy in room.respawnEnemies)
                    {
                        enemy.AddHealth(100);
                        enemy.gameObject.SetActive(true);
                        
                        foreach(Behaviour compt in enemy.gameObject.GetComponents<Behaviour>())
                        {
                            compt.enabled = true;
                        }
                        foreach(GameObject deleted in enemy.deleteAfter)
                        {
                            deleted.SetActive(true);
                        }

                        //ENEMY SPECIFIC CODE
                        
                        //Ninja
                        if(enemy.name.Contains("Ninja"))
                        {
                            enemy.GetComponent<Rigidbody2D>().gravityScale = 3;
                        }
                    }
                }

                //1st room that meets requirements is the room player is in
                isNewRoom = false;
                break;
            }
        }
   
        //Kill player for falling offscreen
        if(player.position.y < roomBottom - 8 || player.position.y > roomTop + 8)
        {
            Health playerHealth = player.GetComponent<Health>();
            playerHealth.invulnerable = false;
            playerHealth.TakeDamage(0);
        }
    }
}
