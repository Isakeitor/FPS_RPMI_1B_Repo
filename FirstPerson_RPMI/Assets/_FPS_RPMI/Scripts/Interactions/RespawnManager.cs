using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    private Dictionary<RespawnableObject, Coroutine> respawningObjects =
        new Dictionary<RespawnableObject, Coroutine>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterObject(RespawnableObject obj)
    {
        if (!respawningObjects.ContainsKey(obj))
        {
            respawningObjects.Add(obj, null);
        }
    }

    public void Respawn(GameObject objectToRespawn)
    {
        RespawnableObject respawnable =
            objectToRespawn.GetComponent<RespawnableObject>();

        if (respawnable == null)
        {
            Debug.LogWarning(
                "El objeto " + objectToRespawn.name +
                " no tiene RespawnableObject."
            );

            return;
        }

        Respawn(respawnable);
    }

    public void Respawn(RespawnableObject respawnable)
    {
        if (respawningObjects.ContainsKey(respawnable) &&
            respawningObjects[respawnable] != null)
        {
            return;
        }

        Coroutine coroutine = StartCoroutine(
            RespawnRoutine(respawnable)
        );

        if (respawningObjects.ContainsKey(respawnable))
        {
            respawningObjects[respawnable] = coroutine;
        }
        else
        {
            respawningObjects.Add(respawnable, coroutine);
        }
    }

    private IEnumerator RespawnRoutine(RespawnableObject respawnable)
    {
        float respawnTime = respawnable.RespawnTime;

        GameObject obj = respawnable.gameObject;

        obj.SetActive(false);

        yield return new WaitForSeconds(respawnTime);

        if (respawnable == null)
        {
            yield break;
        }

        obj.transform.position = respawnable.OriginalPosition;
        obj.transform.rotation = respawnable.OriginalRotation;
        obj.transform.localScale = respawnable.OriginalScale;

        obj.SetActive(true);

        respawningObjects[respawnable] = null;
    }
}
