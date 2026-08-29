using UnityEngine;

public class RespawnableObject : MonoBehaviour
{
    RespawnManager respawnManager;

    public void SetRespawnManager(RespawnManager manager)
    {
        respawnManager = manager;
    }

    void OnDestroy()
    {
        if (respawnManager != null)
        {
            respawnManager.ObjectDestroyed();
        }
    }
}