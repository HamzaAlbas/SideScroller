using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime;



    private void FixedUpdate()
    {
        var desPos = target.position + offset;
        var smoothPos = Vector3.Lerp(transform.position, desPos, smoothTime);
        transform.position = smoothPos;
        transform.LookAt(target);
    }
}
