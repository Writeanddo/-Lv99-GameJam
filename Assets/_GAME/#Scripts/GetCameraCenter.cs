using UnityEngine;

public class GetCameraCenter : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
    }
}
