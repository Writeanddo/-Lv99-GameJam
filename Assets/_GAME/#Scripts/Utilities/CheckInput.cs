using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckInput : MonoBehaviour
{

    public GameObject objectControll;


    private InputDevice _lastDevice;

    private void Start()
    {
        Desactive();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += (obj, change) =>
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;

                if (lastDevice == _lastDevice)
                    return;

                DisableAll();

                var alo = lastDevice.displayName.ToLower();

                if (alo.Contains("keyboard") || alo.Contains("mouse"))
                {
                    objectControll.SetActive(false);
                    print("MOUSE");
                }

                if (alo.Contains("playstation") || alo.Contains("ps") || alo.Contains("xbox"))
                {
                    objectControll.SetActive(true);
                    print("CONTROLE");
                }

                _lastDevice = lastDevice;
            }
        };
    }

    private void DisableAll()
    {
        objectControll.SetActive(false);
    }

    public void Active()
    {
        //canvas.SetActive(true);
    }

    public void Desactive()
    {
        //canvas.SetActive(false);
    }
}
