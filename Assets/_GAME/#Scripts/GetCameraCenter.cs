using UnityEngine;

public class GetCameraCenter : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
    }
}
