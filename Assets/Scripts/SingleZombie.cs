using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleZombie : MonoBehaviour
{

    public float vitessZ;
    public float minSpeedZ;
    public float maxSpeedZ;

    private Transform transformZ;
    void Start()
    {
        vitessZ = Random.Range(minSpeedZ, maxSpeedZ);
        transformZ = GetComponent<MeshRenderer>().transform;
    }

    private void Update()
    {
        Vector3 p = Vector3.MoveTowards(transformZ.position, Camera.main.transform.position, vitessZ * Time.deltaTime);
        p.y = transformZ.localScale.y;
        transformZ.position = p;
    }

    void OnTriggerEnter(Collider collider)
    {
        Destroy(gameObject);
        string currentTag = collider.gameObject.tag;
        switch (currentTag)
        {
            case "MainCamera":
                Destroy(gameObject);
                break;
            case "dealDamage":
                Destroy(gameObject);
                Destroy(collider.gameObject);
                break;
            default:
                Debug.Log("Default");
                break;
        }
    }
}
