using UnityEngine;

public class IAVisionCircle : MonoBehaviour
{
  
    //[Range(15f, 200.0f)]
    public float visionRange ;

    [Header("RaycastHit")]
    public LayerMask hitMask;
    public Transform hitBox;


    public bool IsVisible(Collider2D target)
    {

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Combat", 1f);
        if (target == null)
        {
            return false;

        }

        return !(Vector2.Distance(transform.position, target.gameObject.transform.position) > visionRange);
        

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        var visionDirection = Vector3.right; // GetVisionDirection()

    }

}
