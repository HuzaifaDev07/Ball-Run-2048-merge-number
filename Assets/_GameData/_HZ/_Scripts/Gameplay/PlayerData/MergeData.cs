using DG.Tweening;
using Hz.PlayerMove;
using Hz.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MergeData : MonoBehaviour
{
    private void OnValidate()
    {
        CheckRigidBody();
    }

    public void CheckRigidBody()
    {
        if (MyRgd == null)
        {
            Rigidbody rbd = this.gameObject.AddComponent<Rigidbody>();
            rbd.mass = 0.1f;
            MyRgd = rbd;
        }
        else
        {
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            MyRgd = rb;
        }

        if (this.gameObject.GetComponent<SphereCollider>() == null)
        {
            this.gameObject.AddComponent<SphereCollider>();
        }
    }
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
    [SerializeField] AudioSource MyAudioSource;
    [SerializeField] AudioClip NotMergeSound;
    [SerializeField] AudioClip MergeSound;
    [SerializeField] MeshFilter MeshFilter;
    [SerializeField] MaterialPropertyBlock Mpb;
    [SerializeField] GameObject[] EnjectedBalls;
    [SerializeField] Mesh[] BallMeshes;
    [SerializeField] Color[] EnjectedBallsColors;


    public Rigidbody MyRgd;
    public TrailRenderer MyTrailRenderer;

    public void IsMergeable(MergeData playerRankState)
    {
        Debug.Log($"{playerRankState.gameObject.name} : Player_Name");
        Debug.Log($"{playerRankState.BallIndex} : Player");
        if (playerRankState != null)
        {
            if (BallIndex == RefMergeData.BallIndex)
            {
                Mergeable = true;
                BallIndex++;
                Debug.Log($"{_PlayerType} : PlayerType");
                MyAudioSource.clip = MergeSound;
                MyAudioSource.Play();
                Debug.Log("CallTween");
                MyColor = RefMergeData.MyColor;
                StartCoroutine(WaitforScaleAnimation());
                if (_PlayerType == PlayerType.PlayerBall)
                {
                    MyTrailRenderer.startColor = MyColor;
                }
                _PlayerRankStages = playerRankState._PlayerRankStages;
                if (BallIndex < BallMeshes.Length)
                {
                    MeshFilter.gameObject.transform.position = new Vector3(MeshFilter.gameObject.transform.position.x, MeshFilter.gameObject.transform.position.y + 0.1f, MeshFilter.gameObject.transform.position.z);
                    MeshFilter.mesh = BallMeshes[BallIndex];
                }
                return;
            }
            else
            {
                if (RefMergeData != null)
                {
                    RefMergeData.MyRgd.AddForce(Vector3.back * 50f);
                }
                Mergeable = false;
                MyAudioSource.clip = NotMergeSound;
                MyAudioSource.Play();
                return;
            }
        }
        else
        {
            if (RefMergeData != null)
            {
                RefMergeData.MyRgd.AddForce(Vector3.back * 50f);
            }
            MyAudioSource.clip = NotMergeSound;
            MyAudioSource.Play();
            NotMerge = true;
            Mergeable = false;
            return;
        }
    }
    IEnumerator WaitforScaleAnimation()
    {
        transform.DOScale(new Vector3(IncreaseIndex[RefMergeData.BallIndex] + .4f,
                                          IncreaseIndex[RefMergeData.BallIndex] + .4f,
                                             IncreaseIndex[RefMergeData.BallIndex] + .4f), .3f);
        yield return new WaitForSeconds(0.1f);
        ChangeScale();
    }
    IEnumerator ReduceScaleAnimation()
    {
        if (RefMergeData != null)
        {
            transform.DOScale(new Vector3(IncreaseIndex[RefMergeData.BallIndex] - .8f,
                                              IncreaseIndex[RefMergeData.BallIndex] - .8f,
                                                 IncreaseIndex[RefMergeData.BallIndex] - .8f), .1f);

        }
        yield return new WaitForSeconds(0.2f);
        ReduceScale();
    }
    public void ReduceScale()
    {
        transform.DOScale(Vector3.one, .1f);
    }
    public void ChangeScale()
    {

        transform.DOScale(Vector3.one, .2f);
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
    }
    bool ObstacleTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ObstacleTag))
        {
            if (BallIndex != 0)
            {
                switch (_PlayerType)
                {
                    case PlayerType.PlayerBall:
                        //  ObstacleTriggered = true;
                        other.GetComponent<Collider>().enabled = false;
                        MyAudioSource.clip = NotMergeSound;
                        MyAudioSource.Play();
                        Debug.Log($"{other.name}");
                        Instantiate(EnjectedBalls[BallIndex - 1], new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
                        MyColor = EnjectedBallsColors[BallIndex - 1];
                        MyTrailRenderer.startColor = MyColor;
                        MeshFilter.gameObject.transform.position = new Vector3(MeshFilter.gameObject.transform.position.x, MeshFilter.gameObject.transform.position.y - 0.1f, MeshFilter.gameObject.transform.position.z);
                        BallIndex--;
                        MeshFilter.mesh = BallMeshes[BallIndex];
                        StartCoroutine(ReduceScaleAnimation());
                        break;
                    default:
                        break;
                }

                return;
            }
            else if (BallIndex == 0 && _PlayerType == PlayerType.PlayerBall)
            {
                Time.timeScale = 0;
            }
            else if (_PlayerType == PlayerType.OtherBalls)
            {
                MyRgd.AddForce(Vector3.up * 1075f);
            }
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag(ObstacleTag))
    //    {
    //        switch (_PlayerType)
    //        {
    //            case PlayerType.PlayerBall:
    //                ObstacleTriggered = true;
    //                break;
    //        }
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag(ObstacleTag))
    //    {
    //        switch (_PlayerType)
    //        {
    //            case PlayerType.PlayerBall:
    //                ObstacleTriggered = false;
    //                break;
    //        }

    //    }
    //}
}
