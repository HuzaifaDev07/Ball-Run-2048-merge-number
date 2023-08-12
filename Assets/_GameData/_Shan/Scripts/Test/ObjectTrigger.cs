using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] DirectionToMove direction;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the moving object
        if (other.CompareTag("Ball"))
        {
            // Start moving the object
            MoveObjectInTrigger moveScript = other.GetComponent<MoveObjectInTrigger>();
            if (moveScript != null)
            {
                if (direction == DirectionToMove.Up)
                    moveScript.up = true;
                else if(direction == DirectionToMove.Down)
                    moveScript.up = false;

                moveScript.Direction(direction);
                moveScript.enabled = true;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the moving object
        if (other.CompareTag("Ball"))
        {
            // Stop moving the object
            MoveObjectInTrigger moveScript = other.GetComponent<MoveObjectInTrigger>();
            if (moveScript != null)
            {
                moveScript.enabled = false;
            }
        }
    }
}
public enum DirectionToMove { Up, Down, Forward,Back, Right, Left, RightForward, RightUp, RightDown, }
