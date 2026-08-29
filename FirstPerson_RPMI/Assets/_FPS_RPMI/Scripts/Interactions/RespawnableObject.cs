using UnityEngine;

public class RespawnableObject : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private float respawnTime = 5f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    public float RespawnTime => respawnTime;
    public Vector3 OriginalPosition => originalPosition;
    public Quaternion OriginalRotation => originalRotation;
    public Vector3 OriginalScale => originalScale;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;

        if (RespawnManager.Instance != null)
        {
            RespawnManager.Instance.RegisterObject(this);
        }
    }
}
