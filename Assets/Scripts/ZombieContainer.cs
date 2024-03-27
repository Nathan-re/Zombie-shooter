using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float distanceFromCamera;
    private bool isSpawning = false;
    private float nextSpawn = 0f;
    public float minTime = 0.5f;
    public float maxTime = 1.5f;
    public float heightZ = 0.5f;
    public float minSpeed = 0.5f;
    public float maxSpeed = 2f;
    public int currentLevel = 0;
    private int lives;
    private int zombiesLeft;
    private int totalZombiesSpawned;
    private int numberBullets = 0;
    public string tagZombies = "Zombie";

    private UI ui;

    [System.Serializable]
    public class Protocol
    {
        public LevelData[] levels;

        public Protocol(LevelData[] levels)
        {
            this.levels = levels;
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public int numberZombies;
        public int numberLives;
        public float minSpeedZombie;
        public float maxSpeedZombie;
        public float minTimeSpawn;
        public float maxTimeSpawn;
        public int numberBullets;
        public float distanceFromCamera;
    }

    private Protocol protocol;

    Transform thisTransform;

    private IEnumerator Start()
    {
        ui = GameObject.Find("UI").GetComponent<UI>();
        LevelData[] levelData = new LevelData[]
        {
            new LevelData
            {
                numberZombies = 10,
                numberLives = 3,
                minSpeedZombie = 1.5f,
                maxSpeedZombie = 2.5f,
                minTimeSpawn = 1f,
                maxTimeSpawn = 2f,
                numberBullets = (int) (10 * 2),
                distanceFromCamera = 12.5f,
            },
            new LevelData
            {
                numberZombies = 20,
                numberLives = 2,
                minSpeedZombie = 2.5f,
                maxSpeedZombie = 3.5f,
                minTimeSpawn = 0.5f,
                maxTimeSpawn = 1.5f,
                numberBullets = (int) (20 * 1.5),
                distanceFromCamera = 15f,
            },
            new LevelData
            {
                numberZombies = 30,
                numberLives = 3,
                minSpeedZombie = 3f,
                maxSpeedZombie = 4f,
                minTimeSpawn = 0.25f,
                maxTimeSpawn = 1f,
                numberBullets = (int) (30 * 1.5),
                distanceFromCamera = 15f,
            }
        };

        protocol = new Protocol(levelData);

        thisTransform = gameObject.transform;

        for (int i = 0; i < protocol.levels.Length; i++)
        {
            currentLevel = i;
            minTime = protocol.levels[i].minTimeSpawn;
            maxTime = protocol.levels[i].maxTimeSpawn;
            minSpeed = protocol.levels[i].minTimeSpawn;
            maxSpeed = protocol.levels[i].maxSpeedZombie;
            lives = protocol.levels[i].numberLives;
            zombiesLeft = protocol.levels[i].numberZombies;
            totalZombiesSpawned = 0;
            numberBullets = protocol.levels[i].numberBullets;

            ui.maxHealth = lives;
            ui.health = lives;
            ui.maxBullets = numberBullets;
            ui.bullets = numberBullets;
            ui.ennemiesRemaining = zombiesLeft;
            ui.kills = 0;
            ui.waveNo = i;

            while ( totalZombiesSpawned != protocol.levels[i].numberZombies )
            {
                if (lives <= 0)
                {
                    break;
                }
                else if (!isSpawning)
                {
                    yield return new WaitForSeconds(nextSpawn);
                    spawnZombies();
                }
            }

            yield return new WaitUntil(() => (lives <= 0) || (zombiesLeft <= 0));
            if (lives <= 0)
            {
                killAllByTag(tagZombies);
                break;
            }
            yield return new WaitForSeconds(3);
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
        newZ.name = tagZombies;
        newZ.tag = tagZombies;

        newZ.AddComponent<MeshRenderer>();
        newZ.GetComponent<MeshRenderer>().transform.position = positionNewZ;
        newZ.GetComponent<MeshRenderer>().transform.localScale = new Vector3(0.5f, heightZ, 0.5f);
        newZ.AddComponent<MeshCollider>();
        newZ.GetComponent<MeshCollider>().convex = true;
        newZ.AddComponent<SingleZombie>().minSpeedZ = minSpeed;
        newZ.GetComponent<SingleZombie>().maxSpeedZ = maxSpeed;
        newZ.GetComponent<SingleZombie>().DieCallback = () => {
            zombiesLeft--;
            ui.kills++;
            ui.ennemiesRemaining--; 
        };
        newZ.GetComponent<SingleZombie>().RemoveLifeCallback = () => {
            zombiesLeft--;
            lives--;
            ui.health--;
            ui.ennemiesRemaining--;
        };

        //Calcul du nouveau temps
        nextSpawn = Random.Range(minTime, maxTime);

        totalZombiesSpawned++;
        isSpawning = false;
    }

    public void killAllByTag(string tag)
    {
        GameObject[] allZombies = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < allZombies.Length; i++)
        {
            Destroy(allZombies[i]); 
        }
    }
}
