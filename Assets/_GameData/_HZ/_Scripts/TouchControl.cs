
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    private float movement;
    public float movementSpeed;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Move the player horizontally based on mouse movement
            movement = Input.GetAxis("Mouse X") * movementSpeed * Time.deltaTime;
            transform.Translate(new Vector3(movement, 0, 0));
        }
    }


}