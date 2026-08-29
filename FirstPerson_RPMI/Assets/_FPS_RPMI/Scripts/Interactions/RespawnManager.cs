using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("Object To Respawn")]
    [SerializeField] GameObject objectPrefab;

    [Header("Respawn Settings")]
    [SerializeField] float respawnTime = 5f;
    [SerializeField] bool respawnAutomatically = true;

    [Header("Spawn Position")]
    [SerializeField] Transform spawnPoint;

    GameObject currentObject;

    void Start()
    {
        if (respawnAutomatically)
        {
            SpawnObject();
        }
    }

    public void SpawnObject()
    {
        if (currentObject != null)
            return;

        if (objectPrefab == null)
        {
            Debug.LogWarning("RespawnManager: No hay un prefab asignado.");
            return;
        }

        Vector3 spawnPosition;
        Quaternion spawnRotation;

        if (spawnPoint != null)
        {
            spawnPosition = spawnPoint.position;
            spawnRotation = spawnPoint.rotation;
        }
        else
        {
            spawnPosition = transform.position;
            spawnRotation = transform.rotation;
        }

        currentObject = Instantiate(
            objectPrefab,
            spawnPosition,
            spawnRotation
        );

        RespawnableObject respawnable =
            currentObject.GetComponent<RespawnableObject>();

        if (respawnable != null)
        {
            respawnable.SetRespawnManager(this);
        }
    }

    public void ObjectDestroyed()
    {
        currentObject = null;
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);

        SpawnObject();
    }
}