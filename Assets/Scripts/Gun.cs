using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public Transform ammo;
    public InputActionReference actionReference;
    public AudioClip[] gunshotSounds;

    private readonly List<AudioSource> audioSources = new();
    private int currentSource = 0;
    private readonly System.Random random = new();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSources.Add(source);
        }
        actionReference.action.performed += Shoot;
    }

    /// <summary>
    /// Permet de tirer un projectile
    /// </summary>
    public void Shoot(InputAction.CallbackContext ctx)
    {
        AudioClip clip = gunshotSounds[random.Next(0, gunshotSounds.Length)];
        audioSources[currentSource].clip = clip;
        audioSources[currentSource].Play();
        currentSource = (currentSource + 1) % audioSources.Count;

        var ammo = Instantiate(this.ammo);
        ammo.AddComponent<MeshCollider>();
        ammo.transform.position = transform.position;
        ammo.Translate(gameObject.transform.up * 0.032f);
        ammo.transform.rotation = transform.rotation;
        ammo.Rotate(90, 0, 0, Space.Self);
        ammo.tag = "dealDamage";
        Rigidbody rigidbody = ammo.GetOrAddComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * 50, ForceMode.Impulse);
    }
}
