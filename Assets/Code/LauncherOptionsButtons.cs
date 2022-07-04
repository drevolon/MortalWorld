using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LauncherOptionsButtons : MonoBehaviour
{
    [SerializeField]
    private Button _signInButton;

    [SerializeField]
    private Button _signOutButton;

    Launcher launcherStart;

    private void Start()
    {
        _signInButton.onClick.AddListener(SignIn);
        _signOutButton.onClick.AddListener(SignOut);
        launcherStart = new Launcher();
    }

    private void OnDestroy()
    {
        _signInButton.onClick.RemoveAllListeners();
        _signOutButton.onClick.RemoveAllListeners();
    }

    private void SignOut()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("User left from Lobby");
        
    }

    private void SignIn()
    {
        launcherStart.Connect();
    }
}
