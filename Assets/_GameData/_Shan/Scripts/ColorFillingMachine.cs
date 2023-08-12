using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

namespace ArcadeIdle.Shan
{
    public class ColorFillingMachine : MonoBehaviour
    {
        [BoxGroup("Color Pack")]
        [SerializeField] GameObject _colorPackPrefab;
        private GameObject player;

        private AnimateBalls _Animation;
        void Start()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            trigger.OnEnterTrigger.AddListener(OnPlayerEnter);
            _Animation = GetComponent<AnimateBalls>();
        }

        private void OnPlayerEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Player")
            {
                this.player = gameObject;
                PlayerPicker playerPicker = player.GetComponent<PlayerPicker>();

                if (playerPicker.ColorPaclCount <= 0)
                    PickupColorByPlayer();
            }
        }
        private void PickupColorByPlayer()
        {
            StartCoroutine(AddColorToPlayerPack());
        }
        private IEnumerator AddColorToPlayerPack()
        {
            PlayerPicker playerPicker = player.GetComponent<PlayerPicker>();

            yield return new WaitForSeconds(0.25f);
            var Cube = Instantiate(_colorPackPrefab, this.transform).transform;
            _Animation.ParabolicAnimation(Cube, transform, playerPicker.AnimationEndPoint(), () => {

                //playerPicker.ColorPackFill();
            });
            yield return null;
        }
    }
}
