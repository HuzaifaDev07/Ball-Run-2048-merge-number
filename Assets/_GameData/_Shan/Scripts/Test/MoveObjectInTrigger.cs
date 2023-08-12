using UnityEngine;

public class MoveObjectInTrigger : MonoBehaviour
{
    public bool up;
    public float movementSpeed = 5f;
    Rigidbody rb;
    DirectionToMove direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Direction(DirectionToMove dir)
    {
        direction = dir;
    }
    void Update()
    {
        // Check if the object is inside the trigger area
        if (IsInTrigger())
        {
            // Move the object in the desired direction
            MoveObject();
        }
    }

    void MoveObject()
    {
        // Move the object in the forward direction (you can modify this based on your needs)
        //transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

        Vector3 movement = Vector3.forward;

        // Calculate the movement vector
        if (direction == DirectionToMove.Up)
            movement = Vector3.up * movementSpeed;
        else if (direction == DirectionToMove.Right)
            movement = Vector3.right * movementSpeed;
        else if (direction == DirectionToMove.Left)
            movement = Vector3.left * movementSpeed;
        else if (direction == DirectionToMove.Down)
            movement = Vector3.down * movementSpeed;
        else if (direction == DirectionToMove.Forward)
            movement = Vector3.forward * movementSpeed;
        else if (direction == DirectionToMove.Back)
            movement = Vector3.back * movementSpeed;
        else if (direction == DirectionToMove.RightUp)
        {
            movement = (Vector3.right + Vector3.up) * movementSpeed;
        }
        else if (direction == DirectionToMove.RightDown)
        {
            movement = (Vector3.right + Vector3.down) * movementSpeed;
        }

        // Apply velocity to the Rigidbody
        rb.velocity = movement;
    }

    bool IsInTrigger()
    {
        // Check if the object is currently inside any trigger colliders
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f, Quaternion.identity, LayerMask.GetMask("Default"));
        return colliders.Length > 0;
    }
}
