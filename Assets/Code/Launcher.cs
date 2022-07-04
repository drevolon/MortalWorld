using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion;

    public Launcher()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        gameVersion = "1";
    }

    void Awake()
    {
       // PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        Connect();
    }

    public void Connect()
    {

        if (PhotonNetwork.IsConnected)
        {

            PhotonNetwork.JoinRandomRoom();
            Debug.Log("JoinRandomRoom");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            Debug.Log("ConnectUsingSettings");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");
    }



}
