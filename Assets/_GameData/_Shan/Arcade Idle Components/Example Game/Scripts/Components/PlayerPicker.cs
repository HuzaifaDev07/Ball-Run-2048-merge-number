using UnityEngine;
using NaughtyAttributes;

namespace ArcadeIdle
{
    public class PlayerPicker : MonoBehaviour
    {
        public SpecialBackpack _specialBackpack;
        public SpecialBackpack _specialBackpackForBalls;

        /*[BoxGroup("Color Pack")] 
        [SerializeField] GameObject _colorPack;*/

        [BoxGroup("Animation Related")]
        [SerializeField] Transform _endPoint;
        [BoxGroup("Capacity According To Level")]
        [SerializeField]
        private int[] _playerCapacity;

        [BoxGroup("Capacity According To Level")]
        [SerializeField]
        CanvasGroup _MaxText;

        private PlayerAnimations _playerAnimations;
        private int colorPaclCount;

        public int ColorPaclCount
        {
            get => colorPaclCount;
            set => colorPaclCount = value;
        }

        private void Start()
        {
            _playerAnimations = GetComponent<PlayerAnimations>();
        }

        public void TryPickUp(GameObject go)
        {
            if (_specialBackpack.IsBackpackFull()) return;
                
            var fruit = go.GetComponent<Fruit>();
            if (_specialBackpack.ShowedItemsType == fruit.resourceType || _specialBackpack.ItemsCount == 0)
            {
                _specialBackpack.AddItems(1, fruit.resourceType);
                //fruit.ParabolicAnimation(_specialBackpack.SpawnPoint());
                _playerAnimations.UpdateAnimator(true);
            }
        }
        
        public void TryPickUpBalls(GameObject go)
        {
            if (_specialBackpackForBalls.IsBackpackFull()) return;
                
            var fruit = go.GetComponent<Fruit>();
            if (_specialBackpackForBalls.ShowedItemsType == fruit.resourceType || _specialBackpackForBalls.ItemsCount == 0)
            {
                _specialBackpackForBalls.AddItems(1, fruit.resourceType);
                //fruit.ParabolicAnimation(_specialBackpack.SpawnPoint());
                _playerAnimations.UpdateAnimator(true);
            }
        }

        public int PlayerItemsCount()
        {
            return _specialBackpack.ItemsCount;
        }
        public int PlayerBallsCount()
        {
            return _specialBackpackForBalls.ItemsCount;
        }
        public int PlayerCapacityCount()
        {
            return 2;
        }
        public void PlayerAnimatorUpdate(bool val)
        {
            _playerAnimations.UpdateAnimator(val);
        }

        public Transform AnimationEndPoint()
        {
            return _endPoint;
        }
        /*public void ColorPackFill()
        {
            colorPaclCount += 1;
            _colorPack.SetActive(true);
        }*/
        /*public void ColorPackEmpty()
        {
            colorPaclCount -= 1;
            _colorPack.SetActive(false);
        }*/
        private void OnUpgradePlayerCapacity(int capacityLevel)
        {
        }

        #region _____Max_Text_Canvas_Group______
        public void ShowMaxText()
        {
            _MaxText.alpha = 1f;
        }
        public void HideMaxText()
        {
            _MaxText.alpha = 0f;
        }
        #endregion
    }
}