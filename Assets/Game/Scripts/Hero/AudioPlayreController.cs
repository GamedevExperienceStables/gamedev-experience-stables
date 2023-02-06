using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Game.Actors;

public class AudioPlayreController : MonoBehaviour
{
    public FMODUnity.EventReference inputSound;
    bool plaeySound = false;
    float walkingSpeed = 0.55f;
    void Update()
    {

        if (MovementController.flagRun==true)
        {
            // Debug.Log(1);
            plaeySound = true;

        }
        else if (MovementController.flagRun==false)
        {
            //Debug.Log(2);
            plaeySound = false;
        }
    }
    void CallFootsteps()
    {
        if (plaeySound == true)
        {

            FMODUnity.RuntimeManager.PlayOneShot(inputSound);
        }

    }
    void Start()
    {
        InvokeRepeating("CallFootsteps", 0, walkingSpeed);
    }
    void OnDisable()
    {
        plaeySound = false;
    }
}
