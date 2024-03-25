using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Gun : MonoBehaviour
{
    public Transform ammo;
    public InputActionReference actionReference;
    public AudioClip[] gunshotSounds;

    private List<AudioSource> audioSources;
    private int currentSource = 0;
    private readonly System.Random random = new();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            audioSources.Add(gameObject.AddComponent<AudioSource>());
        }
        audioSources.ForEach(source =>
        {
            source.spatialBlend = 0.0f;
            source.volume = 1.0f;
        });
        actionReference.action.performed += Shoot;
    }

    /// <summary>
    /// Permet de tirer un projectile
    /// </summary>
    public void Shoot(InputAction.CallbackContext ctx)
    {
        var ammo = Instantiate(this.ammo);
        ammo.transform.rotation = gameObject.transform.rotation;
        ammo.SetParent(transform);
        Rigidbody rigidbody = ammo.GetOrAddComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.forward * 460, ForceMode.Impulse);

        AudioClip clip = gunshotSounds[random.Next(0, gunshotSounds.Length)];
        audioSources[currentSource].clip = clip;
        currentSource = (currentSource + 1) % audioSources.Count;
    }
}
