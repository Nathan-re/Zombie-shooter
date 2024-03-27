using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleZombie : MonoBehaviour
{
    public float vitessZ;
    public float minSpeedZ;
    public float maxSpeedZ;
    public bool alive;
    public bool hasSound;

    public List<AudioClip> zombiesSounds { get; set; }
    public AudioClip zombieDeath { get; set; }

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

        if (zombiesSounds.Count != 0)
        {
            AudioClip clip = zombiesSounds[(int)(Mathf.Round(Random.Range(0, zombiesSounds.Count - 1)))];
            gameObject.AddComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().spatialBlend = 1;
            GetComponent<AudioSource>().maxDistance = 30;
            hasSound = true;
        }
        else
        {
            hasSound = false;
        }
        
    }

    private void Update()
    {
        Vector3 p = Vector3.MoveTowards(transformZ.position, Camera.main.transform.position, vitessZ * Time.deltaTime);
        p.y = transformZ.localScale.y;
        transformZ.position = p;
        if (hasSound)
        {
            soundAlive();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (alive)
        {
            string currentTag = collider.gameObject.tag;
            switch (currentTag)
            {
                case "MainCamera":
                    alive = false;
                    RemoveLifeCallback();
                    StartCoroutine(die(false));
                    break;
                case "dealDamage":
                    alive = false;
                    DieCallback();
                    StartCoroutine(die(true));
                    Destroy(collider.gameObject);
                    break;
                default:
                    break;
            }
        }

    }

    private IEnumerator die(bool sound)
    {
        if (zombieDeath != null && sound && hasSound)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<Renderer>().enabled = false;
            GetComponent<AudioSource>().clip = zombieDeath;
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }



    }

    private void soundAlive()
    {
        if (hasSound && !GetComponent<AudioSource>().isPlaying && alive)
        {
            AudioClip clip = zombiesSounds[(int)(Mathf.Round(Random.Range(0, zombiesSounds.Count - 1)))];
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
        }
    }
}
