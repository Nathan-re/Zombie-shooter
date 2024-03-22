using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hand
{
    LEFT,
    RIGHT
}

public class Weapon : MonoBehaviour
{

    public Hand hand;
    private Transform handInstance;

    private void Start()
    {
        handInstance = controller;
    }

    void Update()
    {
        
    }
}
