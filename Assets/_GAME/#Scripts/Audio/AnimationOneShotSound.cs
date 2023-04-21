using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AnimationOneShotSound : MonoBehaviour
{

    void PlayEvent(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, GetComponent<Transform>().position);
    }
}