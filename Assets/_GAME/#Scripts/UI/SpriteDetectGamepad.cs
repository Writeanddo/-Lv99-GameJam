using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteDetectGamepad : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite keyboard;
    [SerializeField] private Sprite xbox;
    [SerializeField] private Sprite ps;

    private InputDevice _lastDevice;

    private void Start()
    {
        Disable();
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

                var alo = lastDevice.displayName.ToLower();

                if (alo.Contains("keyboard") || alo.Contains("mouse"))
                {
                    spriteRenderer.sprite = keyboard;
                }

                if (alo.Contains("xbox"))
                {
                    spriteRenderer.sprite = xbox;
                }

                if (alo.Contains("playstation") || alo.Contains("ps"))
                {
                    spriteRenderer.sprite = ps;
                }

                _lastDevice = lastDevice;
            }
        };
    }

    public void Disable()
    {
        spriteRenderer.enabled = false;
    }

    public void Active()
    {
        spriteRenderer.enabled = true;
    }
}
