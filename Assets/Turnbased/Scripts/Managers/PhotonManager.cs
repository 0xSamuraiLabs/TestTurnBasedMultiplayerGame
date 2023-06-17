using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject matchingPanel;
    [SerializeField] private TMP_Text matchingText;
    [SerializeField] private Button searchButton;
    
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        if (PhotonNetwork.IsConnectedAndReady)
        {
            searchButton.interactable = true;
        }
        Debug.Log("CONNNECTED");
    }
    
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        searchButton.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if (matchingPanel)
        {
            matchingPanel.SetActive(false);
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        matchingPanel.SetActive(true);
        CheckForMaxPlayers();
    }

    public void LoadScene()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        PhotonNetwork.CurrentRoom.MaxPlayers = 2;
        matchingPanel.SetActive(true);
        matchingText.text = "Searching For Opponent...";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        CheckForMaxPlayers();
    }

    void CheckForMaxPlayers()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Max Players Load another scene");
            matchingText.text = "Opponent Found";
            Invoke(nameof(LoadScene),1f);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        matchingText.text = "Searching For Opponent...";
        matchingPanel.SetActive(false);
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void CancelSearch()
    {
        matchingPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }
    
}
