using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FmodPlayer : MonoBehaviour
{
    private float distance = 0.4f;
    private float Material;

    [SerializeField] private bool debug;

    void PlayMeleeEvent(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, GetComponent<Transform>().position);
    }

    void FixedUpdate()
    {
        MaterialCheck();

        if(debug)
            Debug.DrawRay(transform.position, Vector2.down * distance, Color.blue);
    }

    void MaterialCheck()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, Vector2.down, distance, 1 << 3);           //Layer

        if (hit.collider)

        {
            if (hit.collider.tag == "ground")
            {
                Material = 1f;
            }
            if (hit.collider.tag == "Escada")
            {
                Material = 2f;
            }
            

        }
    }

    void PlayFootstepsEvent(string path)
    {
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
        Footsteps.setParameterByName("Material", Material);
        Footsteps.start();
        Footsteps.release();
    }

}