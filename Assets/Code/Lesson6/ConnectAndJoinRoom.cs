using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class ConnectAndJoinRoom : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    [SerializeField]
    private ServerSettings _serverSettings;

    [SerializeField]
    private TMP_Text _stateUiText;

    [SerializeField]
    private TMP_Dropdown _roomList;

    [SerializeField]
    private Button _exitButton;

    private const string GAME_MOD_KEY = "gm";
    private const string AI_MOD_KEY = "ai";

    LoadBalancingClient _lbc;

    private void Start()
    {
        _exitButton.onClick.AddListener(OnExitRoom);


        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);

        if (!_lbc.ConnectUsingSettings(_serverSettings.AppSettings))
            Debug.Log("Error Conncted");
    }

    private void OnExitRoom()
    {
        var roomOptions = new RoomOptions
        {
            IsVisible = false,
            IsOpen = false,
            PublishUserId = true
        };
        var enterRoomParams = new EnterRoomParams { RoomName = "First Room", RoomOptions = roomOptions };
        _lbc.OpCreateRoom(enterRoomParams);
    }

    private void Update()
    {
        if (_lbc == null) return;

        _lbc.Service();

        var state = _lbc.State.ToString();
        _stateUiText.text = string.Format("State {0} userID {1}", state, _lbc.UserId);

        //_lbc.OpGetGameList();
    }

    private void OnDestroy()
    {
        _lbc.RemoveCallbackTarget(this);
    }

    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster active");
        //_lbc.OpJoinRandomRoom();
        var roomOptions = new RoomOptions {
            MaxPlayers = 12,
            CustomRoomProperties = new Hashtable { { GAME_MOD_KEY, 1 } },
            IsVisible = true,
            PublishUserId = true
        };
        var enterRoomParams = new EnterRoomParams { RoomName="First Room", RoomOptions = roomOptions };
        _lbc.OpCreateRoom(enterRoomParams);
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed");
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("OnCustomAuthenticationFailed");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }

    public void OnDisconnected(DisconnectCause cause)
    {
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");

        //var opJoinRandomRoomParams = new OpJoinRandomRoomParams
        //{

        //};

        //_lbc.OpJoinRandomRoom(opJoinRandomRoomParams);

       // _lbc.CurrentLobby.
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        _lbc.OpCreateRoom(new EnterRoomParams());
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed");
    }

    public void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }

    public void OnLeftRoom()
    {
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate active");

        foreach (var itemRoom in roomList)
        {
            var nameRom = itemRoom.Name;
            TMPro.TMP_Dropdown.OptionData item = new TMP_Dropdown.OptionData ();
            item.text = nameRom;
            
            _roomList.options.Add(item);

            Debug.Log($"room: {itemRoom}");
        }     

        
    }
}
