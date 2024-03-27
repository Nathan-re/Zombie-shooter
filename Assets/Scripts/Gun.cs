using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public Transform ammo;
    public Transform trail;
    public InputActionReference actionReference;
    public AudioClip[] gunshotSounds;

    private readonly List<AudioSource> audioSources = new();
    private int currentSource = 0;
    private readonly System.Random random = new();

    private UI ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI").GetComponent<UI>();
        for (int i = 0; i < 2; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.spatialize = false;
            source.spatialBlend = 0.0f;
            source.volume = 1.0f;
            audioSources.Add(source);
        }
        actionReference.action.performed += Shoot;
    }

    /// <summary>
    /// Permet de tirer un projectile
    /// </summary>
    public void Shoot(InputAction.CallbackContext ctx)
    {
        bool canShoot = ui.bullets > 0;
        if (canShoot)
        {
            AudioClip clip = gunshotSounds[random.Next(0, gunshotSounds.Length)];
            audioSources[currentSource].clip = clip;
            audioSources[currentSource].Play();
            currentSource = (currentSource + 1) % audioSources.Count;

            var ammo = Instantiate(this.ammo).gameObject;
            ammo.transform.position = transform.position;
            ammo.transform.Translate(gameObject.transform.up * 0.032f);
            ammo.transform.rotation = transform.rotation;
            ammo.transform.Rotate(90, 0, 0, Space.Self);
            ammo.tag = "dealDamage";
            // Trail
            // Instantiate(trail, ammo.transform);
            Rigidbody rigidbody = ammo.AddComponent<Rigidbody>();
            rigidbody.AddForce(transform.forward * 50, ForceMode.Impulse);
            ui.bullets--;
        }
        else
        {
            // TODO: jouer un son pour dire qu'il n'y a plus de balles
        }
    }
}
