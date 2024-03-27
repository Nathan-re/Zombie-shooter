using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleZombie : MonoBehaviour
{
    public float vitessZ;
    public float minSpeedZ;
    public float maxSpeedZ;
    public bool alive;

    public delegate void DelegateDieCallback();
    public DelegateDieCallback DieCallback { get; set; }

    public delegate void DelegateRemoveLifeCallback();
    public DelegateRemoveLifeCallback RemoveLifeCallback { get; set; }

    private Transform transformZ;
    void Start()
    {
        vitessZ = Random.Range(minSpeedZ, maxSpeedZ);
        transformZ = GetComponent<MeshRenderer>().transform;
        alive = true;
    }

    private void Update()
    {
        Vector3 p = Vector3.MoveTowards(transformZ.position, Camera.main.transform.position, vitessZ * Time.deltaTime);
        p.y = transformZ.localScale.y;
        transformZ.position = p;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (alive)
        {
            string currentTag = collider.gameObject.tag;
            switch (currentTag)
            {
                case "MainCamera":
                    Destroy(gameObject);
                    RemoveLifeCallback();
                    alive = false;
                    break;
                case "dealDamage":
                    Destroy(gameObject);
                    Destroy(collider.gameObject);
                    DieCallback();
                    alive = false;
                    break;
                default:
                    Debug.Log("Default");
                    break;
            }
        }

    }
}
