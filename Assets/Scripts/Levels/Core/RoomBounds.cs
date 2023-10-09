using UnityEngine;

public class RoomBounds : MonoBehaviour
{
    [Header("Room Boundaries")]
    [SerializeField] public float left;
    [SerializeField] public float right;

    [SerializeField] public float bottom;
    [SerializeField] public float top;

    [Header("Only use to respawn enemies")]
    [SerializeField] public Health[] respawnEnemies;
}
