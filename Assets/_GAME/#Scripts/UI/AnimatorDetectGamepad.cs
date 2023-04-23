using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatorDetectGamepad : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [SerializeField] private int keyboard = 0;
    [SerializeField] private int xbox = 1;
    [SerializeField] private int ps = 2;

    private InputDevice _lastDevice;

    private void Start()
    {
        Disable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += InputSystem_onActionChange;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= InputSystem_onActionChange;
    }

    private void InputSystem_onActionChange(object obj, InputActionChange change)
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
                animator.SetInteger("state", keyboard);
            }

            if (alo.Contains("xbox"))
            {
                animator.SetInteger("state", xbox);
            }

            if (alo.Contains("playstation") || alo.Contains("ps"))
            {
                animator.SetInteger("state", ps);
            }

            _lastDevice = lastDevice;
        }
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
