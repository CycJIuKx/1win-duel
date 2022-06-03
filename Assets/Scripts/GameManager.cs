using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public System.Action OnAllConnected, OnRoundEnd;
    public GameObject playerPref;
    public int myActor, otherActor;
    PhotonView pv;
    public bool debug = true;
    public Text ping;
    public Transform masterSpawnPoint, otherSpawnPoint;

    public bool isGameProcess;
   
    public const byte ON_RESTART = 2;
    private void Awake()
    {
        instance = this;
        PhotonNetwork.SerializationRate = 50;
    }
    void Start()
    {

        pv = GetComponent<PhotonView>();
        myActor = PhotonNetwork.LocalPlayer.ActorNumber;

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable()
        {
            {"ConnectToGame", true}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        if (pv.IsMine) StartCoroutine(connectCheck());
        OnAllConnected += () =>
        {
            Vector3 pos = GetSpawnPoint();
            GameController.instance.SetPlayer(PhotonNetwork.Instantiate(playerPref.name, pos, Quaternion.identity));

         
              StartCoroutine(delaySearthEnemy());
        };
        StartCoroutine(fpscor());
        Server.Log("Game started acror-"+myActor);
    }


    IEnumerator connectCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (debug)
            {
                pv.RPC("RPC_StartGame", RpcTarget.All); yield break;
            }
            int count = 0;
            foreach (var item in PhotonNetwork.PlayerList)
            {
                Debug.Log("Check " + (bool)item.CustomProperties["ConnectToGame"] + "" + item.ActorNumber);
                if ((bool)item.CustomProperties["ConnectToGame"] == true)
                {
                    count++;
                }
            }
            if (count == 2) { pv.RPC("RPC_StartGame", RpcTarget.All); yield break; }
        }
    }
    [PunRPC]
    public void RPC_StartGame()
    {
        OnAllConnected.Invoke();
    }
    IEnumerator delaySearthEnemy()
    {
        yield return new WaitForSeconds(1);
      //  if (!debug) otherActor = PhotonNetwork.PlayerListOthers[0].ActorNumber;
        if (PhotonNetwork.IsMasterClient) otherActor = 2; else otherActor = 1;
     
        GameController.instance.SetEnemy(GetPlayerGObyActor(otherActor));
    }

  
 
   


    public GameObject GetPlayerGObyActor(int a)
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
        {
            //Logger.Log($"Найден обьект игрока "+item + "Его актер = "+ item.GetComponent<PhotonView>().Owner.ActorNumber);
            if (item.GetComponent<PhotonView>().Owner.ActorNumber == a) return item.gameObject;
        }
        Logger.Log($"Не нашли другого") ;
        return null;
    }
    public Vector3 GetSpawnPoint()
    {
        return (pv.Owner.ActorNumber == myActor) ? masterSpawnPoint.position : otherSpawnPoint.position;
    }
    IEnumerator fpscor()
    {
        float timer = 1f;
        float fps = 0;
        while (true)
        {
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                yield return null;
                fps++;
            }
            ping.text ="Ping: " +PhotonNetwork.GetPing() + " FPS: " + fps;
            timer = 1; fps = 0;

        }
    }
    public void QuitBtn()
    {
        Server.Log("QuitBtn-" + myActor);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        StartCoroutine(delay());
       
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GameController.instance.StopAllCoroutines();
        Server.Log("Other player lefr Room-" + myActor);
        EndWindow.instance.Open(true);
        Logger.Log("Apponent left the room");
    }
    IEnumerator delay()
    {
   
       
        yield return new WaitForSeconds(1);
        Application.LoadLevel("Menu");
    }
}
