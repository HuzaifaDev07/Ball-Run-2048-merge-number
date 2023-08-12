using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeIdle.Shan
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;

        [SerializeField] GameObject _player;
        [SerializeField] GameObject _joystick;
        [SerializeField] PlayerMovement _playerMovement;
        [SerializeField] Animator _playerAnimator;
        [SerializeField] NavmeshPathDraw _navmeshDraw;
        private void Awake()
        {
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            AddGameStartMoney();
        }
        private void AddGameStartMoney()
        {
            bool given = SaveSystem.Instance.Data.GameStartPriceGiven;
            if (!given)
            {
                SaveSystem.Instance.Data.GameStartPriceGiven = true;
                ResourcesSystem.Instance.AddResourceCount(ResourcesSystem.ResourceType.Banknotes, 300);
                SaveSystem.Instance.SaveData();
            }
        }
        public void StopPlayer()
        {
            _playerMovement.enabled = false;
            _playerAnimator.SetFloat("SpeedMultiplier", 0f);
            _joystick.SetActive(false);
        }
        public void StartPlayer()
        {
            _playerMovement.enabled = true;
            _playerAnimator.SetFloat("SpeedMultiplier", 0f);
            _joystick.SetActive(true);
        }
        public void ShowNextTargetNavmesh(Transform target)
        {
            _navmeshDraw.gameObject.SetActive(true);
            _navmeshDraw.destination = target;
            _navmeshDraw.enabled = true;
        }
        public void DisableNavmeshDraw()
        {
            _navmeshDraw.gameObject.SetActive(false);
        }
    }
}
