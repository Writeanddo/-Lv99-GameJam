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


        if (target == null)
        {
            return false;

        }

        return !(Vector2.Distance(transform.position, target.gameObject.transform.position) > visionRange);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, visionRange);

        var visionDirection = Vector3.right; // GetVisionDirection()

    }

}
