using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float distanceFromCamera = 15f;
    private bool isRunning = true;
    private bool isSpawning = false;
    private float nextSpawn = 0f;
    public float minTime = 0.5f;
    public float maxTime = 1.5f;
    public float heightZ = 0.5f;
    public float minSpeed = 0.5f;
    public float maxSpeed = 2f;

    Transform thisTransform;

    private IEnumerator Start()
    {
        thisTransform = gameObject.transform;
        while (isRunning && !isSpawning)
        {
            yield return new WaitForSeconds(nextSpawn);
            spawnZombies();
        }
    }

    void Update()
    {
        Vector3 p = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
        p.y = heightZ;
        thisTransform.position = p;
        thisTransform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);
    }

    private void spawnZombies()
    {
        isSpawning = true;

        Vector3 positionNewZ = thisTransform.position;
        positionNewZ.x = (thisTransform.position.x - (thisTransform.localScale.x / 2)) + (thisTransform.localScale.x * Random.Range(0.1f, 0.99f));
        positionNewZ.z = (thisTransform.position.z - (thisTransform.localScale.z / 2)) + (thisTransform.localScale.z * Random.Range(0.1f, 0.99f));

        GameObject newZ = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        newZ.name = "Zombie";
        newZ.AddComponent<MeshRenderer>();
        newZ.GetComponent<MeshRenderer>().transform.position = positionNewZ;
        newZ.GetComponent<MeshRenderer>().transform.localScale = new Vector3(0.5f, heightZ, 0.5f);
        newZ.AddComponent<MeshCollider>();
        newZ.GetComponent<MeshCollider>().convex = true;
        newZ.AddComponent<SingleZombie>().minSpeedZ = minSpeed;
        newZ.GetComponent<SingleZombie>().maxSpeedZ = maxSpeed;

        //Calcul du nouveau temps
        nextSpawn = Random.Range(minTime, maxTime);

        isSpawning = false;
    }
}
