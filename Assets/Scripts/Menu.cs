using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
public class Menu : MonoBehaviourPunCallbacks
{
    public Text waitText;
    string playerName;
    public GameObject waitWindow, startWindow;
    public Clip shot, back,start;
    void Start()
    {
        Server.Log("Menu started");
        playerName = "Player " + Random.Range(1000, 10000);

    }
    public void ConnectButton()
    {
        SoundCreator.Create(shot);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
       
    }
    public void ReturnToMenu()
    {
        SoundCreator.Create(back);
        waitWindow.SetActive(false);
        startWindow.SetActive(true);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }
    public override void OnLeftRoom()
    {
        //  Logger.Log("OnLeftRoom()");
        Debug.Log(" OnLeftRoom()");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster()");
        PhotonNetwork.JoinRandomOrCreateRoom();
        //  PhotonNetwork.JoinRandomOrCreateRoom(null, 2);
        //  PhotonNetwork.JoinRandomRoom(null, 2);
        startWindow.SetActive(false);
        waitWindow.SetActive(true);
        waitText.text = "Waiting for apponent...";
    }
    public override void OnJoinedLobby()
    {
        // Logger.Log("OnJoinedLobby()");
        Debug.Log("OnJoinedLobby()");
        PhotonNetwork.JoinRandomRoom(null, 2);
        startWindow.SetActive(false);
        waitWindow.SetActive(true);
    }
    public override void OnJoinedRoom()
    {
        //  Logger.Log("OnJoinedRoom() players "+ PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.Log("OnJoinedRoom()");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) { waitText.text = "Starting..."; StartGame(); Server.Log("After wait starting ac "+PhotonNetwork.LocalPlayer.ActorNumber); }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //   Logger.Log("OnPlayerEnteredRoom players " + PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.Log("OnPlayerEnteredRoom");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) { waitText.text = "Starting..."; StartGame(); Server.Log("After wait starting ac "+ PhotonNetwork.LocalPlayer.ActorNumber); }

    }
    void StartGame()
    {
        StartCoroutine(startDelay());

    }
    IEnumerator startDelay()
    {
        SoundCreator.Create(start);
        yield return new WaitForSeconds(0.1f);
       
        Debug.Log("StartGame");
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("Game");
    }
    public void FORCE_start()
    {
        StartCoroutine(startDelay());
        // PhotonNetwork.CurrentRoom.IsOpen = false;
        // PhotonNetwork.CurrentRoom.IsVisible = false;

        // PhotonNetwork.LoadLevel("Game");
    }







}
