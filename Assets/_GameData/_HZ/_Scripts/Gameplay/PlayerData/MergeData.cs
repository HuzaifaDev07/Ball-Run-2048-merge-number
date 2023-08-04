using Hz.PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeData : MonoBehaviour
{
    public enum PlayerRankStages
    {
        Rank1,
        Rank2,
        Rank3,
        Rank4,
        Rank5,
    }

    public enum PlayerType
    {
        ShortRange,
        LongRange
    }

    public PlayerRankStages _PlayerRankStages;
    public PlayerType _PlayerType;
    public bool Mergeable;
    [SerializeField] bool CharacterSpawn;
    [SerializeField] Color MyColor;
    public int RankIndex;
    [SerializeField] PlayerController playerController;


    public bool IsMergeable(MergeData playerRankState)
    {
        if (playerRankState != null)
        {
            if (_PlayerRankStages == playerRankState._PlayerRankStages && _PlayerType == playerRankState._PlayerType)
            {
                Mergeable = true;
                return Mergeable;
            }
            else
            {
                Mergeable = false;
                return Mergeable;
            }
        }
        else
        {
            Mergeable = false;
            return Mergeable;
        }
    }

    public void PlayerMerged()
    {
        switch (_PlayerRankStages)
        {
            case PlayerRankStages.Rank1:
                
                break;
            case PlayerRankStages.Rank2:
                break;
            case PlayerRankStages.Rank3:
                break;
            case PlayerRankStages.Rank4:
                break;
            case PlayerRankStages.Rank5:
                break;
            default:
                break;
        }
    }
}
