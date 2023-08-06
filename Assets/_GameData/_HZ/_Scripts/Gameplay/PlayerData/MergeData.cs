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
    [SerializeField] string ObstacleTag = "Obstacle";
    [SerializeField] float[] IncreaseIndex;
    public int BallIndex;
    [SerializeField] int _2xNumholder = 2;
    [SerializeField] int Ball2xNum;
    [SerializeField] TextMeshPro TextDisplayer;
    [SerializeField] MeshRenderer objectRenderer;
    [SerializeField] MaterialPropertyBlock Mpb;
    [SerializeField] GameObject[] EnjectedBalls;
    [SerializeField] Color[] EnjectedBallsColors;
  

    public Rigidbody MyRgd;
    public TrailRenderer MyTrailRenderer;




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
                //if (_PlayerType == PlayerType.PlayerBall)
                //{
                Debug.Log("CallTween");
                RefMergeData.objectRenderer.enabled = false;
                MyColor = RefMergeData.MyColor;
                objectRenderer.material.color = MyColor;
                StartCoroutine(WaitforScaleAnimation());
                TextDisplayer.text = RefMergeData.Ball2xNum.ToString();
                objectRenderer.GetPropertyBlock(Mpb);
                if (_PlayerType == PlayerType.PlayerBall)
                {
                    MyTrailRenderer.startColor = MyColor;
                }
                _2xNumholder = _2xNumholder * 2;
                Mpb.SetColor("_Color", MyColor);

                objectRenderer.SetPropertyBlock(Mpb);
                BallIndex++;
                //}
                return Mergeable;
            }
            else
            {
                if (RefMergeData != null)
                {
                    RefMergeData.MyRgd.AddForce(Vector3.back * 50f);
                }
                Mergeable = false;
                return Mergeable;
            }
        }
        else
        {
            if (RefMergeData != null)
            {
                RefMergeData.MyRgd.AddForce(Vector3.back * 50f);
            }
            NotMerge = true;
            Mergeable = false;
            return Mergeable;
        }
    }
    IEnumerator WaitforScaleAnimation()
    {
        transform.DOScale(new Vector3(IncreaseIndex[RefMergeData.BallIndex] + .9f,
                                          IncreaseIndex[RefMergeData.BallIndex] + .9f,
                                             IncreaseIndex[RefMergeData.BallIndex] + .9f), .3f);
        yield return new WaitForSeconds(0.2f);
        ChangeScale();
    }
    public void ChangeScale()
    {

        transform.DOScale(new Vector3(IncreaseIndex[RefMergeData.BallIndex] + .1f,
                                          IncreaseIndex[RefMergeData.BallIndex] + .1f,
                                              IncreaseIndex[RefMergeData.BallIndex] + .1f), .2f);
        if (RefMergeData != null)
        {
            Destroy(RefMergeData.gameObject);
        }

        RefMergeData = null;
    }


    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag(MergeBallTag))
        {
            RefMergeData = collision.gameObject.GetComponent<MergeData>();

            IsMergeable(RefMergeData);
        }
        if (collision.gameObject.CompareTag(ObstacleTag))
        {
            if (BallIndex != 0 && _PlayerType == PlayerType.PlayerBall)
            {
                Instantiate(EnjectedBalls[BallIndex - 1], new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
                MyColor = EnjectedBallsColors[BallIndex - 1];
                objectRenderer.material.color = MyColor;
                objectRenderer.GetPropertyBlock(Mpb);
                Mpb.SetColor("_Color", MyColor);
                _2xNumholder = _2xNumholder / 2;
                TextDisplayer.text = _2xNumholder.ToString();
                objectRenderer.SetPropertyBlock(Mpb);
                MyTrailRenderer.startColor = MyColor;

                BallIndex--;
                return;
            }
            else
            {
                Time.timeScale = 0;
            }
        }


    }



}
