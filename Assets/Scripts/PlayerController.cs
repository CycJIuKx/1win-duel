using System.Collections;
using UnityEngine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour, IPunObservable
{
    public bool IsDead;
    [SerializeField] Weapone weapone;
    [SerializeField] Animator anim;
    Joystick joystick;
    public Vector2 dir = new Vector2();
    Movement movement;
    public float testAngle = 0;
    PhotonView pv;
    Coroutine cor;
    public Transform innerCam;
    bool shootProcess;

    Slots.ShootStruct curShot;
    public Clip rollClip, preShoot;
    public List<Clip> steps;
   // [SerializeField] SpriteRenderer heat;
   [ContextMenu("Fire")]
   public void SetFire()
    {
        pv.RPC("RPC_fire", RpcTarget.All);
    }
    [PunRPC]
    public void RPC_fire()
    {
        Logger.Log("fire");
        transform.Find("Fire").GetComponent<ParticleSystem>().Play();
    }
    public void PlayStep()
    {
        SoundCreator.Create(steps.GetRandomElement());
    }
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        movement = GetComponent<Movement>();
        if (pv.IsMine)
        {
            GameController.instance.shootBtn.onClick.AddListener(() => Shoot());
            InvokeRepeating("sync", 1, 1);
        }
        else
        {
            transform.Find("Arrow").gameObject.SetActive(true);
        }
        GameController.instance.OnRestart += OnRoundRestart;

        if (!pv.IsMine)
        {
            Destroy(GetComponent<Rigidbody2D>());
            return;
        }
        joystick = GameObject.FindObjectOfType<Joystick>();


        Slots.instance.onRolled += (r) => { cor = StartCoroutine(Shoot(r)); };



    }
    void sync()
    {
        pv.RPC("RPC_sync", RpcTarget.All, transform.position.x, transform.position.y);
    }
    [PunRPC]
    void RPC_sync(float x, float y)
    {

        Vector3 pos = new Vector3(x, y, 0);
        transform.position = pos;
    }
    public void Die()
    {
        if (GameController.instance.Enemy.transform.position.x > transform.position.x)
        {
            if (movement.isRight) anim.SetBool("IsFront", true);
            else anim.SetBool("IsFront", false);
        }
        else//противник слева
        {
            if (movement.isRight) anim.SetBool("IsFront", false);
            else anim.SetBool("IsFront", true);
        }
        //если противник справа
        shootProcess = false;
        weapone.DisableLine();
        GameController.instance.delayText.text = "";
        anim.SetLayerWeight(1, 0);
        if (cor != null) StopCoroutine(cor);
        anim.SetBool("Die", true);
        IsDead = true;
    }
    public void OnRoundRestart()
    {
        // if (cor != null) StopCoroutine(cor);
        shootProcess = false;
        weapone.DisableLine();
        StopAllCoroutines();
        GameController.instance.delayText.text = "";
        anim.SetLayerWeight(1, 0);
        anim.SetBool("Die", false);
        IsDead = false;
        transform.position = GameManager.instance.GetSpawnPoint();
        movement.StartRaundFlip();
    }
    public void Reset()
    {
        weapone.DisableLine();
        shootProcess = false;
        anim.SetLayerWeight(1, 0);
        GameController.instance.delayText.text = "";
        StopAllCoroutines();
    }
    [PunRPC]

    public void RPC_move(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
    }



    void Update()
    {
        if (!pv.IsMine || IsDead || !GameManager.instance.isGameProcess) return;
        if (shootProcess)
        {
            weapone.SetLine(curShot, movement.isRight);

        }
        if (Input.GetKeyDown(KeyCode.Space)) movement.JumpBtn();
        if (Input.GetMouseButtonDown(1)) { Shoot(); }
    }
    public void Shoot()
    {
        if (GameController.instance.blockShoot == false) { Slots.instance.StartRoll(); StartCoroutine(rollCor()); }
    }
    IEnumerator rollCor()
    {
        SoundCreator.Create(rollClip);
        anim.SetBool("Roll", true);
        while (anim.GetLayerWeight(1) != 1)
        {
            anim.SetLayerWeight(1, Mathf.MoveTowards(anim.GetLayerWeight(1), 1, 10 * Time.deltaTime));
            yield return null;
        }
        yield return new WaitForSeconds(1.8f);
        while (anim.GetLayerWeight(1) != 0)
        {
            anim.SetLayerWeight(1, Mathf.MoveTowards(anim.GetLayerWeight(1), 0, 10 * Time.deltaTime));
            yield return null;
        }
        anim.SetBool("Roll", false);
    }
    IEnumerator Shoot(Slots.ShootStruct shot)
    {
        SoundCreator.Create(preShoot);

        curShot = shot;
        shootProcess = true;
        int shots = (int)shot.count + 1;
        float delay = (int)shot.delay + 1;
        GameController.instance.delayText.text = "" + Math.Round(delay, 1);
        while (delay > 0)
        {
            delay -= 0.1f;
            yield return new WaitForSeconds(0.1f);
            GameController.instance.delayText.text = "" + Math.Round(delay, 1);
        }

        GameController.instance.delayText.text = "";
        if (IsDead) yield break;
        anim.SetTrigger("Shoot");
        while (anim.GetLayerWeight(1) != 1)
        {
            anim.SetLayerWeight(1, Mathf.MoveTowards(anim.GetLayerWeight(1), 1, 10 * Time.deltaTime));
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        if (IsDead) yield break;
        Slots.instance.OnShoot();
        //  SoundEffects.AddEffectsAllAudio(SoundEffects.Effects.lowPass, 1,0.3f);
        weapone.DisableLine();
        shootProcess = false;
        for (int i = 0; i < shots; i++)
        {
            weapone.Shoot(shot, movement.isRight);
            yield return new WaitForSeconds(0.5f);
        }

        while (anim.GetLayerWeight(1) != 0)
        {
            anim.SetLayerWeight(1, Mathf.MoveTowards(anim.GetLayerWeight(1), 0, 10 * Time.deltaTime));
            yield return null;
        }
    }


    private void FixedUpdate()
    {
        if (!pv.IsMine || IsDead || !GameManager.instance.isGameProcess) return;

        dir = joystick.Direction;
        if (Input.GetKey(KeyCode.A)) dir.x -= 1;
        if (Input.GetKey(KeyCode.D)) dir.x += 1;
        if (Input.GetKey(KeyCode.W)) dir.y += 1;


        if (dir != Vector2.zero) movement.Move(dir);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
