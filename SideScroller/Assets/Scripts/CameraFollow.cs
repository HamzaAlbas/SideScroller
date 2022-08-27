using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target, camTransform;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        offset = camTransform.position - target.position;
    }

    private void LateUpdate()
    {
        var targetPos = target.position + offset;
        camTransform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        transform.LookAt(target);
    }
}
