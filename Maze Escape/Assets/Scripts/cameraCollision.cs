using UnityEngine;

public class cameraCollision : MonoBehaviour
{
    public Transform player;
    public LayerMask wallMask;
    public float cameraRadius = 0.2f;
    public float minDistance = 0.05f;

    private Vector3 defaultLocalPos;

    void Start()
    {
        defaultLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        Vector3 origin = player.position;
        Vector3 desiredWorldPos = player.TransformPoint(defaultLocalPos);
        Vector3 direction = desiredWorldPos - origin;
        float distance = direction.magnitude;

        if (Physics.SphereCast(
            origin,
            cameraRadius,
            direction.normalized,
            out RaycastHit hit,
            distance,
            wallMask))
        {
            transform.position = hit.point + hit.normal * minDistance;
        }
        else
        {
            transform.position = desiredWorldPos;
        }
    }
}
