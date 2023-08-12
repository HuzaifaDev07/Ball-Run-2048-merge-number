using NaughtyAttributes;
using UnityEngine;

namespace ArcadeIdle
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(PlayerAnimations))]
    [AddComponentMenu("Arcade Idle Components/Player Movement", 0)]
    public class PlayerMovement : MonoBehaviour
    {
        private enum MovementType
        {
            MoveAndRotate,
            RotateAndMove,
        }
        
        [BoxGroup("CONTROLS")] [SerializeField] [Required("Drag and drop joystick here.")]
        private VariableJoystick _joystick;
        [BoxGroup("CONTROLS")] [SerializeField] 
        private MovementType _movementType;

        [BoxGroup("MOVEMENT")] [SerializeField] 
        private float _movementSpeed = 10f;
        [BoxGroup("MOVEMENT")] [SerializeField] 
        private float _rotationSpeed = 6f;
        
        private Rigidbody _rigidbody;
        private PlayerAnimations _playerAnimations;

        [Button]
        private void TryFindJoystick()
        {
            _joystick = FindObjectOfType<VariableJoystick>();
        }
        
        private void Start()
        {
            CheckForErrors();
            _rigidbody = GetComponent<Rigidbody>();
            _playerAnimations = GetComponent<PlayerAnimations>();
        }
        
        private void CheckForErrors()
        {
            //if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_joystick == null)
                {
                    Debug.LogError("Player Movement: joystick is null.");
                }
            }
        }
        
        private void FixedUpdate()
        {
            var direction = new Vector3(_joystick.Direction.x, 0f, _joystick.Direction.y); //Convert direction to Vector3

            if (direction.x != 0 || direction.y != 0) //If direction.x or direction.y is 0 means joystick isn't using
            {
                if (_movementType == MovementType.MoveAndRotate)
                {
                    MoveAndRotate(direction, direction.magnitude);
                }
                else
                {
                    RotateAndMove(direction, direction.magnitude);
                }
            }
            else
            {
                _rigidbody.velocity = Vector3.zero; //Sets velocity to zero to avoid inertial motion
            }

            _playerAnimations.UpdateAnimator(direction.magnitude);
        }

        private void MoveAndRotate(Vector3 direction, float speedMultiplier)
        {
            _rigidbody.velocity = direction * _movementSpeed * speedMultiplier;
            _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), _rotationSpeed)); 
        }

        private void RotateAndMove(Vector3 direction, float speedMultiplier)
        {
            _rigidbody.velocity = transform.forward * _movementSpeed * speedMultiplier;
            _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), _rotationSpeed));
        }
    }
}
