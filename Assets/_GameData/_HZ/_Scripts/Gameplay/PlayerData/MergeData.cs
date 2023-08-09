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
        if (_PlayerType == PlayerType.OtherBalls)
        {
            MeshFilter = gameObject.GetComponent<MeshFilter>();

        }

        //if (MyCollider == null)
        //{
        //    MyCollider = this.gameObject.AddComponent<MeshCollider>();
        //}
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
    [SerializeField] MeshCollider MyCollider;
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
    [SerializeField] AudioClip SplitSound;
    [SerializeField] AudioClip MergeSound;
    [SerializeField] MeshFilter MeshFilter;
    [SerializeField] MaterialPropertyBlock Mpb;
    [SerializeField] GameObject[] EnjectedBalls;
    [SerializeField] Mesh[] BallMeshes;
    [SerializeField] Color[] EnjectedBallsColors;

    public DOTweenAnimation MergeParticle;
    public Rigidbody MyRgd;
    public TrailRenderer MyTrailRenderer;

    private void Update()
    {
        if (_PlayerType == PlayerType.PlayerBall)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2f))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.CompareTag(MergeBallTag))
                {
                    // The ray hit an object with the specified tag
                    IsMergeable(hitObject.GetComponent<MergeData>());
                    Debug.Log("FromStay");

                    // You can now perform actions or access properties of the hitObject
                    // For example, you can access its transform, components, etc.
                }
            }
        }
    }

    bool collided;
    public void IsMergeable(MergeData playerRankState)
    {
        collider = true;
        if (playerRankState != null)
        {
            Debug.Log($"{playerRankState.gameObject.name} : Player_Name");
            Debug.Log($"{playerRankState.BallIndex} : Player");
            if (BallIndex == playerRankState.BallIndex)
            {
                //Mergeable = true;
                BallIndex++;

                Debug.Log($"{_PlayerType} : PlayerType");
                Debug.Log("CallTween");
                //  MyColor = RefMergeData.MyColor;
                if (_PlayerType == PlayerType.PlayerBall)
                {

                    MyAudioSource.clip = MergeSound;
                    MyAudioSource.Play();
                    StartCoroutine(WaitforScaleAnimation());
                    //  MyTrailRenderer.startColor = MyColor;
                    MergeParticle.gameObject.SetActive(true);
                    MergeParticle.DOPlay();
                    if (BallIndex < BallMeshes.Length)
                    {
                        MyCollider.sharedMesh = BallMeshes[BallIndex];
                        //MeshFilter.gameObject.transform.position = new Vector3(MeshFilter.gameObject.transform.position.x, MeshFilter.gameObject.transform.position.y + 0.1f, MeshFilter.gameObject.transform.position.z);
                        MeshFilter.mesh = BallMeshes[BallIndex];
                    }

                }
               
                    //MeshFilter.gameObject.transform.position = new Vector3(MeshFilter.gameObject.transform.position.x, MeshFilter.gameObject.transform.position.y + 0.1f, MeshFilter.gameObject.transform.position.z);
                  
                    Destroy(playerRankState.gameObject);
                


                _PlayerRankStages = playerRankState._PlayerRankStages;
                playerRankState.GetComponent<MeshRenderer>().enabled = false;

            }
            else
            {
                if (RefMergeData != null)
                {
                    RefMergeData.MyRgd.freezeRotation = true;
                    RefMergeData.MyRgd.AddForce(Vector3.back * 70f);
                    RefMergeData.MyRgd.freezeRotation = false;
                }
                //Mergeable = false;

            }
        }
    }
    IEnumerator WaitforScaleAnimation()
    {

        transform.DOScale(new Vector3(1.3f,
                                         1.3f,
                                             1.3f), .2f);
        yield return new WaitForSeconds(0.25f);

        ChangeScale();
    }
    IEnumerator ReduceScaleAnimation()
    {

        transform.DOScale(new Vector3(.9f,
                                      .9f,
                                          .9f), .2f);
        yield return new WaitForSeconds(0.4f);
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

    bool collider = false;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag(MergeBallTag))
        {
            RefMergeData = collision.gameObject.GetComponent<MergeData>();

            IsMergeable(RefMergeData);
            if (_PlayerType == PlayerType.PlayerBall && !collided)
            {
                MyAudioSource.clip = NotMergeSound;
                MyAudioSource.Play();

            }
        }
    }

    private void OnCollisionStay(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag(MergeBallTag))
        {
            collided = true;
        }
    }
    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag(MergeBallTag) && collider)
        {
            collided = false;
            collider = false;
        }
    }
    private void OnDrawGizmos()
    {
        if (transform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * 2); // Draw the ray for visualization
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
                        MyAudioSource.clip = SplitSound;
                        MyAudioSource.Play();
                        Handheld.Vibrate();
                        //  ObstacleTriggered = true;
                        other.GetComponent<Collider>().enabled = false;

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
                Hz.Gameplay.GameManager.instance.StageFailed();
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
