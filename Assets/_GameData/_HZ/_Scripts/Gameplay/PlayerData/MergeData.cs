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
        if (_PlayerType == PlayerType.OtherBalls)
        {
            CheckRigidBody();
        }
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
            MeshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        if (MyCollider == null)
        {
            MyCollider = this.gameObject.AddComponent<MeshCollider>();
            MyCollider.convex = true;
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
    [SerializeField] MeshRenderer MeshRenderer;
    [SerializeField] Material[] BallMaterial;

    public DOTweenAnimation MergeParticle;
    public Rigidbody MyRgd;
    public TrailRenderer MyTrailRenderer;
    bool collider = false;
    bool collided;
    bool IsMerged;
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
                    return;
                    // You can now perform actions or access properties of the hitObject
                    // For example, you can access its transform, components, etc.
                }
            }
        }
    }


    public void IsMergeable(MergeData playerRankState)
    {
        collider = true;
        if (playerRankState != null)
        {
            Debug.Log($"{playerRankState.gameObject.name} : Player_Name");
            Debug.Log($"{playerRankState.BallIndex} : Player");
            if (BallIndex == playerRankState.BallIndex)
            {
                IsMerged = true;
                //Mergeable = true;
                BallIndex++;
                //MeshRenderer.material = BallMaterial[BallIndex];

                //MeshRenderer.materials[1] = BallMaterial[BallIndex];
                Debug.Log($"{_PlayerType} : PlayerType");
                Debug.Log("CallTween");
                //  MyColor = RefMergeData.MyColor;
                if (_PlayerType == PlayerType.PlayerBall)
                {
                    MyAudioSource.PlayOneShot(MergeSound);
                  
                    StartCoroutine(WaitforScaleAnimation());
                 
                    if (BallIndex < BallMeshes.Length)
                    {
                        MyCollider.sharedMesh = BallMeshes[BallIndex];
                        //MeshFilter.gameObject.transform.position = new Vector3(MeshFilter.gameObject.transform.position.x, MeshFilter.gameObject.transform.position.y + 0.1f, MeshFilter.gameObject.transform.position.z);
                        MeshFilter.mesh = BallMeshes[BallIndex];
                        playerRankState.gameObject.SetActive(false);
                    }

                }
                else if (_PlayerType == PlayerType.OtherBalls)
                {
                    if (BallIndex < BallMeshes.Length)
                    {

                        MeshFilter.mesh = BallMeshes[BallIndex];
                        playerRankState.gameObject.SetActive(false);
                    }
                }
                _PlayerRankStages = playerRankState._PlayerRankStages;
                playerRankState.GetComponent<MeshRenderer>().enabled = false;

            }
            else
            {
                IsMerged = false;
            }
        }
    }

    public void UpgrageBall()
    {
        if (BallIndex < BallMeshes.Length - 1)
        {
            BallIndex++;
            MeshFilter.mesh = BallMeshes[BallIndex];
        }
        Hz.Gameplay.GameManager.instance.MainScreen.SetActive(false);
        if (_PlayerType == PlayerType.PlayerBall)
        {
            PlayerMovement.FollowPath.enabled = true;
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



    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag(MergeBallTag))
        {
            RefMergeData = collision.gameObject.GetComponent<MergeData>();

            IsMergeable(RefMergeData);
            if (!IsMerged)
            {
                MyAudioSource.PlayOneShot(NotMergeSound);
            }
            if (_PlayerType == PlayerType.PlayerBall)
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(Vector3.forward * 1.5f);
                rb.AddForce(Vector3.up * 1.5f);
            }
        }
    }
    //Coroutine _StopCoroutine;


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
    bool ObstacleTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ObstacleTag) && !PlayerMovement.ObstacleCross)
        {

            if (BallIndex != 0)
            {
                switch (_PlayerType)
                {
                    case PlayerType.PlayerBall:
                        other.GetComponent<Collider>().enabled = false;
                        //MyAudioSource.clip = SplitSound;
                        MyAudioSource.PlayOneShot(SplitSound);
                        Handheld.Vibrate();
                        //  ObstacleTriggered = true;

                        Debug.Log($"{other.name} : Object that been collided");
                        BallIndex--;
                        Instantiate(EnjectedBalls[BallIndex], new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
                        MyTrailRenderer.startColor = MyColor;
                        MeshFilter.mesh = BallMeshes[BallIndex];
                        StartCoroutine(ReduceScaleAnimation());
                        break;
                    case PlayerType.OtherBalls:

                        break;
                    default:
                        break;
                }
            }
            else if (BallIndex == 0 && _PlayerType == PlayerType.PlayerBall)
            {

                Hz.Gameplay.GameManager.instance.StageFailed();

                Debug.Log($"{other.name} : Object that been collided : {gameObject.name} = my name");

            }


        }
        if (other.gameObject.CompareTag("Failed") && _PlayerType == PlayerType.OtherBalls)
        {
            Debug.Log($"{other.gameObject.name} : Ball Name ");
            gameObject.SetActive(false);
        }
    }
}
