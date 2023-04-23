using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterSetByName : MonoBehaviour
{
    FMOD.Studio.EventInstance Ambience;

    private void Start()
    {
        Ambience = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Ambience");
        Ambience.start();
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PLAYER")
            Ambience.setParameterByName("Ambience Fade", 1f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "PLAYER")
            Ambience.setParameterByName("Ambience Fade", 0f);
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController player))
        {
            Ambience.setParameterByName("Ambience Fade", 1f);
            print("a");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController player))
        {
            Ambience.setParameterByName("Ambience Fade", 0f);
            print("b");
        }
    }

}