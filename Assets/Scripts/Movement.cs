using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Movement : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    public float speed = 1, jump = 10;
    [SerializeField] GroundChecker checker;
    [SerializeField] Transform flipTarget;
    public bool isRight;
    PhotonView pv;
    float jumpDelay;
    public Clip jumpClip;
    void Start()
    {
        pv = GetComponent<PhotonView>();

        StartRaundFlip();
    }
    public void StartRaundFlip()
    {
        // if (PhotonNetwork.IsMasterClient)
        // {
        //     if (PhotonNetwork.IsMasterClient) Flip(true);
        //     else Flip(false);
        // }

        if (PhotonNetwork.IsMasterClient)
        {
            if (pv.IsMine)
            {
                Flip(true);
            }
            else
            {
                Flip(false);
            }
        }
      
        else
        {
            if (pv.IsMine)
            {
                Flip(false);
            }
            else
            {
                Flip(true);
            }
        }
       // if (PhotonNetwork.IsMasterClient && pv.IsMine) { flipTarget.localScale = new Vector3(1, 1, -1); Logger.Log("Хост вправо"); }
       // if (!PhotonNetwork.IsMasterClient && !) { flipTarget.localScale = new Vector3(1, 1, 1); Logger.Log("Другой влево"); }




    }


    void Flip(bool right)
    {
        if (right)
        {
            if (isRight == false)
            {
                pv.RPC("RPC_FLIP", RpcTarget.All, right);
            }

        }
        else
        {
            if (isRight == true)
            {
                pv.RPC("RPC_FLIP", RpcTarget.All, right);
            }
        }

        if (right != isRight)
        {

        }

    }
    [PunRPC]
    public void RPC_FLIP(bool right)
    {
        isRight = right;
        // if (right) flipTarget.localScale = new Vector3(1, 1, 1);
        // else flipTarget.localScale = new Vector3(-1, 1, 1);
        if (right) flipTarget.localScale = new Vector3(1, 1, -1);
        else flipTarget.localScale = new Vector3(1, 1, 1);

    }
    void Update()
    {

        if (!pv.IsMine) return;
        jumpDelay -= Time.deltaTime; ;
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("SpeedY", rb.velocity.y);
        anim.SetBool("IsGround", checker.isGround);
    }
    public void JumpBtn()
    {
        // Debug.Log("JumpBtn()");
        if (!checker.isGround || jumpDelay > 0) return;
        // Debug.Log("Jchecker.isGround"+ checker.isGround);
        SoundCreator.Create(jumpClip);
        jumpDelay = 0.5f;
        rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        // Debug.Log("jump");
        checker.isGround = false;

    }

    public void Move(Vector2 dir)
    {
        if (dir.y > 0.8f) JumpBtn();
        if (!checker.isGround) dir.x /= 1.5f;
        dir.y = rb.velocity.y;
        rb.velocity = new Vector2(dir.x * speed, dir.y);
        if (dir.x > 0) Flip(true);
        else if (dir.x < 0) Flip(false);
    }
}
