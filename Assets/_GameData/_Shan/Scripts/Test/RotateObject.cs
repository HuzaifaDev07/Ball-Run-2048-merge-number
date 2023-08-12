using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 30f; // Adjust this to change the rotation speed

    // Update is called once per frame
    void Update()
    {
        // Rotate the object along the Z-axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}