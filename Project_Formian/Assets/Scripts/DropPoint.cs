using System;
using System.Collections;
using UnityEngine;

public class DropPoint : MonoBehaviour
{

    public GameObject dropPointObject; // Reference to the drop point object

    public float spawnInterval = 5f; // Interval between spawns in seconds
    private float spawnTimer; // Timer to track spawn intervals

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = spawnInterval; // Initialize the spawn timer
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerable SpawnObjectsOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
            SpawnObject(); // Call the spawn method to create objects
        }
    }

    private void SpawnObject()
    {
        Instantiate(dropPointObject, transform.position, Quaternion.identity); // Instantiate the drop point object at the current position
    }
}