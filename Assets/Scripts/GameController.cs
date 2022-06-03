using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameController : MonoBehaviour, IOnEventCallback
{
    public bool debug = false;
    public static GameController instance;
    public bool blockShoot;
    public GameObject Player, Enemy;
    PhotonView pv;
    int shots;

    public Animator camAnim, darkAnim;
    [SerializeField] CinemachineVirtualCamera cinemachine;
    [SerializeField] Text score;
    [SerializeField] Camera cam;

    public int kills, deaths;
    public int timerStart = 10;
    public System.Action OnRestart;
    Coroutine timerCor;
    public Text delayText;
    public int killsToWon = 3;
    public Clip killClip, deadClip;
    public Clip startROundClip, OnCheckDead;

    public Button shootBtn;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        pv = GetComponent<PhotonView>();
        GameManager.instance.OnAllConnected += OnAllLoaded;
        showScore();
        OnRestart += () =>
        {
            shots = 0;
            blockShoot = false;
            if (timerCor != null) StopCoroutine(timerCor);
            if (PhotonNetwork.IsMasterClient) timerCor = StartCoroutine(timer());
            if (PhotonNetwork.IsMasterClient) PhotonNetwork.CreateEvent(GameManager.ON_RESTART);
        };
        //PhotonNetwork.CreateEvent(1);
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnAllLoaded()
    {
        StartCoroutine(cutCor());
    }
    IEnumerator cutCor()
    {

        if (debug)
        {
            cinemachine.gameObject.SetActive(true);
            darkAnim.enabled = true;
            darkAnim.Play("DrakToLight");

            GameManager.instance.isGameProcess = true;
            cinemachine.Follow = Player.transform;
            if (PhotonNetwork.IsMasterClient) timerCor = StartCoroutine(timer());
            yield break;
        }
        camAnim.Play("camAwake");
        darkAnim.enabled = true;
        yield return new WaitForSeconds(4);
        SoundEffects.AddEffectsAllAudio(SoundEffects.Effects.lowPass, 2, 0.3f);
        darkAnim.Play("LightToDark");
        yield return new WaitForSeconds(2f);
        cinemachine.gameObject.SetActive(true);
        darkAnim.Play("DrakToLight");
        GameManager.instance.isGameProcess = true;
        cinemachine.Follow = Player.transform;
        if (PhotonNetwork.IsMasterClient) timerCor = StartCoroutine(timer());
    }
    IEnumerator timer()
    {
        SoundCreator.Create(startROundClip);
     
        float t = timerStart;
        while (t > 0)
        {
            t--;

            yield return new WaitForSeconds(1);
        }
      
        //if (GameManager.instance.isGameProcess)
        //{
        //
        //    StartCoroutine(delayDeadCheck());
        //    Server.Log("Timer end,restart actor- " + GameManager.instance.myActor);
        //}
        pv.RPC("RPC_TimerEnd", RpcTarget.All);

    }
    [PunRPC]
    public void RPC_TimerEnd()
    {
        if (GameManager.instance.isGameProcess)
        {

            StartCoroutine(delayDeadCheck("TEME LEFT"));
            Server.Log("Timer end,restart actor- " + GameManager.instance.myActor);
        }
    }
    public void OnEnemyKilled(GameObject go)
    {
        pv.RPC("RPC_OnEnemyKilled", RpcTarget.All, go.GetComponent<PhotonView>().CreatorActorNr);
    }

    public void OnShoot()
    {
        pv.RPC("RPC_shootAdd", RpcTarget.All);
       
    }
    [PunRPC]
    public void RPC_shootAdd()
    {
        shots++;
     
        if (shots == 2)
        {
            shots = 0;

            StartCoroutine(delayShotCheck());
        }
    }
    IEnumerator delayShotCheck()
    {
        yield return new WaitForSeconds(2);
        if (GameManager.instance.isGameProcess) StartCoroutine(delayDeadCheck("AMMO SPENT"));
    }

    [PunRPC]
    public void RPC_OnEnemyKilled(int actor)
    {

        Player.GetComponent<PlayerController>().Reset();
        if (Enemy == null) Enemy = GameManager.instance.GetPlayerGObyActor(GameManager.instance.otherActor);
     
        if (PhotonNetwork.LocalPlayer.ActorNumber == actor)//если убили меня
        {
            SoundCreator.Create(deadClip);
            StartCoroutine(camEffect(false));
            deaths++;
            Player.GetComponent<PlayerController>().Die();
        }
        else
        {
            SoundCreator.Create(killClip);
            StartCoroutine(camEffect(true));
            kills++;
            Enemy.GetComponent<PlayerController>().Die();
        }
        showScore();
        if (GameManager.instance.isGameProcess) StartCoroutine(delayDeadCheck("DEATH..."));
        Server.Log("Enemy killed, actor " + GameManager.instance.myActor);
        Server.Log($"Kills:{kills} deaths:{deaths} actor: " + GameManager.instance.myActor);

    }
    IEnumerator camEffect(bool isEnemyKilled)
    {
        Time.timeScale = 0.3f;
        SoundEffects.AddEffectsAllAudio(SoundEffects.Effects.slow, 3);
        cinemachine.gameObject.SetActive(false);
        cam.enabled = false;

        Transform targetCam = null;
        if (isEnemyKilled) targetCam = Enemy.transform.Find("Cam");
        else targetCam = Player.transform.Find("Cam");
        if (isEnemyKilled) targetCam = Enemy.GetComponent<PlayerController>().innerCam;
        else targetCam = Player.GetComponent<PlayerController>().innerCam;
        targetCam.gameObject.SetActive(true);

        targetCam.GetComponent<Animator>().SetTrigger("Go");
        yield return new WaitForSeconds(1.4f);

        cam.enabled = true;
        cinemachine.gameObject.SetActive(true);
        targetCam.gameObject.SetActive(false);
        cinemachine.Follow = Player.transform;
        Time.timeScale = 1f;
    }
    IEnumerator delayDeadCheck(string reason)
    {
       // Logger.Log("КОнец раунда - причина : " + reason);
        Helpers.UI.UiCreator.instance.CreateMiddleText(reason);
        GameManager.instance.isGameProcess = false;
        SoundCreator.Create(OnCheckDead);
        blockShoot = true;
        Player.GetComponent<PlayerController>().Reset();
        yield return new WaitForSeconds(2);

      

        if (kills == killsToWon || deaths == killsToWon)
        {
            darkAnim.Play("LightToDark");
            yield return new WaitForSeconds(2f);
            if (kills == killsToWon)
                EndWindow.instance.Open(true);
            else if (deaths == killsToWon)
                EndWindow.instance.Open(false);
            darkAnim.Play("DarkToLightFast");

        }
        else
        {
            SoundEffects.AddEffectsAllAudio(SoundEffects.Effects.lowPass, 1, 0.3f);
            darkAnim.Play("LightToDark");
            yield return new WaitForSeconds(1f);



            StopAllCoroutines();
            OnRestart?.Invoke();
            GameManager.instance.isGameProcess = true;
            darkAnim.Play("DrakToLightFast");



        }



    }


    void showScore()
    {
        score.text = $"<color=red>Die:{deaths} </color> <color=blue>Kills:{kills} </color>";
    }
    public void SetEnemy(GameObject go)
    {
        Enemy = go;
    }
    public void SetPlayer(GameObject go)
    {
        Player = go;
    }

    public void OnEvent(EventData photonEvent)
    {
        //  Logger.Log(photonEvent.Code+" n");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(camEffect(false));
        }
    }
}
