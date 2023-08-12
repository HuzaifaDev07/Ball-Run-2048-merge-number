using UnityEngine;

namespace ArcadeIdle
{
    [AddComponentMenu("Arcade Idle Components/Follow Camera", 0)]
    public class FollowCamera : MonoBehaviour
    {
        private Transform _camera;

        private void Awake()
        {
            if (Camera.main == null)
            {
                if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
                {
                    Debug.LogError("FollowCamera: can't find camera. Are you sure the camera has a MainCamera tag?");
                }
            }
            else
            {
                _camera = Camera.main.transform;
            }
        }

        private void Update()
        {
            if (_camera == null) return;
            LookAtCamera();
        }

        private void LookAtCamera()
        {
            transform.rotation =  Quaternion.LookRotation(_camera.forward, Vector3.up);
        }
    }
}
