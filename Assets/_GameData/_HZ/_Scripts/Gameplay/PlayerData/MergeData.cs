using DG.Tweening;
using Hz.PlayerMove;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        Rank6,
        Rank7,
        Rank8,
        Rank9,
        Rank10,
    }

    public enum PlayerType
    {
        OtherBalls,
        PlayerBall,
    }

    public PlayerRankStages _PlayerRankStages;
    public PlayerType _PlayerType;

    public bool Mergeable;
    [SerializeField] bool CharacterSpawn;
    [SerializeField] Color MyColor;
    public int RankIndex;
    [SerializeField] bool NotMerge;
    [SerializeField] MergeData RefMergeData;
    [SerializeField] PlayerMovement PlayerMovement;
    [SerializeField] string MergeBallTag = "Ball";
    [SerializeField] float[] IncreaseIndex;
    [SerializeField] int BallIndex;
    [SerializeField] int Ball2xNum;
    [SerializeField] TextMeshPro TextDisplayer;
    [SerializeField] MeshRenderer objectRenderer;
    [SerializeField] MaterialPropertyBlock Mpb;
    public Rigidbody MyRgd;

    private void Start()
    {
        Mpb = new MaterialPropertyBlock();
    }
    public bool IsMergeable(MergeData playerRankState)
    {
        if (playerRankState != null)
        {
            if (_PlayerRankStages == playerRankState._PlayerRankStages && RefMergeData.BallIndex == BallIndex)
            {
                Mergeable = true;
                Debug.Log($"{_PlayerType} : PlayerType");
                if (_PlayerType == PlayerType.PlayerBall)
                {
                    Debug.Log("CallTween");

                    PlayerMovement.MyScaleTween.endValueV3 = new Vector3(PlayerMovement.transform.localScale.x + .1f,
                                        PlayerMovement.transform.localScale.y + .1f,
                                        PlayerMovement.transform.localScale.z + .1f);
                    RefMergeData.objectRenderer.enabled = false;
                    MyColor = RefMergeData.MyColor;
                    objectRenderer.material.color = MyColor;

                    TextDisplayer.text = RefMergeData.Ball2xNum.ToString();
                    objectRenderer.GetPropertyBlock(Mpb);

                    Mpb.SetColor("_Color", MyColor);

                    objectRenderer.SetPropertyBlock(Mpb);
                    PlayerMovement.MyScaleTween.DOPlay();
                    BallIndex++;
                }
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
            NotMerge = true;
            Mergeable = false;
            return Mergeable;
        }
    }

    public void ChangeScale()
    {
        PlayerMovement.MyScaleTween.transform.localScale = new Vector3(IncreaseIndex[RefMergeData.BallIndex] + .1f,
                       IncreaseIndex[RefMergeData.BallIndex] + .1f,
                       IncreaseIndex[RefMergeData.BallIndex] + .1f);
        Destroy(RefMergeData.gameObject);
        RefMergeData = null;
    }


    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag(MergeBallTag) && !NotMerge)
        {
            RefMergeData = collision.gameObject.GetComponent<MergeData>();

            IsMergeable(RefMergeData);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag(MergeBallTag) && !NotMerge)
    //    {
    //        RefMergeData = other.GetComponent<MergeData>();
    //        IsMergeable(RefMergeData);
    //    }
    //}


}
