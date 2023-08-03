using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;

    private void LateUpdate()
    {
        transform.position = playerTransform.position + offset;
    }
}
