using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hz.PlayerMove;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        SuperPower,
        Booster,
        Magnet,
    }
    bool CallOnce = false;

    public PowerUpType _PowerUpType;
    PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = PlayerMovement.instance;
    }
    public void PowerCheck()
    {
        if (!CallOnce)
        {
            CallOnce = true;
            switch (_PowerUpType)
            {
                case PowerUpType.Booster:
                    playerMovement.Booster = true;
                    playerMovement.BoosterState(true);
                    StartCoroutine(EndPowerUp(1f));
                    break;
                case PowerUpType.Magnet:
                    playerMovement.Magnet = true;
                    StartCoroutine(EndPowerUp(5f));
                    break;
                case PowerUpType.SuperPower:
                    playerMovement.SuperPowerState(true);
                    playerMovement.SuperPower = true;
                    playerMovement.Magnet = true;
                    StartCoroutine(EndPowerUp(3f));
                    break;
                default:
                    break;
            }

        }
    }

    IEnumerator EndPowerUp(float Time)
    {
        yield return new WaitForSeconds(Time);
        playerMovement.SuperPowerState(false);
        playerMovement.BoosterState(false);
        CallOnce = false;
        playerMovement.Booster = false;
        playerMovement.Magnet = false;
        playerMovement.SuperPower = false;
    }



}
